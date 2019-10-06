using Slothsoft.UnityExtensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldSpawner : MonoBehaviour {
    [SerializeField]
    private WorldSettings worldSettings = default;

    public World world { get; private set; }
    public Level level { get; private set; }

    void Start() {
        world = new World(worldSettings, transform);
        level = worldSettings.levels[0];
        FindObjectsOfType<Transform>()
            .SelectMany(t => t.GetComponents<IWorldObserver>())
            .ForAll(observer => world.onLevelLoad += observer.Observe);
        NextLevelSegment();
    }

    public void NextLevelSegment() {
        world.LoadLevel(level);
    }
}
