﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITile {
    Transform transform { get; }
    World ownerWorld { get; set; }
    bool ownerWorldHasChanged { set; }
    Faction faction { get; set; }
    Dude dude { get; set; }
    bool DudeIsFaction(Faction faction);
    Vector2Int position { get; set; }
    float happiness { get; }
    IEnumerable<ITile> neighboringTiles { get; set; }
    IEnumerable<ITile> accessibleTiles { get; set; }

}
