using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Gameplay/Level", order = 3)]
public class Level : ScriptableObject {
    [Serializable]
    public class FactionInstance {
        public Faction faction;
        public int numberOfMembers;
    }
    public FactionInstance[] factions;
}
