using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dude : MonoBehaviour
{
    public Faction faction;

    private SpriteRenderer spriteRenderer;

    [SerializeField] private float animateInDuration;
    [SerializeField] private AnimationCurve animateInCurve;
    [SerializeField] private float animateOutDuration;
    [SerializeField] private AnimationCurve animateOutCurve;

    private Coroutine animateInCoroutine;
    private Coroutine animateOutCoroutine;

    private float baseScale;

    private void Awake()
    {
        baseScale = transform.localScale.x;
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Spawn();
    }

    void Update()
    {

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
        animateInCoroutine = StartCoroutine(AnimationUtility.ScaleGameObjectRoutine(transform, Vector3.one * baseScale, animateInDuration, animateInCurve));
    }

    public void Despawn()
    {
        //animateInCoroutine = StartCoroutine(AnimationUtility.ScaleGameObjectRoutine(transform, Vector3.zero, animateOutDuration, animateOutCurve));
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
