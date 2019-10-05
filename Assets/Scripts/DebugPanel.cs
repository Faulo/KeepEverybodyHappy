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
    private IEnumerable<Level.FactionInstance> factions;

    public void Observe(World world, Level level) {
        this.world = world;
        this.level = level;

        factions = Enumerable.Prepend(
            level.factions,
            new Level.FactionInstance() {
                faction = Faction.defaultFaction,
                numberOfMembers = world.tiles.Count()
            }
        );
    }

    void Update() {
        if (world != null && level != null) {
            text.text = "";
            foreach (var faction in factions) {
                var tiles = world.tiles.Where(tile => tile.faction == faction.faction);
                text.text += string.Format(
                    "{0} ({1}/{2}): {3}\n",
                    faction.faction.name,
                    tiles.Count(),
                    faction.numberOfMembers,
                    tiles.Sum(tile => tile.happiness)
                );
            }
        }
    }
}
