using Slothsoft.UnityExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Gameplay/Level", order = 3)]
public class Level : ScriptableObject {
    public LevelSegment[] segments;

    private Dictionary<Faction, FactionInstance> factionInstancesDicionary = new Dictionary<Faction, FactionInstance>();
    public IEnumerable<FactionInstance> factionInstances => factionInstancesDicionary.Values;

    private int segmentIndex { get; set; } = 0;
    public void NextSegment() {
        if (segmentIndex < segments.Length) {
            MergeWith(segments[segmentIndex].factionInstances);
            segmentIndex++;
        }
    }
    private void MergeWith(FactionInstance[] newInstances) {
        foreach (var newInstance in newInstances) {
            if (factionInstancesDicionary.ContainsKey(newInstance.faction)) {
                factionInstancesDicionary[newInstance.faction].numberOfDudes += newInstance.numberOfDudes;
            } else {
                factionInstancesDicionary[newInstance.faction] = newInstance;
            }
        }
    }
}
