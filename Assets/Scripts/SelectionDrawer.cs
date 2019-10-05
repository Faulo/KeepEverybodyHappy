using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SelectionDrawer : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Mesh mesh;
    private BoxCollider boxCollider;

    [SerializeField] private bool draw;
    public bool Draw
    {
        get => draw;
        private set => draw = value;
    }

    private Vector3[] verts = new Vector3[6];
    private int[] triangles = new int[12];
    Vector2[] uvs = new Vector2[6];

    private Vector3 drawStartPoint;
    private Vector3 drawEndPoint;

    private Vector2 selectionSize;
    public Vector2 SelectionSize
    {
        get => selectionSize;
        private set => selectionSize = value;
    }

    private Vector3 selectionCenter;
    public Vector3 SelectionCenter
    {
        get => selectionCenter;
        private set => selectionCenter = value;
    }

    private const float ZPOSITION = 29f;

    private Collider[] hitColliders = new Collider[100];
    [SerializeField] private LayerMask hitMask;
    [SerializeField, Range(0.01f, 1f)] private float changeColorSpeed;
    [SerializeField] private AnimationCurve changeColorHighlightAnimationCurve;
    [SerializeField] private AnimationCurve changeColorRevertAnimationCurve;

    private Camera cam;

    private Dictionary<ITile, (Faction faction, TileHighlight tileHighlight)> markedTilesWithFaction = new Dictionary<ITile, (Faction, TileHighlight)>();
    private List<TileHighlight> unmarkTileList;
    private List<TileHighlight> unfinishedTileList;

    [Header("Factions")]
    [SerializeField] private Faction none;
    [SerializeField] private Faction green;
    [SerializeField] private Faction pink;


    [SerializeField] private Faction currentFaction;
    public Faction CurrentFaction
    {
        get => currentFaction;
        set => currentFaction = value;
    }

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        mesh = meshFilter.mesh;
        cam = Camera.main;
        boxCollider = GetComponent<BoxCollider>();
        unmarkTileList = new List<TileHighlight>();
        unfinishedTileList = new List<TileHighlight>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            drawStartPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            drawStartPoint.z = ZPOSITION;
            meshRenderer.enabled = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            meshRenderer.enabled = false;
            foreach (var item in markedTilesWithFaction)
            {
                item.Value.tileHighlight.unfinished = true;
            }
        }

        Draw = Input.GetMouseButton(0);

        if (Draw)
        {
            drawEndPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            drawEndPoint.z = ZPOSITION;
            DefineSelectionArea();
            DrawSelection();
        }
    }

    private void FixedUpdate()
    {
        AnimateTiles();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(SelectionCenter, SelectionSize);
    }

    private void DefineSelectionArea()
    {
        SelectionSize = new Vector2((drawStartPoint.x - drawEndPoint.x), (drawStartPoint.y - drawEndPoint.y)) * -1;
        SelectionCenter = drawStartPoint + new Vector3(SelectionSize.x * .5f, SelectionSize.y * .5f, 0f);
        boxCollider.center = SelectionCenter;
        boxCollider.size = new Vector3(SelectionSize.x, SelectionSize.y, 2f);
    }

    private void DrawSelection()
    {
        verts[0] = drawStartPoint;
        verts[1] = new Vector3(drawStartPoint.x, drawEndPoint.y, ZPOSITION);
        verts[4] = new Vector3(drawEndPoint.x, drawStartPoint.y, ZPOSITION);

        verts[2] = new Vector3(drawEndPoint.x, drawStartPoint.y, ZPOSITION);
        verts[5] = new Vector3(drawStartPoint.x, drawEndPoint.y, ZPOSITION);

        verts[3] = drawEndPoint;

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 3;

        triangles[3] = 0;
        triangles[4] = 3;
        triangles[5] = 4;

        triangles[6] = 0;
        triangles[7] = 2;
        triangles[8] = 3;

        triangles[9] = 0;
        triangles[10] = 3;
        triangles[11] = 5;

        mesh.vertices = verts;
        mesh.triangles = triangles;
        mesh.uv = uvs;
    }

    private void OnTriggerEnter(Collider other)
    {
        MarkTile(other, CurrentFaction);
    }

    private void OnTriggerExit(Collider other)
    {
        RevertMarking(other);
    }

    private void MarkTile(Collider other, Faction faction)
    {
        if (Draw == false)
            return;
        ITile tile = other.GetComponent<ITile>();
        if (tile != null && markedTilesWithFaction.ContainsKey(tile) == false)
        {
            Material mat = other.GetComponent<MeshRenderer>().material;
            markedTilesWithFaction.Add(tile, (tile.faction, new TileHighlight(mat, 0f, 1)));
            mat.SetColor("_HighlightColor", faction.color);
            tile.faction = faction;
        }
    }
    private void RevertMarking(Collider other)
    {
        if (Draw == false)
            return;
        ITile tile = other.GetComponent<ITile>();
        if (tile != null)
        {
            if (markedTilesWithFaction.ContainsKey(tile))
            {
                if (markedTilesWithFaction[tile].tileHighlight.unfinished)
                    return;
                markedTilesWithFaction[tile].tileHighlight.unfinished = true;
                markedTilesWithFaction[tile].tileHighlight.multiplier = -1;
                tile.faction = markedTilesWithFaction[tile].faction;
            }
        }
    }

    private void AnimateTiles()
    {
        foreach (var item in markedTilesWithFaction.ToArray())
        {
            float oldValue = item.Value.tileHighlight.progress;
            float newValue = Mathf.Clamp01(oldValue + (changeColorSpeed * item.Value.tileHighlight.multiplier));
            item.Value.tileHighlight.progress = newValue;
            float curveValue = changeColorHighlightAnimationCurve.Evaluate(newValue);
            item.Value.tileHighlight.mat.SetFloat("_Highlight", curveValue);
            if (item.Value.tileHighlight.unfinished && (newValue >= 1f || newValue <= 0f))
            {
                markedTilesWithFaction.Remove(item.Key);
                item.Value.tileHighlight.mat.SetColor("_BaseColor", item.Key.faction.color);
            }
        }
    }
}

public class TileHighlight
{
    public Material mat;
    public float progress;
    public float multiplier;
    public bool unfinished;

    public TileHighlight(Material mat, float progress, float multiplier)
    {
        this.mat = mat;
        this.progress = progress;
        this.multiplier = multiplier;
    }
}
