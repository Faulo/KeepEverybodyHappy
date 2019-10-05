using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionDrawer : MonoBehaviour
{
    private MeshFilter meshFilter;
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

    private Camera cam;


    [Header("Factions")]
    [SerializeField] private Faction none;
    [SerializeField] private Faction green;
    [SerializeField] private Faction pink;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;
        cam = Camera.main;
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            drawStartPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            drawStartPoint.z = ZPOSITION;
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
        ITile tile = other.GetComponent<ITile>();
        if (tile != null)
        {
            tile.faction = green;
            Debug.Log("OnTriggerEnter green");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ITile tile = other.GetComponent<ITile>();
        if (tile != null)
        {
            tile.faction = none;
        }

    }
}
