using Slothsoft.UnityExtensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour, ITile {
    public World ownerWorld { get; set; }
    public bool ownerWorldHasChanged {
        set {
            if (value) {
                happinessDirty = true;
                neighboringTilesDirty = true;
                accessibleTilesDirty = true;
            }
        }
    }
    public Faction faction {
        get => factionCache;
        set {
            if (factionCache != value) {
                factionCache = value;
                ownerWorld.tiles
                    .Where(tile => tile != default)
                    .ForAll(tile => tile.ownerWorldHasChanged = true);
            }
        }
    }
    private Faction factionCache;
    public Dude dude { get; set; }
    public Vector2Int position {
        get => positionCache;
        set {
            positionCache = value;
            transform.localPosition = new Vector3(position.x, 0, position.y);
        }
    }
    private Vector2Int positionCache;

    public float happiness {
        get {
            if (happinessDirty) {
                happinessDirty = false;
                happinessCache = 0;
                if (!faction.isDefault) {
                    happinessCache++;
                    foreach (var f in faction.likesBeingNextTo) {
                        if (neighboringTiles.Any(tile => tile.faction == f)) {
                            happinessCache++;
                        }
                    }
                    foreach (var f in faction.dislikesBeingNextTo) {
                        if (neighboringTiles.Any(tile => tile.faction == f)) {
                            happinessCache--;
                        }
                    }
                    foreach (var f in faction.likesHavingAccessTo) {
                        if (accessibleTiles.Any(tile => tile.faction == f)) {
                            happinessCache++;
                        }
                    }
                    foreach (var f in faction.dislikesHavingAccessTo) {
                        if (accessibleTiles.Any(tile => tile.faction == f)) {
                            happinessCache--;
                        }
                    }
                }
            }
            return happinessCache;
        }
    }
    private bool happinessDirty = true;
    private float happinessCache;

    public IEnumerable<ITile> neighboringTiles {
        set {
            neighboringTilesDirty = false;
            neighboringTilesCache = value;
        }
        get {
            if (neighboringTilesDirty) {
                var offsets = new Vector2Int[] {
                    new Vector2Int(-1, 0),
                    new Vector2Int(1, 0),
                    new Vector2Int(0, -1),
                    new Vector2Int(0, 1)
                };
                neighboringTiles = offsets
                    .Select(offset => ownerWorld.TileAt(position + offset))
                    .Where(tile => tile != default)
                    .ToArray();
            }
            return neighboringTilesCache;
        }
    }
    private bool neighboringTilesDirty = true;
    public IEnumerable<ITile> neighboringTilesCache;

    public IEnumerable<ITile> accessibleTiles {
        set {
            accessibleTilesDirty = false;
            accessibleTilesCache = value;
        }
        get {
            if (accessibleTilesDirty) {
                Queue<ITile> uncheckedTiles = new Queue<ITile>(neighboringTiles);
                ISet<ITile> checkedTiles = new HashSet<ITile>();

                while (uncheckedTiles.Count > 0) {
                    var tile = uncheckedTiles.Dequeue();
                    if (checkedTiles.Add(tile)) {
                        if (faction == tile.faction) {
                            tile.accessibleTiles = checkedTiles;
                            tile.neighboringTiles.ForAll(t => uncheckedTiles.Enqueue(t));
                        }
                    }
                }
            }
            return accessibleTilesCache;
        }
    }
    private bool accessibleTilesDirty = true;
    public IEnumerable<ITile> accessibleTilesCache;

    private void Start() {
        GetComponent<Renderer>().material.SetColor("_BaseColor", faction.tileColor);
        transform.localPosition = new Vector3(position.x, 0, position.y);
    }
}
