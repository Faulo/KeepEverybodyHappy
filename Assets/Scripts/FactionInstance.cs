using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class FactionInstance {
    public Faction faction;
    public int numberOfDudes { get; set; }
    public int numberOfResidentDudes => ownerWorld.tiles.Where(tile => tile.DudeIsFaction(faction)).Count();
    public int numberOfHomelessDudes => numberOfDudes - numberOfResidentDudes;
    [SerializeField, Range(1, 1000)]
    public int startingDudes = 1;
    [SerializeField, Range(0, 10)]
    public int startingHappiness = 5;
    [SerializeField, Range(-10, 0)]
    public int homelessUnhappiness = -2;

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
    private World ownerWorldCache;

    public float globalHappiness { get; set; }
    public float deltaHappiness {
        get {
            if (deltaHappinessDirty) {
                deltaHappinessDirty = false;
                deltaHappinessCache = ownerWorld.tiles
                    .Where(tile => tile.DudeIsFaction(faction))
                    .Sum(tile => tile.happiness);
                deltaHappinessCache += homelessUnhappiness * numberOfHomelessDudes;
                deltaHappinessCache /= numberOfDudes;
            }
            return deltaHappinessCache;
        }
    }

    public void Init(World ownerWorld) {
        this.ownerWorld = ownerWorld;
        numberOfDudes = startingDudes;
        globalHappiness = startingHappiness;
    }

    private bool deltaHappinessDirty = true;
    private float deltaHappinessCache;
}
