﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Faction", menuName = "Gameplay/Faction", order = 1)]
public class Faction : ScriptableObject {
    private static IEnumerable<Faction> allFactions => Resources.LoadAll<Faction>("Factions");
    public static IEnumerable<Faction> activeFactions => allFactions.Where(faction => faction.isActive);
    public static Faction defaultFaction => activeFactions.FirstOrDefault(faction => faction.isDefault);

    public bool isActive = true;
    public Color color;
    public bool isDefault = false;
}