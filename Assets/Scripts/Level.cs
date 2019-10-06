using Slothsoft.UnityExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Gameplay/Level", order = 3)]
public class Level : ScriptableObject {
    public LevelSegment[] segments;

    private Dictionary<Faction, FactionInstance> factionInstancesDicionary = new Dictionary<Faction, FactionInstance>();
    public IEnumerable<FactionInstance> factionInstances => factionInstancesDicionary.Values;
    public IEnumerable<Faction> factions => factionInstancesDicionary.Keys;

    private int segmentIndex { get; set; } = 0;
    private void OnEnable() {
        segmentIndex = 0;
    }
    public void NextSegment() {
        if (segmentIndex > 0) {
            factionInstances.ForAll(factionInstance => factionInstance.PassTime());
        }
        if (segmentIndex < segments.Length) {
            MergeWith(segments[segmentIndex].factionInstances);
            segmentIndex++;
        }
    }
    private void MergeWith(FactionInstance[] newInstances) {
        foreach (var newInstance in newInstances) {
            if (factionInstancesDicionary.ContainsKey(newInstance.faction)) {
                factionInstancesDicionary[newInstance.faction].Add(newInstance);
            } else {
                factionInstancesDicionary[newInstance.faction] = newInstance;
                factionInstancesDicionary[newInstance.faction].Init(World.instance);
            }
        }
    }
}
