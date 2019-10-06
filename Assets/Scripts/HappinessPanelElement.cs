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
    private Button buildButton;
    [SerializeField]
    private TextMeshProUGUI buildButtonText;
    [SerializeField]
    private TextMeshProUGUI likesText;
    [SerializeField]
    private TextMeshProUGUI dislikesText;
    [SerializeField]
    private TextMeshProUGUI placedText;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Image fillImage;

    private FactionInstance factionInstance;
    private World world;
    // Start is called before the first frame update
    void Start()
    {
        fillImage.color = factionInstance.faction.tileColor;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = factionInstance.globalHappiness;
        placedText.text = "Placed: " + factionInstance.numberOfResidentDudes + "/" + factionInstance.numberOfDudes.ToString();
    }

    public void Init(FactionInstance factionInstance, World world, Level level)
    {
        this.factionInstance = factionInstance;
        this.world = world;

        buildButtonText.text = factionInstance.faction.legend;
        buildButton.onClick.AddListener(() =>
        {
            FindObjectOfType<SelectionDrawer>().CurrentFaction = factionInstance.faction;
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
