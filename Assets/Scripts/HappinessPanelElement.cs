using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HappinessPanelElement : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private Button buildButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFaction(FactionInstance factionInstance) {
        text.text = factionInstance.faction.name;
        buildButton.onClick.AddListener(() => {
            FindObjectOfType<SelectionDrawer>().CurrentFaction = factionInstance.faction;
        });
    }
}
