using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World {
    private WorldSettings settings;
    private Transform tileRoot;
    private GameObject[][] map;
    public World(WorldSettings settings, Transform tileRoot) {
        this.settings = settings;
        this.tileRoot = tileRoot;
        Setup();
    }
    private void Setup() {
        map = new GameObject[settings.width][];
        for (int x = 0; x < settings.width; x++) {
            map[x] = new GameObject[settings.height];
            for (int y = 0; y < settings.height; y++) {
                map[x][y] = Object.Instantiate(settings.tilePrefab, tileRoot);
                map[x][y].transform.localPosition = new Vector3(x - settings.width / 2, 0, y - settings.height / 2);
                var mats = map[x][y].GetComponent<Renderer>().materials;
                mats[0].color = Color.white;
                map[x][y].GetComponent<Renderer>().materials = mats;
            }
        }
    }
}
