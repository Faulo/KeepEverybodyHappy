using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World {
    private WorldSettings settings;
    private Transform tileRoot;
    private ITile[][] map;
    public World(WorldSettings settings, Transform tileRoot) {
        this.settings = settings;
        this.tileRoot = tileRoot;
        Setup();
    }
    private void Setup() {
        map = new ITile[settings.width][];
        for (int x = 0; x < settings.width; x++) {
            map[x] = new ITile[settings.height];
            for (int y = 0; y < settings.height; y++) {
                var tileObject = Object.Instantiate(settings.tilePrefab, tileRoot);
                tileObject.transform.localPosition = new Vector3(x - settings.width / 2, 0, y - settings.height / 2);

                var tile = tileObject.GetComponent<ITile>();
                tile.faction = settings.defaultFaction;

                map[x][y] = tile;
            }
        }
    }
    public ITile TileAt(int x, int y) => map[x][y];
}
