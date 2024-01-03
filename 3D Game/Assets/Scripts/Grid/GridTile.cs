using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.Experimental.Rendering.RayTracingAccelerationStructure;

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

// vvvv new World
public enum direction { NE = 0, SE = 1, S = 2, SW = 3, NW = 4, N = 5 }
// ^^^^

/// <summary>
/// MonoBehaviour Class to hold Information and provide functions of a hexagonal Cell.
/// It also generates it's mesh.
/// </summary>
public class GridTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GridTileSO gridTileSO;
    public Vector2Int AxialCoordinate;

    // vvvv new World
    const float bufferRadius = 0.75f;
    const float outerRadius = 1f;
    const float innerRadius = outerRadius * 0.866025404f;


    List<Vector3> corners = new List<Vector3>()
    {
        new Vector3(outerRadius*.5f,0,innerRadius),     // upper right corner
        new Vector3(outerRadius,0,0),                   // left corner
        new Vector3(outerRadius*.5f,0,-innerRadius),    // lower right corner
        new Vector3(-outerRadius*.5f,0,-innerRadius),   // lower left corner
        new Vector3(-outerRadius,0,0),                  // left Corner
        new Vector3(-outerRadius*.5f,0,innerRadius)     // upper left corner
    };


    List<Vector3> vertices;
    List<int> triangles;
    List<Color> vertexColors;
    // ^^^^

    [HideInInspector] public float innerSize = 0;
    [HideInInspector] public float outerSize = 1;
    [HideInInspector] public float height = 0;
    Mesh mesh;
    MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    MeshCollider meshCollider;
    List<Face> faces;

    bool withWalls = false;

    [Header("Tile Statestuff")]
    public GridState currentGridState;
    public Ressource ressource;
    public TileHighlight moveTileHighlightPrefab;
    public TileHighlight enemySpreadTileHighlightPrefab;

    public int neutralTurnCounter = 0;
    public int NeutralRegenerationTime;

    private void Start()
    {
        EventManager.OnEndTurnEvent += neutralRegeneration;
        switch (currentGridState)
        {
            case GS_positive:
                switch (ressource)
                {
                    case Ressource.ressourceA:
                        meshRenderer.material = gridTileSO.resourceAMaterial;
                        ressource = Ressource.ressourceA;
                        break;
                    case Ressource.ressourceB:
                        meshRenderer.material = gridTileSO.resourceBMaterial;
                        ressource = Ressource.ressourceB;
                        break;
                    case Ressource.ressourceC:
                        meshRenderer.material = gridTileSO.resourceCMaterial;
                        ressource = Ressource.ressourceC;
                        break;
                    case Ressource.ressourceD:
                        meshRenderer.material = gridTileSO.resourceDMaterial;
                        ressource = Ressource.ressourceD;
                        break;
                }
                break;
            case GS_neutral: meshRenderer.material = gridTileSO.neutralMaterial; break;
            case GS_negative: meshRenderer.material = gridTileSO.negativeMaterial; break;
            case GS_Enemy: meshRenderer.material = gridTileSO.negativeMaterial; SpawnEnemy(); break;
            case GS_Boss: meshRenderer.material = gridTileSO.negativeMaterial; SpawnEnemy(); break;
            case GS_BossNegative: meshRenderer.material = gridTileSO.negativeMaterial; break;
            case GS_Pofl: meshRenderer.material = gridTileSO.PofIMaterial; break;
        }
    }

    private void OnDestroy()
    {
        EventManager.OnEndTurnEvent -= neutralRegeneration;
    }

    /// <summary>
    /// Sets up the Tile. Should allway be called when instantiating the Tile.
    /// </summary>
    /// <param name="cellCoordinate">Axial Coordinate in the the Grid.</param>
    /// <param name="resource">Type of Ressource for the Tile.</param>
    /// <param name="gridstate">The Gridstate to start with.</param>
    public void Setup(Vector2Int cellCoordinate, GridTileSO gridTileSO, GridState gridstate, bool withWalls)
    {


        this.withWalls = withWalls;
        this.gridTileSO = gridTileSO;
        meshCollider = GetComponent<MeshCollider>();
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        mesh = new Mesh();
        mesh.name = $"Hex{cellCoordinate.x},{cellCoordinate.y}";

        this.AxialCoordinate = cellCoordinate;
        this.ressource = gridTileSO.ressource;
        this.currentGridState = gridstate;
        switch (gridstate)
        {
            case GS_positive:
                switch (ressource)
                {
                    case Ressource.ressourceA:
                        meshRenderer.material = gridTileSO.resourceAMaterial;
                        ressource = Ressource.ressourceA;
                        break;
                    case Ressource.ressourceB:
                        meshRenderer.material = gridTileSO.resourceBMaterial;
                        ressource = Ressource.ressourceB;
                        break;
                    case Ressource.ressourceC:
                        meshRenderer.material = gridTileSO.resourceCMaterial;
                        ressource = Ressource.ressourceC;
                        break;
                    case Ressource.ressourceD:
                        meshRenderer.material = gridTileSO.resourceDMaterial;
                        ressource = Ressource.ressourceD;
                        break;
                }
                break;
            case GS_neutral: meshRenderer.material = gridTileSO.neutralMaterial; break;
            case GS_negative: meshRenderer.material = gridTileSO.negativeMaterial; break;
            case GS_Enemy: meshRenderer.material = gridTileSO.negativeMaterial; SpawnEnemy(); break;
            case GS_Boss: meshRenderer.material = gridTileSO.negativeMaterial; SpawnEnemy(); break;
            case GS_BossNegative: meshRenderer.material = gridTileSO.negativeMaterial; break;
            case GS_Pofl: meshRenderer.material = gridTileSO.PofIMaterial; break;
        }

        meshFilter.mesh = DrawMesh();


    }

    public void HighlightEnemySpreadPrediction()
    {
        Instantiate(enemySpreadTileHighlightPrefab, transform.position, Quaternion.identity, transform);
    }

    public void Setup(Vector2Int cellCoordinate, GridTileSO gridTileSO, bool withWalls)
    {
        this.withWalls = withWalls;
        this.gridTileSO = gridTileSO;
        meshCollider = GetComponent<MeshCollider>();
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        mesh = new Mesh();
        mesh.name = "Hex";
        meshFilter.mesh = mesh;

        AxialCoordinate = cellCoordinate;
        this.ressource = gridTileSO.ressource;
        //gridStateString = this.currentGridState.ToString();
        switch (ressource)
        {
            case Ressource.ressourceA:
                meshRenderer.material = gridTileSO.resourceAMaterial;
                break;
            case Ressource.ressourceB:
                meshRenderer.material = gridTileSO.resourceBMaterial;
                break;
            case Ressource.ressourceC:
                meshRenderer.material = gridTileSO.resourceCMaterial;
                break;
            case Ressource.ressourceD:
                meshRenderer.material = gridTileSO.resourceDMaterial;
                break;
        }

        DrawMesh();
    }

    public void Setup(Vector2Int coordinate, Ressource ressource, bool withWalls)
    {
        this.withWalls = withWalls;
        meshCollider = GetComponent<MeshCollider>();
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        mesh = new Mesh();
        mesh.name = $"Hex{coordinate.x},{coordinate.y}";

        AxialCoordinate = coordinate;
        this.ressource = ressource;
        currentGridState = GridManager.Instance.gS_Positive;

        //meshFilter.mesh = DrawMesh();

    }

    // STATE MASHINE STUFF ----------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Just changes the current State of this Tile.
    /// </summary>
    /// <param name="newState">State to change into.</param>
    public void ChangeCurrentState(GridState newState)
    {
        if (newState == GridManager.Instance.gS_Neutral)
        {
            neutralTurnCounter = 0;
        }
        try
        {
            currentGridState.ExitState(this);
        }
        catch
        {

        }
        currentGridState = newState;
        currentGridState.EnterState(this);
    }

    // UTILITY STUFF ----------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Spawns an Enemy on this Tile.
    /// </summary>
    public void SpawnEnemy()
    {
        if (GetComponentInChildren<Enemy>() == null)
        {
            GridManager gridManagerInstance = GridManager.Instance;
            Enemy newEnemy = Instantiate(gridManagerInstance.StartEnemyPrefabs[Random.Range(0, gridManagerInstance.StartEnemyPrefabs.Count)]);
            newEnemy.Setup(/*gridManagerInstance.enemySOs[Random.Range(0, gridManagerInstance.enemySOs.Count)], */this);
            newEnemy.transform.parent = transform;
            newEnemy.transform.position = transform.position;
        }
    }

    // MESH TILE STUFF --------------------------------------------------------------------------------------------------------------

    // vvvv new World
    public void Triangluate()
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();
        vertexColors = new List<Color>();

        List<Vector2Int> neighborCords = HexGridUtil.AxialNeighbors(AxialCoordinate);

        GridTile prevNeigbor = null;
        GridTile currentNeighbor = null;
        GridTile nextNeighbor = null;

        for (int i = 0; i < 6; i++)
        {
            prevNeigbor = null;
            currentNeighbor = null;
            nextNeighbor = null;
            Color prevNeighborColor;
            Color currentNeighborColor;
            Color nextNeighborColor;
            Color myColor = GetColor(ressource);

            if (GridManager.Instance.Grid.ContainsKey(neighborCords[i]))
            {
                currentNeighbor = GridManager.Instance.Grid[neighborCords[i]];
                currentNeighborColor = currentNeighbor.GetColor(currentNeighbor.ressource);
            }
            else
            {
                currentNeighbor = null;
                currentNeighborColor = myColor;
            }

            if (GridManager.Instance.Grid.ContainsKey(neighborCords[i == 5 ? 0 : i + 1]))
            {
                nextNeighbor = GridManager.Instance.Grid[neighborCords[i == 5 ? 0 : i + 1]];
                nextNeighborColor = nextNeighbor.GetColor(nextNeighbor.ressource);
            }
            else
            {
                nextNeighbor = null;
                nextNeighborColor = myColor;
            }

            if (GridManager.Instance.Grid.ContainsKey(neighborCords[i == 0 ? 5 : i - 1]))
            {
                prevNeigbor = GridManager.Instance.Grid[neighborCords[i == 0 ? 5 : i - 1]];
                prevNeighborColor = prevNeigbor.GetColor(prevNeigbor.ressource);
            }
            else
            {
                prevNeigbor = null;
                prevNeighborColor = myColor;
            }

            Vector3 currentCorner = corners[i];
            Vector3 nextCorner = corners[i == 5 ? 0 : i + 1];
            Vector3 currentBufferedCorner = currentCorner * bufferRadius;
            Vector3 nextBufferedCorner = nextCorner * bufferRadius;
            Vector3 currentInnerCorner = (currentCorner + nextCorner) * 0.5f * (1f - bufferRadius) + currentBufferedCorner;
            Vector3 nextInnerCorner = (currentCorner + nextCorner) * 0.5f * (1f - bufferRadius) + nextBufferedCorner;


            if (currentNeighbor != null)
            {
                currentInnerCorner.y = (transform.position.y + currentNeighbor.transform.position.y) / 2f - transform.position.y;
                nextInnerCorner.y = (transform.position.y + currentNeighbor.transform.position.y) / 2f - transform.position.y;
            }
            //else
            //{
            //    nextInnerCorner.y = transform.position.y;
            //}
            if (prevNeigbor != null && currentNeighbor != null)
            {
                currentCorner.y = (transform.position.y + prevNeigbor.transform.position.y + currentNeighbor.transform.position.y) / 3f - transform.position.y;
            }
            else if (prevNeigbor != null && currentNeighbor == null)
            {
                currentCorner.y = (transform.position.y + prevNeigbor.transform.position.y) / 2f - transform.position.y;
            }
            else if (currentNeighbor != null && prevNeigbor == null)
            {
                currentCorner.y = (transform.position.y + currentNeighbor.transform.position.y) / 2f - transform.position.y;
            }
            //else
            //{
            //    currentCorner.y = transform.position.y;
            //}
            if (currentNeighbor != null && nextNeighbor != null)
            {
                nextCorner.y = (transform.position.y + currentNeighbor.transform.position.y + nextNeighbor.transform.position.y) / 3f - transform.position.y;
            }
            else if (currentNeighbor != null && nextNeighbor == null)
            {
                nextCorner.y = (transform.position.y + currentNeighbor.transform.position.y) / 2f - transform.position.y;
            }
            else if (currentNeighbor == null && nextNeighbor != null)
            {
                nextCorner.y = (transform.position.y + nextNeighbor.transform.position.y) / 2f - transform.position.y;
            }
            //else
            //{
            //    nextCorner.y = transform.position.y;
            //}



            // inner triangle
            AddTriangle(Vector3.zero, currentBufferedCorner, nextBufferedCorner);
            AddTriangleColors(myColor, myColor, myColor);

            // the 2 triangles forming the Quad with its direct neighbor
            AddTriangle(currentBufferedCorner, currentInnerCorner, nextInnerCorner);
            AddTriangle(currentBufferedCorner, nextInnerCorner, nextBufferedCorner);
            AddTriangleColors(myColor, AverageColor(new Color[] { myColor, currentNeighborColor }), AverageColor(new Color[] { myColor, currentNeighborColor }));
            AddTriangleColors(myColor, AverageColor(new Color[] { myColor, currentNeighborColor }), myColor);



            // triangles for the Corner of the Hexagon
            AddTriangle(currentBufferedCorner, currentCorner, currentInnerCorner);
            AddTriangleColors(myColor, AverageColor(new Color[] { myColor, prevNeighborColor, currentNeighborColor }), AverageColor(new Color[] { myColor, currentNeighborColor }));

            AddTriangle(nextBufferedCorner, nextInnerCorner, nextCorner);
            AddTriangleColors(myColor, AverageColor(new Color[] { myColor, currentNeighborColor }), AverageColor(new Color[] { myColor, nextNeighborColor, currentNeighborColor }));
        }

        pertulate(GridManager.Instance.mapSettings);
        mesh = new Mesh();
        mesh.name = "HexMesh " + AxialCoordinate;
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.colors = vertexColors.ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        if (Application.isPlaying)
        {
            meshCollider = GetComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;
        }
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void AddTriangle(Vector3 a, Vector3 b, Vector3 c)
    {
        int vertexIndex = vertices.Count;
        vertices.Add(a);
        vertices.Add(b);
        vertices.Add(c);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
    }

    public void pertulate(MapSettings mapsettings)
    {
        for(int i = 0; i < vertices.Count; i++) 
        {
            vertices[i] += mapsettings.GetIrregularityNoiseData(vertices[i]+transform.position)*mapsettings.IrregularityFactor;
        }
    }

    public void AddTriangleColors(Color colorA, Color colorB, Color colorC)
    {
        vertexColors.Add(colorA);
        vertexColors.Add(colorB);
        vertexColors.Add(colorC);
    }

    public Color GetColor(Ressource res)
    {
        if (currentGridState == null)
        {
            return Color.clear;
        }
        if (currentGridState.StateValue() > 0)
        {
            switch (res)
            {
                case Ressource.ressourceA:
                    return gridTileSO.ressourceAColor;
                case Ressource.ressourceB:
                    return gridTileSO.ressourceBColor;
                case Ressource.ressourceC:
                    return gridTileSO.ressourceCColor;
                case Ressource.ressourceD:
                    return gridTileSO.ressourceDColor;
            }
        }
        else if (currentGridState.StateValue() == 0)
        {
            return gridTileSO.neutralColor;
        }
        else
        {
            return gridTileSO.negativeColor;
        }
        return gridTileSO.neutralColor;
    }

    public Color AverageColor(Color[] colors)
    {
        Color avgColor = new Color();
        Vector4 avgColorV = new Vector4();
        foreach (Color color in colors)
        {
            avgColor += color;
        }
        avgColor /= colors.Length;
        return avgColor;
    }
    // ^^^^

    /// <summary>
    /// Function DrawMesh is called, to generate the Mesh of this GridCell.
    /// </summary>
    public Mesh DrawMesh()
    {
        mesh = new Mesh();
        //List<Face> theseFaces = DrawFaces();
        //mesh = CombineFaces(theseFaces);
        Triangluate();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.colors = vertexColors.ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        if (Application.isPlaying)
        {
            meshCollider = GetComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;
        }

        return mesh;
    }

    /// <summary>
    /// Function DrawFaces Generates faces for a Hexagonal Shape.
    /// </summary>
    public List<Face> DrawFaces()
    {
        List<Face> localFaces = new List<Face>();
        faces = new List<Face>();
        for (int i = 0; i < 6; i++)
        {
            Face newFace = CreateFace(innerSize, outerSize, height / 2f, height / 2f, i);
            faces.Add(newFace);
            localFaces.Add(newFace);
            //Debug.Log("FACE -:--------------------------------------------------------------------------------------------------------------------");
            //foreach (Vector3 vertex in newFace.vertices)
            //{
            //    Debug.Log(vertex);
            //}
            //foreach (int triangle in newFace.triangles)
            //{
            //    Debug.Log(triangle);
            //}
            //Debug.Log("FACE -:--------------------------------------------------------------------------------------------------------------------");
        }

        return localFaces;
    }

    /// <summary>
    /// Function CombineFaces combines the Faces of this GridCell to one Mesh.
    /// <br>See also: <seealso cref="Face"/></br>
    /// </summary>
    public Mesh CombineFaces(List<Face> facesToCombine)
    {
        Mesh meh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        for (int i = 0; i < facesToCombine.Count; i++)
        {
            vertices.AddRange(facesToCombine[i].vertices);
            uvs.AddRange(faces[i].uvs);

            int offset = (withWalls ? 8 : 4) * i;
            foreach (int tris in facesToCombine[i].triangles)
            {
                triangles.Add(tris + offset);
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
        meh.vertices = vertices.ToArray();
        meh.triangles = triangles.ToArray();
        meh.uv = uvs.ToArray();
        meh.RecalculateNormals();

        return meh;
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
        Vector3 p1 = GetPoint(innerRad, heightB, point);
        Vector3 p2 = GetPoint(innerRad, heightB, (point < 5) ? point + 1 : 0);
        Vector3 p3 = GetPoint(outerRad, heightA, (point < 5) ? point + 1 : 0);
        Vector3 p4 = GetPoint(outerRad, heightA, point);
        Vector3 p5 = p3 - new Vector3(0, 1, 0);
        Vector3 p6 = p4 - new Vector3(0, 1, 0);

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        if (withWalls)
        {
            vertices.AddRange(new List<Vector3>() { /*0*/p1, /*1*/p2,/*2*/ p3, /*3*/p4,/*4*/ p3,/*5*/ p4,/*6*/ p5,/*7*/ p6 });
            triangles.AddRange(new List<int>()
            {
                0, 1, 2,
                2, 3, 0,
                5,4,6,
                5,6,7
            });
        }
        else
        {
            vertices.AddRange(new List<Vector3>() { /*0*/p1, /*1*/p2,/*2*/ p3, /*3*/p4 });
            triangles.AddRange(new List<int>()
            {
                0, 1, 2,
                2, 3, 0
            });
        }
        List<Vector2> uvs = new List<Vector2>();

        for (int i = 0; i < vertices.Count; i++)
        {
            uvs.Add(new Vector2(vertices[i].x, vertices[i].z));
        }

        //List<Vector2> uvs = new List<Vector2>() { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) };
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (PlayerManager.Instance.movementAction > 0 && !PlayerManager.Instance.abilityActivated && currentGridState.StateValue() >= 0)
        {
            List<Vector3Int> neighbors = HexGridUtil.CubeNeighbors(HexGridUtil.AxialToCubeCoord(PlayerManager.Instance.selectedPlayer.CoordinatePosition));

            if (neighbors.Contains(HexGridUtil.AxialToCubeCoord(AxialCoordinate)))
            {
                Instantiate(moveTileHighlightPrefab, transform.position, Quaternion.identity, transform);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (TileHighlight tileHighlightInstance in GetComponentsInChildren<TileHighlight>())
        {
            Destroy(tileHighlightInstance.gameObject);
        }
    }

    public void neutralRegeneration()
    {
        if (currentGridState == GridManager.Instance.gS_Neutral)
        {
            neutralTurnCounter++;
            if (neutralTurnCounter >= NeutralRegenerationTime)
            {
                neutralTurnCounter = 0;
                ChangeCurrentState(GridManager.Instance.gS_Positive);
            }
        }
    }
}
