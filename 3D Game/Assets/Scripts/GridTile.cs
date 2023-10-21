using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This struct holds, vertices, triangles and uvs of a Face
/// </summary>
public struct Face
{
    public List<Vector3> vertices;
    public List<int> triangles;
    public List<Vector2> uvs;


    /// <summary>
    /// This constructor creates a face with given vertices, triangles and uvs
    /// </summary>
    /// <param name="vertices">Locations where the edges meet, making a corner</param>
    /// <param name="triangles">Indexes for vertices to make an edges.</param>
    /// <param name="uvs">markerpoints that control which pixels on a texture correspond to which verex.</param>
    public Face(List<Vector3> vertices, List<int> triangles, List<Vector2> uvs)
    {
        this.vertices = vertices;
        this.triangles = triangles;
        this.uvs = uvs;
    }
}

/// <summary>
/// MonoBehaviour Class to hold Information and provide functions of a hexagonal Cell.
/// It also generates it's mesh.
/// </summary>
public class GridTile : MonoBehaviour
{
    public Vector2Int AxialCoordinate;

    [HideInInspector] public float innerSize = 0;
    [HideInInspector] public float outerSize = 1;
    [HideInInspector] public float height = 0;
    Mesh mesh;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    List<Face> faces;

    [Header("Tile Statestuff")]
    public GridState currentGridState;
    public string gridStateString;
    public Ressource ressource;


    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
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

    /// <summary>
    /// Sets up the Tile. Should allway be called when instantiating the Tile.
    /// </summary>
    /// <param name="cellCoordinate">Axial Coordinate in the the Grid.</param>
    /// <param name="resource">Type of Ressource for the Tile.</param>
    /// <param name="gridstate">The Gridstate to start with.</param>
    public void Setup(Vector2Int cellCoordinate, Ressource resource, GridState gridstate)
    {
        this.AxialCoordinate = cellCoordinate;
        this.ressource = resource;
        this.currentGridState = gridstate;
        gridStateString = this.currentGridState.ToString();
        switch (gridstate)
        {
            case GS_positive:
                switch (ressource)
                {
                    case Ressource.ressourceA:
                        meshRenderer.material = GridManager.Instance.resourceAMaterial;
                        break;
                    case Ressource.ressourceB:
                        meshRenderer.material = GridManager.Instance.resourceBMaterial;
                        break;
                    case Ressource.ressourceC:
                        meshRenderer.material = GridManager.Instance.resourceCMaterial;
                        break;
                    case Ressource.resscoureD:
                        meshRenderer.material = GridManager.Instance.resourceDMaterial;
                        break;
                }
                break;
            case GS_neutral: meshRenderer.material = GridManager.Instance.neutralMaterial; break;
            case GS_negative: meshRenderer.material = GridManager.Instance.negativeMaterial; break;
            case GS_Enemy: meshRenderer.material = GridManager.Instance.negativeMaterial; SpawnEnemy(); break;
            case GS_Boss: meshRenderer.material = GridManager.Instance.negativeMaterial; SpawnEnemy(); break;
        }
        
    }

    // STATE MASHINE STUFF ----------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Just changes the current State of this Tile.
    /// </summary>
    /// <param name="newState">State to change into.</param>
    public void ChangeCurrentState(GridState newState)
    {
        currentGridState.ExitState(this);
        currentGridState = newState;
        currentGridState.EnterState(this);
    }

    // UTILITY STUFF ----------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Spawns an Enemy on this Tile.
    /// </summary>
    public void SpawnEnemy()
    {
        GridManager gridManagerInstance = GridManager.Instance;
        Enemy newEnemy = Instantiate(gridManagerInstance.enemyPrefab);
        newEnemy.Setup(gridManagerInstance.enemySOs[Random.Range(0, gridManagerInstance.enemySOs.Count)]);
        newEnemy.transform.parent = transform;
        newEnemy.transform.position = transform.position;
    }

    // MESH TILE STUFF --------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Function DrawMesh is called, to generate the Mesh of this GridCell.
    /// </summary>
    public void DrawMesh()
    {
        DrawFaces();
        CombineFaces();
    }

    /// <summary>
    /// Function DrawFaces Generates faces for a Hexagonal Shape.
    /// </summary>
    public void DrawFaces()
    {
        faces = new List<Face>();
        for (int i = 0; i < 6; i++)
        {
            faces.Add(CreateFace(innerSize, outerSize, height / 2f, height / 2f, i));
        }
    }

    /// <summary>
    /// Function CombineFaces combines the Faces of this GridCell to one Mesh.
    /// <br>See also: <seealso cref="Face"/></br>
    /// </summary>
    public void CombineFaces()
    {
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

    /// <summary>
    /// Function CreateFace creates a Face with 2 triangles.
    /// </summary>
    /// <param name="innerRad">Distance between inner vertices.</param>
    /// <param name="outerRad">Distance between outer vertices.</param>
    /// <param name="heightA">outer height.</param>
    /// <param name="heightB">inner height.</param>
    /// <param name="point">index of Face</param>
    /// <param name="reverse">to reverse vertices to render on opposing side.</param>
    /// <returns>returns thing</returns>
    private Face CreateFace(float innerRad, float outerRad, float heightA, float heightB, int point, bool reverse = false)
    {
        // 4 Points to form the Face.
        // 
        Vector3 p1 = GetPoint(innerRad, heightB, point);
        Vector3 p2 = GetPoint(innerRad, heightB, (point < 5) ? point + 1 : 0);
        Vector3 p3 = GetPoint(outerRad, heightA, (point < 5) ? point + 1 : 0);
        Vector3 p4 = GetPoint(outerRad, heightA, point);

        List<Vector3> vertices = new List<Vector3>() { p1, p2, p3, p4 };
        List<int> triangles = new List<int>()
        {
            0, 1, 2,
            2, 3, 0
        };
        List<Vector2> uvs = new List<Vector2>() { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) };
        if (reverse)
        {
            vertices.Reverse();
        }

        return new Face(vertices, triangles, uvs);
    }

    /// <summary>
    /// returns the Vertex for one Face of the Hexagon.
    /// </summary>
    /// <param name="size">radius of outer circle.</param>
    /// <param name="height">y Position</param>
    /// <param name="index">Face index in the Hexagon.</param>
    /// <returns>Vector3 for vertex of faces, forming the Hexagon.</returns>
    private Vector3 GetPoint(float size, float height, int index)
    {
        // Triangles that form a regular Hexagon have every angle in 60°. So i multiply index with 60 to get every rotation for a Hexagon.
        float angle_deg = 60 * index;

        // translate degree of the angle into radius.
        float angle_rad = Mathf.PI / 180f * angle_deg;

        // each corner has a distance to the Center of "size". This Distance needs to be translated to x and z coordinate.
        // the x coordinate needs to multiply cos of the radial angle with size.
        // the z coordinate needs to multiply sin of the radial angle with size.
        return new Vector3((size * Mathf.Cos(angle_rad)), height, size * Mathf.Sin(angle_rad));
    }
}
