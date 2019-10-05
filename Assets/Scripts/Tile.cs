using Slothsoft.UnityExtensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour, ITile {
    public World ownerWorld { get; set; }
    public Faction faction { get; set; }
    public Vector2Int position { get; set; }

    public float happiness {
        get {
            float happiness = 0;
            if (faction.isDefault) {
                return happiness;
            }
            happiness++;
            foreach (var f in faction.likesBeingNextTo) {
                happiness += neighboringTiles
                    .Count(tile => tile.faction == f);
            }
            foreach (var f in faction.dislikesBeingNextTo) {
                happiness -= neighboringTiles
                    .Count(tile => tile.faction == f);
            }
            foreach (var f in faction.likesHavingAccessTo) {
                happiness += accessibleTiles
                    .Count(tile => tile.faction == f);
            }
            foreach (var f in faction.dislikesHavingAccessTo) {
                happiness -= accessibleTiles
                    .Count(tile => tile.faction == f);
            }
            return happiness;
        }
    }

    public IEnumerable<ITile> neighboringTiles {
        get {
            var offsets = new Vector2Int[] {
                new Vector2Int(-1, 0),
                new Vector2Int(1, 0),
                new Vector2Int(0, -1),
                new Vector2Int(0, 1)
            };
            foreach (var offset in offsets) {
                var tile = ownerWorld.TileAt(position + offset);
                if (tile != default) {
                    yield return tile;
                }
            }
        }
    }

    public IEnumerable<ITile> accessibleTiles {
        get {
            Queue<ITile> uncheckedTiles = new Queue<ITile>(neighboringTiles);
            ISet<ITile> checkedTiles = new HashSet<ITile>();
            while (uncheckedTiles.Count > 0) {
                var tile = uncheckedTiles.Dequeue();
                if (!checkedTiles.Contains(tile)) {
                    checkedTiles.Add(tile);
                    yield return tile;
                    if (faction == tile.faction) {
                        tile.neighboringTiles.ForAll(t => uncheckedTiles.Enqueue(t));
                    }
                }
            }
        }
    }

    private void Update() {
        transform.localPosition = new Vector3(position.x, 0, position.y);
        GetComponent<Renderer>().material.SetColor("_BaseColor", faction.color);
    }
}
