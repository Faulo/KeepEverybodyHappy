using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DudeManager : MonoBehaviour
{

    [SerializeField] private Dude dudePrefab;

    private List<Dude> activeDudes = new List<Dude>();

    public static DudeManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void SpawnDudes(params Tile[] tiles)
    {
        foreach (var tile in tiles)
        {
            Dude dude = Instantiate(dudePrefab, tile.transform);
            dude.faction = tile.faction;
            activeDudes.Add(dude);
        }
    }

    public void UpdateDudePositions()
    {
        foreach (var dude in activeDudes)
        {

        }
    }
}
