using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DebugPanel : MonoBehaviour, IWorldObserver {
    [SerializeField]
    private TextMeshProUGUI text = default;

    private World world;
    private Level level;
    private IEnumerable<FactionInstance> factions;

    public void Observe(World world, Level level) {
        this.world = world;
        this.level = level;

        factions = Enumerable.Prepend(
            level.factionInstances,
            new FactionInstance() {
                faction = Faction.defaultFaction,
                numberOfDudes = world.tiles.Count()
            }
        );
    }

    void Update() {
        if (world != null && level != null) {
            text.text = "";
            foreach (var factionInstance in factions) {
                var tiles = world.tiles.Where(tile => tile.DudeIsFaction(factionInstance.faction));
                text.text += string.Format(
                    "{0} ({1}/{2}): {3}\n",
                    factionInstance.faction.description,
                    tiles.Count(),
                    factionInstance.numberOfDudes,
                    tiles.Sum(tile => tile.happiness)
                );
            }
        }
    }
}
