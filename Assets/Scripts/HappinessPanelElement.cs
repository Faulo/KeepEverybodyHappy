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
    private TextMeshProUGUI valuableText = default;
    [SerializeField]
    private TextMeshProUGUI homelessText = default;
    [SerializeField]
    private Slider slider = default;
    [SerializeField]
    private Image fillImage = default;
    [SerializeField]
    private TextMeshProUGUI deltaText = default;
    [SerializeField]
    private Color increaseHappinessColor = default;
    private string increaseHappinessString => ColorUtility.ToHtmlStringRGB(increaseHappinessColor);
    [SerializeField]
    private Color decreaseHappinessColor = default;
    private string decreaseHappinessString => ColorUtility.ToHtmlStringRGB(decreaseHappinessColor);

    private FactionInstance factionInstance;

    void Start()
    {
        fillImage.color = factionInstance.faction.tileColor;
    }

    void Update()
    {
        homelessText.text = string.Format(
            "Placed: <color=#{0}>{1}/{2}</color>",
            factionInstance.numberOfHomelessDudes == 0 ? increaseHappinessString : decreaseHappinessString,
            factionInstance.numberOfResidentDudes,
            factionInstance.numberOfDudes
        );
        slider.value = factionInstance.deltaHappiness;
        deltaText.text = string.Format(
            "Happiness: <color=#{0}>{1}{2}</color>",
            factionInstance.deltaHappiness > 0 ? increaseHappinessString : decreaseHappinessString,
            factionInstance.deltaHappiness > 0 ? "+" : "-",
            Mathf.Abs(factionInstance.deltaHappiness).ToString("F1")
        );
    }

    public void Init(FactionInstance factionInstance, World world, Level level)
    {
        this.factionInstance = factionInstance;

        buildButtonText.text = factionInstance.faction.legend;
        buildButton.onClick.AddListener(() =>
        {
            FindObjectOfType<SelectionDrawer>().CurrentFaction = factionInstance.faction;

        });
        factionInstance.faction.likesBeingNextTo
            .Where(level.factions.Contains)
            .ForAll(AddLike);
        factionInstance.faction.dislikesBeingNextTo
            .Union(factionInstance.faction.dislikesHavingAccessTo)
            .Where(level.factions.Contains)
            .ForAll(AddDislike);
        factionInstance.faction.likesHavingAccessTo
            .Union(factionInstance.faction.likesHavingAccessTo)
            .Where(level.factions.Contains)
            .ForAll(AddValuable);
    }
    private void AddLike(Faction faction)
    {
        likesText.text += faction.icon;
    }
    private void AddDislike(Faction faction) {
        dislikesText.text += faction.icon;
    }
    private void AddValuable(Faction faction) {
        valuableText.text += faction.icon;
    }
}
