﻿using Slothsoft.UnityExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class World {
    public static World instance { get; private set; }
    private WorldSettings settings;
    private Transform tileRoot;
    private ITile[,] map;
    public Action<World, Level> onLevelLoad;
    public Action onTileChange;

    public World(WorldSettings settings, Transform tileRoot) {
        instance = this;
        this.settings = settings;
        this.tileRoot = tileRoot;
        Setup();
    }

    private void Setup() {
        map = new ITile[settings.width, settings.height];
        tileRoot.position -= new Vector3(settings.width / 2, 0, settings.height / 2);
        for (int x = 0; x < settings.width; x++) {
            for (int y = 0; y < settings.height; y++) {
                var tileObject = UnityEngine.Object.Instantiate(settings.tilePrefab, tileRoot);

                var tile = tileObject.GetComponent<ITile>();
                tile.ownerWorld = this;
                tile.faction = Faction.defaultFaction;
                tile.position = new Vector2Int(x, y);

                map[x, y] = tile;
            }
        }
        tiles = map.Cast<ITile>().Shuffle().ToArray();
    }

    public void LoadLevel(Level level) {
        level.NextSegment();
        foreach (var valuable in level.factionInstances.Where(faction => faction.faction.isValuable)) {
            for (int i = tiles.Where(t => t.faction == valuable.faction).Count(); i < valuable.numberOfDudes; i++) {
                randomEmptyTile.faction = valuable.faction;
            }
        }
        onLevelLoad?.Invoke(this, level);
    }

    public ITile TileAt(int x, int y) {
        return (x < 0 || x >= settings.width || y < 0 || y >= settings.height)
            ? default
            : map[x, y];
    }
    public ITile TileAt(Vector2Int position) => TileAt(position.x, position.y);
    public IEnumerable<ITile> tiles { get; private set; }
    public ITile randomEmptyTile => tiles
        .Where(tile => tile.isZoneable)
        .Where(tile => tile.neighboringTiles.Where(t => t.isZoneable).Count() == 8)
        .RandomElement();
}
