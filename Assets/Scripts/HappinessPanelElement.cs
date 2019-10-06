using Slothsoft.UnityExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HappinessPanelElement : MonoBehaviour
{
    [SerializeField]
    private Button buildButton = default;
    [SerializeField]
    private TextMeshProUGUI buildButtonText = default;
    [SerializeField]
    private TextMeshProUGUI likesText = default;
    [SerializeField]
    private TextMeshProUGUI dislikesText = default;
    [SerializeField]
    private TextMeshProUGUI placedText = default;
    [SerializeField]
    private Slider slider = default;
    [SerializeField]
    private Image fillImage = default;
    [SerializeField]
    private TextMeshProUGUI deltaText = default;
    [SerializeField]
    private Color increaseHappinessColor = default;
    [SerializeField]
    private Color decreaseHappinessColor = default;

    private FactionInstance factionInstance;

    private Vector2 cursorHitspot = new Vector2(400, 0);

    void Start()
    {
        fillImage.color = factionInstance.faction.tileColor;
    }

    void Update()
    {
        slider.value = factionInstance.globalHappiness;
        placedText.text = string.Format(
            "Placed: {0}/{1}",
            factionInstance.numberOfResidentDudes,
            factionInstance.numberOfDudes
        );
        deltaText.text = string.Format(
            "Happiness: <color=#{0}>{1}{2}</color>",
            ColorUtility.ToHtmlStringRGB(factionInstance.deltaHappiness > 0 ? increaseHappinessColor : decreaseHappinessColor),
            factionInstance.deltaHappiness > 0 ? "+" : "",
            factionInstance.deltaHappiness.ToString("F1")
        );
    }

    public void Init(FactionInstance factionInstance, World world, Level level)
    {
        this.factionInstance = factionInstance;

        buildButtonText.text = factionInstance.faction.legend;
        buildButton.onClick.AddListener(() =>
        {
            FindObjectOfType<SelectionDrawer>().CurrentFaction = factionInstance.faction;
            Cursor.SetCursor(factionInstance.faction.cursor_texture, cursorHitspot, CursorMode.Auto);

        });
        factionInstance.faction.likesBeingNextTo
            .Union(factionInstance.faction.likesHavingAccessTo)
            .Where(level.factions.Contains)
            .ForAll(AddLike);
        factionInstance.faction.dislikesBeingNextTo
            .Union(factionInstance.faction.dislikesHavingAccessTo)
            .Where(level.factions.Contains)
            .ForAll(AddDislike);
    }
    private void AddLike(Faction faction)
    {
        likesText.text += faction.icon;
    }
    private void AddDislike(Faction faction)
    {
        dislikesText.text += faction.icon;
    }
}
