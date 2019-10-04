using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour, ITile {
    public Faction faction { get; set; }

    private void Update() {
        GetComponent<Renderer>().material.SetColor("_BaseColor", faction.color);
    }
}
