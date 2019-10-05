using System;
using System.Linq;

[Serializable]
public class FactionInstance {
    public Faction faction;
    public int numberOfDudes;
    public int CalculateHappiness(World world) {
        return world.tiles
            .Where(tile => tile.DudeIsFaction(faction))
            .Sum(tile => tile.happiness);
    }
}
