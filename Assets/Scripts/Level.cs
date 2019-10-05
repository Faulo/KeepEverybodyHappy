using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Gameplay/Level", order = 3)]
public class Level : ScriptableObject {
    public FactionInstance[] factions;
}
