using Slothsoft.UnityExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HappinessPanelElement : MonoBehaviour {
    [SerializeField]
    private Button buildButton;
    [SerializeField]
    private TextMeshProUGUI buildButtonText;
    [SerializeField]
    private TextMeshProUGUI likesText;
    [SerializeField]
    private TextMeshProUGUI dislikesText;
    [SerializeField]
    private TextMeshProUGUI happinessText;

    private FactionInstance factionInstance;
    private World world;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        happinessText.text = string.Format("Happiness:\n{0}", factionInstance.CalculateHappiness(world));
    }

    public void SetFaction(FactionInstance factionInstance, World world) {
        this.factionInstance = factionInstance;
        this.world = world;

        buildButtonText.text = factionInstance.faction.name;
        buildButton.onClick.AddListener(() => {
            FindObjectOfType<SelectionDrawer>().CurrentFaction = factionInstance.faction;
        });
        factionInstance.faction.likesBeingNextTo.ForAll(AddLike);
        factionInstance.faction.likesHavingAccessTo.ForAll(AddLike);
        factionInstance.faction.dislikesBeingNextTo.ForAll(AddDislike);
        factionInstance.faction.dislikesHavingAccessTo.ForAll(AddDislike);
    }
    private void AddLike(Faction faction) {
        likesText.text += string.Format(
            "<color=#{0}>■</color>",
            ColorUtility.ToHtmlStringRGB(faction.tileColor)
        );
    }
    private void AddDislike(Faction faction) {
        dislikesText.text += string.Format(
            "<color=#{0}>■</color>",
            ColorUtility.ToHtmlStringRGB(faction.tileColor)
        );
    }
}
