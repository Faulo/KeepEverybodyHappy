using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Faction", menuName = "Gameplay/Faction", order = 1)]
public class Faction : ScriptableObject {
    private static IEnumerable<Faction> allFactions => Resources.LoadAll<Faction>("Factions");
    public static Faction defaultFaction => allFactions.FirstOrDefault(faction => faction.isDefault);

    [Header("Basic Data")]
    public string description = "??";
    public string icon => string.Format("<color=#{0}>■</color>", ColorUtility.ToHtmlStringRGB(tileColor));
    public string legend => string.Format("{0} {1}", icon, description);

    [ColorUsage(true, true)]
    public Color tileColor;
    public bool isDefault = false;
    public bool isValuable = false;
    public bool isDudes => !(isDefault || isValuable);

    [Header("Dude Data")]
    public Color dudeColor;
    [SerializeField, Range(-10, 0)]
    public int homelessUnhappiness = -2;
    public Faction[] likesHavingAccessTo;
    public Faction[] likesBeingNextTo;
    public Faction[] dislikesHavingAccessTo;
    public Faction[] dislikesBeingNextTo;
}
