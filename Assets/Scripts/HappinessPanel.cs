using Slothsoft.UnityExtensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HappinessPanel : MonoBehaviour, IWorldObserver {
    [SerializeField]
    private HappinessPanelElement elementPrefab = default;
    private World world;
    private Level level;
    public void Observe(World world, Level level) {
        this.world = world;
        this.level = level;
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
        level.factionInstances
            .Where(faction => faction.faction.isDudes)
            .ForAll(faction => {
                Instantiate(elementPrefab, transform).Init(faction, world, level);
            });
    }
    void Update() {

    }
}
