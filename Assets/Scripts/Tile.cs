using Slothsoft.UnityExtensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour, ITile
{
    public World ownerWorld { get; set; }
    public Faction faction
    {
        get => factionCache;
        set
        {
            if (factionCache != value)
            {
                factionCache = value;
                ownerWorld.onTileChange?.Invoke();
                GetComponent<Renderer>().material.SetColor("_BaseColor", faction.tileColor);
                if (faction.isValuable) {
                    GetComponent<Renderer>().material.SetColor("_Emission", faction.tileColor);
                }
            }
        }
    }
    public bool isZoneable => !faction.isValuable;
    private Faction factionCache;
    public Dude dude
    {
        get => dudeCache;
        set
        {
            if (dudeCache != value)
            {
                if (dudeCache != null)
                {
                    dudeCache.happiness = 0;
                }
                dudeCache = value;
                if (dudeCache != null)
                {
                    dudeCache.happiness = happiness;
                }
            }
        }
    }
    public Dude dudeCache;
    public bool DudeIsFaction(Faction faction) => dude && dude.faction == faction;
    public Vector2Int position
    {
        get => positionCache;
        set
        {
            positionCache = value;
            transform.localPosition = new Vector3(position.x, 0, position.y);
        }
    }
    private Vector2Int positionCache;

    public float happiness
    {
        get
        {
            if (happinessDirty)
            {
                happinessDirty = false;
                happinessCache = 0;
                if (!faction.isDefault)
                {
                    happinessCache++;
                    foreach (var f in faction.likesBeingNextTo)
                    {
                        happinessCache += neighboringTiles.Count(tile => tile.faction == f);
                    }
                    foreach (var f in faction.dislikesBeingNextTo)
                    {
                        happinessCache -= neighboringTiles.Count(tile => tile.faction == f);
                    }
                    foreach (var f in faction.likesHavingAccessTo)
                    {
                        happinessCache += accessibleTiles.Count(tile => tile.faction == f);
                    }
                    foreach (var f in faction.dislikesHavingAccessTo)
                    {
                        happinessCache -= accessibleTiles.Count(tile => tile.faction == f);
                    }
                }
                if (dude != null)
                {
                    dude.happiness = happinessCache;
                }
            }
            return happinessCache;
        }
    }
    private bool happinessDirty = true;
    private float happinessCache;

    public IEnumerable<ITile> neighboringTiles
    {
        set
        {
            neighboringTilesDirty = false;
            neighboringTilesCache = value;
        }
        get
        {
            if (neighboringTilesDirty)
            {
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

    public IEnumerable<ITile> accessibleTiles
    {
        set
        {
            accessibleTilesDirty = false;
            accessibleTilesCache = value;
        }
        get
        {
            if (accessibleTilesDirty)
            {
                Queue<ITile> uncheckedTiles = new Queue<ITile>(neighboringTiles);
                ISet<ITile> checkedTiles = new HashSet<ITile>();

                while (uncheckedTiles.Count > 0)
                {
                    var tile = uncheckedTiles.Dequeue();
                    if (checkedTiles.Add(tile))
                    {
                        if (faction == tile.faction)
                        {
                            tile.accessibleTiles = checkedTiles;
                            tile.neighboringTiles.ForAll(t => uncheckedTiles.Enqueue(t));
                        }
                    }
                }
                accessibleTiles = checkedTiles;
            }
            return accessibleTilesCache;
        }
    }
    private bool accessibleTilesDirty = true;
    public IEnumerable<ITile> accessibleTilesCache;

    private void Start()
    {
        ownerWorld.onTileChange += () =>
        {
            happinessDirty = true;
            neighboringTilesDirty = true;
            accessibleTilesDirty = true;
        };
        transform.localPosition = new Vector3(position.x, 0, position.y);

    }
}
