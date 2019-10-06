using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HappinessManager : MonoBehaviour, IWorldObserver {
    [SerializeField]
    private DudeManager dudeManager = default;
    private World world;
    private Level level;
    private FactionInstance[] dudeFactions;
    public void Observe(World world, Level level) {
        this.world = world;
        this.level = level;
        dudeFactions = level.factionInstances
            .Where(factionInstance => factionInstance.faction.isDudes)
            .ToArray();
    }
    void Update() {
        if (world != null && level != null) {
            foreach (var factionInstance in dudeFactions) {
                var faction = factionInstance.faction;
                var factionTiles = world.tiles
                    .Where(tile => tile.faction == faction)
                    .OrderBy(tile => tile.happiness)
                    .Reverse()
                    .Take(factionInstance.numberOfDudes);
                var otherTiles = world.tiles
                    .Except(factionTiles);

                dudeManager?.SpawnDudes(factionTiles.Where(tile => tile.dude == null));
                dudeManager?.DespawnDudes(otherTiles.Where(tile => tile.DudeIsFaction(faction)));
            }
        }
    }
}
