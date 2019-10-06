using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dude : MonoBehaviour
{
    enum HappienessState
    {
        Undefined,
        Angry,
        Neutral,
        Happy
    }
    public Faction faction;
    public float happiness;

    private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer mouthSpriteRenderer;

    [SerializeField] private float animateInDuration;
    [SerializeField] private AnimationCurve animateInCurve;
    [SerializeField] private float animateOutDuration;
    [SerializeField] private AnimationCurve animateOutCurve;

    private Coroutine happienessCoroutine;
    private Coroutine despawnCoroutine;
    public bool Despawning => despawnCoroutine != null;

    private float baseScale;
    private Quaternion baseRotation;

    [SerializeField] private Sprite mouthSad;
    [SerializeField] private Sprite mouthNeutral;
    [SerializeField] private Sprite mouthHappy;

    [Header("Happy Animation")]
    [SerializeField] private float swayingSpeed;
    [SerializeField] private float swayingIntensity;
    [SerializeField] private AnimationCurve swayingAnimCurvePosition;

    [Header("Angry Animation")]
    [SerializeField] private float angryStompingSpeed;
    [SerializeField] private float angryStompingIntensity;
    [SerializeField] private AnimationCurve angryScaleAnimCurveRotation;

    [Header("Happieness Values")]
    [SerializeField] private float minHappienessForAngry;
    [SerializeField] private float minHappienessForNeutral;
    [SerializeField] private float minHappienessForHappy;

    private HappienessState happienessState;

    private void Awake()
    {
        baseScale = transform.localScale.x;
        baseRotation = transform.rotation;
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Spawn();
        SetHappienessState();
        //  Debug.Log("happiness" + happiness);
    }

    void Update()
    {
        SetHappienessState();
    }

    private void Spawn()
    {
        Color color = faction.tileColor;
        color.a = 1f;
        color.r += .05f;
        color.g += .05f;
        color.b += .05f;
        spriteRenderer.color = color;
        transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        var localPos = transform.localPosition;
        localPos.y = 1f;
        transform.localPosition = localPos;
        transform.localScale = Vector3.zero;
        animateInDuration += UnityEngine.Random.Range(-animateInDuration / 2f, animateInDuration / 2f);
        StartCoroutine(AnimationUtility.ScaleGameObjectRoutine(transform, Vector3.one * baseScale, animateInDuration, animateInCurve));
    }

    public void Despawn()
    {
        if (Despawning)
            return;
        if (happienessCoroutine != null)
        {
            StopCoroutine(happienessCoroutine);
            happienessCoroutine = null;

        }
        despawnCoroutine = StartCoroutine(DespawnRoutine());
    }

    private void SetHappienessState()
    {
        if (Despawning)
            return;
        HappienessState newState = HappienessState.Undefined;
        // Debug.Log(happiness);
        if (happiness <= minHappienessForAngry)
            newState = HappienessState.Angry;
        else if (happiness > minHappienessForAngry && happiness < minHappienessForHappy)
            newState = HappienessState.Neutral;
        else if (happiness >= minHappienessForHappy)
            newState = HappienessState.Happy;

        if (newState != happienessState)
        {
            if (happienessCoroutine != null)
            {
                StopCoroutine(happienessCoroutine);
                happienessCoroutine = null;
                ResetDudeTransform();
            }
            happienessState = newState;
            switch (happienessState)
            {
                case HappienessState.Angry:
                    mouthSpriteRenderer.sprite = mouthSad;
                    happienessCoroutine = StartCoroutine(AngryStompingRoutine());
                    break;
                case HappienessState.Neutral:
                    mouthSpriteRenderer.sprite = mouthNeutral;
                    // happienessCoroutine = StartCoroutine(HappySwayingRoutine());
                    break;
                case HappienessState.Happy:
                    mouthSpriteRenderer.sprite = mouthHappy;
                    happienessCoroutine = StartCoroutine(HappySwayingRoutine());
                    break;
                default:
                    Debug.LogError("SetHappienessState happienessState default");
                    break;
            }
        }
    }

    private void ResetDudeTransform()
    {
        transform.localScale = Vector3.one * baseScale;
        transform.localRotation = baseRotation;
    }

    private IEnumerator DespawnRoutine()
    {
        int steps = Mathf.RoundToInt(animateOutDuration / Time.fixedDeltaTime);
        if (steps < 2)
            steps = 2;
        float progress = 0f;
        Vector3 startScale = transform.localScale;
        for (int i = 0; i < steps; i++)
        {
            progress = AnimationUtility.Remap(i, 0, steps - 1, 0f, 1f);
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, animateOutCurve.Evaluate(progress));
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z + 15f);
            yield return new WaitForFixedUpdate();
        }
        despawnCoroutine = null;
        Destroy(gameObject);
    }

    private IEnumerator HappySwayingRoutine()
    {
        float progress = 0f;
        Vector2 startingScale = Vector2.one * baseScale;
        Vector2 targetScale = startingScale * .75f;
        Vector3 originalRotation = transform.localRotation.eulerAngles;
        Quaternion baseRot = Quaternion.Euler(90f, 0f, -swayingIntensity);
        Quaternion targetRot = Quaternion.Euler(90f, 0f, swayingIntensity);
        while (true)
        {
            progress = Mathf.PingPong(Time.time * swayingSpeed, 1f);
            transform.localScale = Vector3.Lerp(startingScale, targetScale, swayingAnimCurvePosition.Evaluate(progress));
            transform.localRotation = Quaternion.Lerp(baseRot, targetRot, progress);
            yield return null;
        }
    }

    private IEnumerator AngryStompingRoutine()
    {
        float progress = 0f;
        Vector2 startingScale = Vector2.one * baseScale;
        Vector2 targetScale = startingScale * .75f;
        Vector3 startPos = transform.localPosition;
        Vector3 targetPos = startPos + new Vector3(0f, 0f, angryStompingIntensity);
        while (true)
        {
            progress = Mathf.PingPong(Time.time * angryStompingSpeed, 1f);
            transform.localScale = Vector3.Lerp(startingScale, targetScale, angryScaleAnimCurveRotation.Evaluate(progress));
            transform.localPosition = Vector3.Lerp(startPos, targetPos, angryScaleAnimCurveRotation.Evaluate(progress));
            // transform.localRotation = Quaternion.Lerp(baseRot, targetRot, progress);
            yield return null;
        }
    }
}

public static class AnimationUtility
{
    public static void ScaleGameObject(Transform transform, Vector3 targetScale, float duration, MonoBehaviour monoBehaviour, AnimationCurve animationCurve = null)
    {
        monoBehaviour.StartCoroutine(ScaleGameObjectRoutine(transform, targetScale, duration, animationCurve));
    }
    public static IEnumerator ScaleGameObjectRoutine(Transform transform, Vector3 targetScale, float duration, AnimationCurve animationCurve = null)
    {
        int steps = Mathf.RoundToInt(duration / Time.fixedDeltaTime);
        if (steps < 2)
            steps = 2;
        float progress = 0f;
        Vector3 startScale = transform.localScale;
        for (int i = 0; i < steps; i++)
        {
            progress = Remap(i, 0, steps - 1, 0f, 1f);
            transform.localScale = Vector3.Lerp(startScale, targetScale, animationCurve == null ? progress : animationCurve.Evaluate(progress));
            yield return new WaitForFixedUpdate();
        }
    }

    public static float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
