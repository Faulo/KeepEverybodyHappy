using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpawner : MonoBehaviour
{
    [SerializeField]
    private WorldSettings worldSettings = default;
    // Start is called before the first frame update
    void Start()
    {
        new World(worldSettings, transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
