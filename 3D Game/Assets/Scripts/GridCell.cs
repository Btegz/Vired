using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Face
{
    public List<Vector3> vertices;
    public List<int> triangles;
    public List<Vector2> uvs;

    public Face(List<Vector3> vertices, List<int> triangles, List<Vector2> normals)
    {
        this.vertices = vertices;
        this.triangles = triangles;
        this.uvs = normals;
    }
}

public class GridCell : MonoBehaviour
{
    public Vector3 CellCoordinate;

    [SerializeField] public float innerSize = 0;
    [SerializeField] public float outerSize = 1;
    [SerializeField] public float height = 0;

    Mesh mesh;
    public MeshFilter meshFilter;
    //MeshRenderer meshRenderer;

    List<Face> faces;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh();
        mesh.name = "Hex";
        meshFilter.mesh = mesh;
        DrawMesh();
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            DrawMesh();
        }
    }

    public void DrawMesh()
    {
        DrawFaces();
        CombineFaces();
    }

    public void DrawFaces()
    {
        faces = new List<Face>();
        for (int i = 0; i < 6; i++)
        {
            faces.Add(CreateFace(innerSize, outerSize, height / 2f, height / 2f, i));
        }
    }

    public void CombineFaces()
    {
        //mesh = new Mesh();
        //mesh.name = "Hex";
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        for (int i = 0; i < faces.Count; i++)
        {
            vertices.AddRange(faces[i].vertices);
            uvs.AddRange(faces[i].uvs);

            int offset = 4 * i;
            foreach (int tris in faces[i].triangles)
            {
                triangles.Add(tris + offset);
            }
        }
        
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
    }

    private Face CreateFace(float innerRad, float outerRad, float heightA, float heightB, int point, bool reverse = false)
    {
        Vector3 p1 = GetPoint(innerRad, heightB, point);
        Vector3 p2 = GetPoint(innerRad, heightB, (point < 5) ? point + 1 : 0);
        Vector3 p3 = GetPoint(outerRad, heightA, (point < 5) ? point + 1 : 0);
        Vector3 p4 = GetPoint(outerRad, heightA, point);

        List<Vector3> vertices = new List<Vector3>() { p1, p2, p3, p4 };
        List<int> triangles = new List<int>() { 0, 1, 2, 2, 3, 0 };
        List<Vector2> uvs = new List<Vector2>() { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) };
        if(reverse)
        {
            vertices.Reverse();
        }

        return new Face(vertices,triangles,uvs);
    }

    private Vector3 GetPoint(float size, float height, int index)
    {
        float angle_deg = 60 * index;
        float angle_rad = Mathf.PI / 180f * angle_deg;
        return new Vector3((size * Mathf.Cos(angle_rad)), height, size * Mathf.Sin(angle_rad));

    }
}
