using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITile {
    World ownerWorld { get; set; }
    bool ownerWorldHasChanged { set; }
    Faction faction { get; set; }
    Dude dude { get; set; }
    Vector2Int position { get; set; }
    float happiness { get; }
    IEnumerable<ITile> neighboringTiles { get; set; }
    IEnumerable<ITile> accessibleTiles { get; set; }
}
