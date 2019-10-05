using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITile {
    World ownerWorld { get; set; }
    Faction faction { get; set; }
    Vector2Int position { get; set; }
    float happiness { get; }
    IEnumerable<ITile> neighboringTiles { get; }
    IEnumerable<ITile> accessibleTiles { get; }
}
