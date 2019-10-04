using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionDrawer : MonoBehaviour
{

    private MeshFilter meshFilter;
    private Mesh mesh;

    [SerializeField] private bool draw;
    public bool Draw
    {
        get => draw;
        private set => draw = value;
    }

    private Vector3 drawStartPoint;
    private Vector3 drawEndPoint;

    private const float ZPOSITION = -1f;

    private Camera cam;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;
        cam = Camera.main;
    }

    private void Start()
    {

    }

    void Update()
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
            DrawSelection();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(drawStartPoint, drawEndPoint);
    }

    private void DrawSelection()
    {
        Vector3[] verts = new Vector3[6];

        verts[0] = drawStartPoint;
        verts[1] = new Vector3(drawStartPoint.x, drawEndPoint.y, ZPOSITION);
        verts[4] = new Vector3(drawEndPoint.x, drawStartPoint.y, ZPOSITION);

        verts[2] = new Vector3(drawEndPoint.x, drawStartPoint.y, ZPOSITION);
        verts[5] = new Vector3(drawStartPoint.x, drawEndPoint.y, ZPOSITION);

        verts[3] = drawEndPoint;

        int[] triangles = new int[12];
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

        Vector2[] uvs = new Vector2[6];
        mesh.vertices = verts;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateNormals();
    }
}
