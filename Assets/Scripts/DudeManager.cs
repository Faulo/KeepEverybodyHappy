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

    public void SpawnDudes(IEnumerable<ITile> tiles) {
        foreach (var tile in tiles) {
            Dude dude = Instantiate(dudePrefab, tile.transform);
            dude.faction = tile.faction;
            tile.dude = dude;
        }
    }

    public void DespawnDudes(IEnumerable<ITile> tiles) {
        foreach (var tile in tiles) {
            //???
            Destroy(tile.dude.gameObject);
            tile.dude = null;
        }
    }

    public void UpdateDudePositions()
    {
        foreach (var dude in activeDudes)
        {

        }
    }
}
