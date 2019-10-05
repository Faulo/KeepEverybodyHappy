using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HappinessPanelElement : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFaction(Level.FactionInstance faction) {
        text.text = faction.faction.name;
    }
}
