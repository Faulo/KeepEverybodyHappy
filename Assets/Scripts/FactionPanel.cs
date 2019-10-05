using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class FactionPanel : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI text = default;

    private World world;
    private Level level;

    public void Observe(World world, Level level) {
        this.world = world;
        this.level = level;
    }

    void Update() {
        if (world != null && level != null) {
            text.text = "";
            var factions = Enumerable.Prepend(
                level.factions,
                new Level.FactionInstance() {
                    faction = Faction.defaultFaction,
                    numberOfMembers = world.tiles.Count()
                }
            );
            foreach (var faction in factions) {
                text.text += string.Format(
                    "{0}: {1}/{2}\n",
                    faction.faction.name,
                    world.tiles.Count(tile => tile.faction == faction.faction),
                    faction.numberOfMembers
                );
            }
        }
    }
}
