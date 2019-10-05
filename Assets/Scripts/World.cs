using Slothsoft.UnityExtensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class World {
    private WorldSettings settings;
    private Transform tileRoot;
    private ITile[,] map;

    public World(WorldSettings settings, Transform tileRoot) {
        this.settings = settings;
        this.tileRoot = tileRoot;
        Setup();
        LoadLevel(this.settings.levels[0]);
    }

    private void Setup() {
        map = new ITile[settings.width, settings.height];
        for (int x = 0; x < settings.width; x++) {
            for (int y = 0; y < settings.height; y++) {
                var tileObject = Object.Instantiate(settings.tilePrefab, tileRoot);
                tileObject.transform.localPosition = new Vector3(x - settings.width / 2, 0, y - settings.height / 2);

                var tile = tileObject.GetComponent<ITile>();

                map[x,y] = tile;
            }
        }
    }

    private void LoadLevel(Level level) {
        tiles.ForAll(tile => tile.faction = level.factions.RandomElement().faction);
        Object.FindObjectsOfType<FactionPanel>()
            .ForAll(panel => panel.Observe(this, level));
    }

    public ITile TileAt(int x, int y) => map[x,y];
    public IEnumerable<ITile> tiles => map.Cast<ITile>();
}
