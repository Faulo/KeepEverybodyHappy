using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class FactionInstance {
    public Faction faction;
    public int numberOfDudes { get; set; }
    public int numberOfResidentDudes => ownerWorld.tiles.Where(tile => tile.DudeIsFaction(faction)).Count();
    public int numberOfHomelessDudes => numberOfDudes - numberOfResidentDudes;
    [SerializeField, Range(1, 999)]
    public int startingDudes = 1;
    [SerializeField, Range(-100, 100)]
    public int startingHappiness = 0;

    public World ownerWorld {
        get => ownerWorldCache;
        set {
            if (ownerWorldCache != value) {
                ownerWorldCache = value;
                ownerWorldCache.onTileChange += () => {
                    deltaHappinessDirty = true;
                };
            }
        }
    }

    public void PassTime() {
        globalHappiness += deltaHappiness;
        numberOfDudes += Mathf.RoundToInt(numberOfDudes * globalHappiness / 100);
        numberOfDudes = Mathf.Clamp(numberOfDudes, 0, 999);
        deltaHappinessDirty = true;
    }

    private World ownerWorldCache;

    public float globalHappiness { get; set; }
    public float deltaHappiness {
        get {
            if (deltaHappinessDirty) {
                deltaHappinessDirty = false;
                if (numberOfDudes > 0) {
                    deltaHappinessCache = ownerWorld.tiles
                        .Where(tile => tile.DudeIsFaction(faction))
                        .Sum(tile => tile.happiness);
                    deltaHappinessCache += faction.homelessUnhappiness * numberOfHomelessDudes;
                    deltaHappinessCache /= numberOfDudes;
                } else {
                    deltaHappinessCache = 0;
                }
            }
            return deltaHappinessCache;
        }
    }

    public void Init(World ownerWorld) {
        this.ownerWorld = ownerWorld;
        numberOfDudes = startingDudes;
        globalHappiness = startingHappiness;
    }
    public void Add(FactionInstance other) {
        numberOfDudes += other.startingDudes;
        globalHappiness += other.startingHappiness;
    }

    private bool deltaHappinessDirty = true;
    private float deltaHappinessCache;
}
