using Slothsoft.UnityExtensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldSpawner : MonoBehaviour {
    [SerializeField]
    private WorldSettings worldSettings = default;

    public World world { get; private set; }
    // Start is called before the first frame update
    void Start() {
        var world = new World(worldSettings, transform);
        FindObjectOfType<Canvas>()
            .GetComponentsInChildren<IWorldObserver>()
            .ForAll(observer => world.onLevelLoad += observer.Observe);
        world.LoadLevel(worldSettings.levels[0]);
    }

    // Update is called once per frame
    void Update() {

    }
}
