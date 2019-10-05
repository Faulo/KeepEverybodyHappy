using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New World Settings", menuName = "Gameplay/WorldSettings", order = 2)]
public class WorldSettings : ScriptableObject {
    public int width;
    public int height;
    public GameObject tilePrefab;
    public Level[] levels;
}
