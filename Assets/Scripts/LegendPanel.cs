using Slothsoft.UnityExtensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LegendPanel : MonoBehaviour, IWorldObserver {
    [SerializeField]
    private LayoutGroup elementGroup = default;
    [SerializeField]
    private TextMeshProUGUI elementPrefab = default;

    public void Observe(World world, Level level) {
        foreach (Transform child in elementGroup.transform) {
            Destroy(child.gameObject);
        }
        level.factionInstances
            .Select(factionInstance => factionInstance.faction)
            .OrderBy(faction => faction.name)
            .ForAll(faction => Instantiate(elementPrefab, elementGroup.transform).text = faction.legend);
    }
}
