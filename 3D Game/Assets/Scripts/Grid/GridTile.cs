using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
public enum Direction { C = 0, NE = 1, SE = 2, S = 3, SW = 4, NW = 5, N = 6 }
// ^^^^

/// <summary>
/// MonoBehaviour Class to hold Information and provide functions of a hexagonal Cell.
/// It also generates it's mesh.
/// </summary>
public class GridTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color SplatMapColor1;
    public Color SplatMapColor2;
    public Color SplatMapColor3;


    public GridTileSO gridTileSO;
    public Vector2Int AxialCoordinate;

    // vvvv new World
    const float bufferRadius = 0.75f;
    const float outerRadius = 1f;
    const float innerRadius = outerRadius * 0.866025404f;

    List<GridTile> myNeighbors;


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
    List<Vector3> uvs;
    List<Color> vertexColors;

    public Dictionary<Direction, Vector3> Points;

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

   

    public void HighlightEnemySpreadPrediction()
    {
        Instantiate(enemySpreadTileHighlightPrefab, transform.localPosition, Quaternion.identity, transform);
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
        GetComponent<RessourceVisuals>().Setup();
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
        try
        {
            RecalculateTerrain();
            if (myNeighbors == null)
            {
                return;
            }
            foreach (GridTile neighbor in myNeighbors)
            {
                neighbor.RecalculateTerrain();
            }
        }
        catch
        {
        }
        

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
            newEnemy.transform.localPosition = transform.localPosition;
        }
    }

    // MESH TILE STUFF --------------------------------------------------------------------------------------------------------------

    // vvvv new World
    public void Triangluate()
    {
        Points = new Dictionary<Direction, Vector3>();
        Points.Add(Direction.C, new Vector3(0, 0, 0));
        vertices = new List<Vector3>();
        triangles = new List<int>();
        vertexColors = new List<Color>();
        uvs = new List<Vector3>();

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

            Vector3 currentInbetween = Vector3.Lerp(currentBufferedCorner, nextBufferedCorner, 1f / 3f);
            Vector3 nextInbetween = Vector3.Lerp(currentBufferedCorner, nextBufferedCorner, 2f / 3f);

            Vector3 currentInnerCorner = (currentCorner + nextCorner) * 0.5f * (1f - bufferRadius) + currentBufferedCorner;
            Vector3 currentInnerInbetweenCorner = (currentCorner + nextCorner) * 0.5f * (1f - bufferRadius) + currentInbetween;
            Vector3 nextInnerInbetweenCorner = (currentCorner + nextCorner) * 0.5f * (1f - bufferRadius) + nextInbetween;
            Vector3 nextInnerCorner = (currentCorner + nextCorner) * 0.5f * (1f - bufferRadius) + nextBufferedCorner;





            if (currentNeighbor != null)
            {
                currentInnerCorner.y = (transform.localPosition.y + currentNeighbor.transform.localPosition.y) / 2f - transform.localPosition.y;
                nextInnerCorner.y = (transform.localPosition.y + currentNeighbor.transform.localPosition.y) / 2f - transform.localPosition.y;
                currentInbetween.y = Mathf.Lerp(currentBufferedCorner.y, nextBufferedCorner.y, 1f / 3f);
                nextInbetween.y = Mathf.Lerp(currentBufferedCorner.y, nextBufferedCorner.y, 2f / 3f);
                currentInnerInbetweenCorner.y = Mathf.Lerp(currentInnerCorner.y, nextInnerCorner.y, 1f / 3f);
                nextInnerInbetweenCorner.y = Mathf.Lerp(currentInnerCorner.y, nextInnerCorner.y, 2f / 3f);

            }


            Points.Add((Direction)i + 1, currentBufferedCorner);

            //else
            //{
            //    nextInnerCorner.y = transform.localPosition.y;
            //}
            if (prevNeigbor != null && currentNeighbor != null)
            {
                currentCorner.y = (transform.localPosition.y + prevNeigbor.transform.localPosition.y + currentNeighbor.transform.localPosition.y) / 3f - transform.localPosition.y;
            }
            else if (prevNeigbor != null && currentNeighbor == null)
            {
                currentCorner.y = (transform.localPosition.y + prevNeigbor.transform.localPosition.y) / 2f - transform.localPosition.y;
            }
            else if (currentNeighbor != null && prevNeigbor == null)
            {
                currentCorner.y = (transform.localPosition.y + currentNeighbor.transform.localPosition.y) / 2f - transform.localPosition.y;
            }
            //else
            //{
            //    currentCorner.y = transform.localPosition.y;
            //}
            if (currentNeighbor != null && nextNeighbor != null)
            {
                nextCorner.y = (transform.localPosition.y + currentNeighbor.transform.localPosition.y + nextNeighbor.transform.localPosition.y) / 3f - transform.localPosition.y;
            }
            else if (currentNeighbor != null && nextNeighbor == null)
            {
                nextCorner.y = (transform.localPosition.y + currentNeighbor.transform.localPosition.y) / 2f - transform.localPosition.y;
            }
            else if (currentNeighbor == null && nextNeighbor != null)
            {
                nextCorner.y = (transform.localPosition.y + nextNeighbor.transform.localPosition.y) / 2f - transform.localPosition.y;
            }
            //else
            //{
            //    nextCorner.y = transform.localPosition.y;
            //}

            if(ressource == 0)
            {

            }

            // inner triangles
            AddTriangle(Vector3.zero+ (ressource == 0?Vector3.down*.2f:Vector3.zero), currentBufferedCorner, currentInbetween);
            AddTriangle(Vector3.zero + (ressource == 0 ? Vector3.down * .2f : Vector3.zero), currentInbetween, nextInbetween);
            AddTriangle(Vector3.zero + (ressource == 0 ? Vector3.down * .2f : Vector3.zero), nextInbetween, nextBufferedCorner);
            AddTriangleColors(SplatMapColor1, SplatMapColor1, SplatMapColor1);
            AddTriangleColors(SplatMapColor1, SplatMapColor1, SplatMapColor1);
            AddTriangleColors(SplatMapColor1, SplatMapColor1, SplatMapColor1);
            AddTerrainIndexes(this, this, this);
            AddTerrainIndexes(this, this, this);
            AddTerrainIndexes(this, this, this);

            // the Quads combining two adjacent hexes or form the outer line
            //if (currentNeighbor == null)
            //{
            //    // small bridge
            //    AddTriangle(currentBufferedCorner, currentInnerCorner, currentInnerInbetweenCorner);
            //    AddTriangle(currentBufferedCorner, currentInnerInbetweenCorner, currentInbetween);
            //    AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor2);
            //    AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor1);
            //    AddTerrainIndexes(ressource, (currentNeighbor != null ? currentNeighbor.ressource : ressource), (currentNeighbor != null ? currentNeighbor.ressource : ressource));
            //    AddTerrainIndexes(ressource, ressource, (currentNeighbor != null ? currentNeighbor.ressource : ressource));

            //    AddTriangle(currentInbetween, currentInnerInbetweenCorner, nextInnerInbetweenCorner);
            //    AddTriangle(currentInbetween, nextInnerInbetweenCorner, nextInbetween);
            //    AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor2);
            //    AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor1);
            //    AddTerrainIndexes(ressource, (currentNeighbor != null ? currentNeighbor.ressource : ressource), (currentNeighbor != null ? currentNeighbor.ressource : ressource));
            //    AddTerrainIndexes(ressource, ressource, (currentNeighbor != null ? currentNeighbor.ressource : ressource));

            //    AddTriangle(nextInbetween, nextInnerInbetweenCorner, nextInnerCorner);
            //    AddTriangle(nextInbetween, nextInnerCorner, nextBufferedCorner);
            //    AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor2);
            //    AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor1);
            //    AddTerrainIndexes(ressource, (currentNeighbor != null ? currentNeighbor.ressource : ressource), (currentNeighbor != null ? currentNeighbor.ressource : ressource));
            //    AddTerrainIndexes(ressource, ressource, (currentNeighbor != null ? currentNeighbor.ressource : ressource));
            //}
            /*//else */

            // Bridge Triangles to directly adjacent Neighbors
            if (i < 3 && currentNeighbor != null)
            {
                // big bridge
                Vector3 newCurrentInnerCorner = (currentCorner + nextCorner) * 0.5f * (1f - bufferRadius) * 2 + currentBufferedCorner;
                newCurrentInnerCorner.y = 0;
                newCurrentInnerCorner.y = currentNeighbor.transform.localPosition.y - transform.localPosition.y;
                Vector3 newNextInnerCorner = (currentCorner + nextCorner) * 0.5f * (1f - bufferRadius) * 2 + nextBufferedCorner;
                newNextInnerCorner.y = 0;
                newNextInnerCorner.y = currentNeighbor.transform.localPosition.y - transform.localPosition.y;
                Vector3 newcurrentInnerInbetweenCorner = Vector3.Lerp(newCurrentInnerCorner, newNextInnerCorner, 1f / 3f);
                newcurrentInnerInbetweenCorner.y = 0;
                newcurrentInnerInbetweenCorner.y = Mathf.Lerp(newCurrentInnerCorner.y, newNextInnerCorner.y, 1f / 3f);
                Vector3 newnextInnerInbetweenCorner = Vector3.Lerp(newCurrentInnerCorner, newNextInnerCorner, 2f / 3f);
                newnextInnerInbetweenCorner.y = 0;
                newnextInnerInbetweenCorner.y = Mathf.Lerp(newCurrentInnerCorner.y, newNextInnerCorner.y, 2f / 3f);


                AddTriangle(currentBufferedCorner, newCurrentInnerCorner, newcurrentInnerInbetweenCorner);
                AddTriangle(currentBufferedCorner, newcurrentInnerInbetweenCorner, currentInbetween);
                AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor2);
                AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor1);
                AddTerrainIndexes(this, (currentNeighbor != null ? currentNeighbor : this), (currentNeighbor != null ? currentNeighbor : this));
                AddTerrainIndexes(this, (currentNeighbor != null ? currentNeighbor : this), this);

                AddTriangle(currentInbetween, newcurrentInnerInbetweenCorner, newnextInnerInbetweenCorner);
                AddTriangle(currentInbetween, newnextInnerInbetweenCorner, nextInbetween);
                AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor2);
                AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor1);
                AddTerrainIndexes(this, (currentNeighbor != null ? currentNeighbor : this), (currentNeighbor != null ? currentNeighbor : this));
                AddTerrainIndexes(this, (currentNeighbor != null ? currentNeighbor : this), this);

                AddTriangle(nextInbetween, newnextInnerInbetweenCorner, newNextInnerCorner);
                AddTriangle(nextInbetween, newNextInnerCorner, nextBufferedCorner);
                AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor2);
                AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor1);
                AddTerrainIndexes(this, (currentNeighbor != null ? currentNeighbor : this), (currentNeighbor != null ? currentNeighbor : this));
                AddTerrainIndexes(this, (currentNeighbor != null ? currentNeighbor : this), this);


            }
            else if (currentNeighbor == null)
            {
                // Walls on worlds edge
                currentInnerCorner.y = /*-transform.localPosition.y*/-3;
                currentInnerInbetweenCorner.y = /*-transform.localPosition.y **/-3;
                nextInnerInbetweenCorner.y = /*-transform.localPosition.y **/-3;
                nextInnerCorner.y = /*-transform.localPosition.y **/ -3;

                AddTriangle(currentBufferedCorner, currentInnerCorner, currentInnerInbetweenCorner);
                AddTriangle(currentBufferedCorner, currentInnerInbetweenCorner, currentInbetween);
                AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor2);
                AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor1);
                AddTerrainIndexes(this, this, this);
                AddTerrainIndexes(this, this, this);

                AddTriangle(currentInbetween, currentInnerInbetweenCorner, nextInnerInbetweenCorner);
                AddTriangle(currentInbetween, nextInnerInbetweenCorner, nextInbetween);
                AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor2);
                AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor1);
                AddTerrainIndexes(this, this, this);
                AddTerrainIndexes(this, this, this);

                AddTriangle(nextInbetween, nextInnerInbetweenCorner, nextInnerCorner);
                AddTriangle(nextInbetween, nextInnerCorner, nextBufferedCorner);
                AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor2);
                AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor1);
                AddTerrainIndexes(this, this, this);
                AddTerrainIndexes(this, this, this);
            }




            // Triangles on the Corner of the Hexes
            if (currentNeighbor != null && nextNeighbor != null && i < 2)
            {
                // triangles between 3 hexes
                Vector3 newNextInnerCorner = (currentCorner + nextCorner) * 0.5f * (1f - bufferRadius) * 2 + nextBufferedCorner;
                newNextInnerCorner.y = 0;
                newNextInnerCorner.y = currentNeighbor.transform.localPosition.y - transform.localPosition.y;
                Vector3 newCurrentCorner = newNextInnerCorner;
                newCurrentCorner.y = 0;
                newCurrentCorner.y = currentNeighbor.transform.localPosition.y - transform.localPosition.y;
                Vector3 xDD = corners[i + 1 == 5 ? 0 : i + 1 + 1];
                Vector3 xDDDD = (xDD + nextCorner) * 0.5f * (1f - bufferRadius) * 2 + nextBufferedCorner;
                xDDDD.y = 0;
                xDDDD.y = nextNeighbor.transform.localPosition.y - transform.localPosition.y;


                // big Triangle
                AddTriangle(nextBufferedCorner, newCurrentCorner, xDDDD);
                AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor3);
                AddTerrainIndexes(this, (currentNeighbor != null ? currentNeighbor : this), (nextNeighbor != null ? nextNeighbor : this));

                Vector3 uwu = currentCorner;
                uwu.y = /*-transform.localPosition.y*/-3;
                // small triangle
                AddTriangle(currentBufferedCorner, uwu, currentInnerCorner);
                AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor3);
                AddTerrainIndexes(this, this, (currentNeighbor != null ? currentNeighbor : this));

            }
            else
            {
                // triangles on World Edge

                currentCorner.y = /*-transform.localPosition.y * 5f*/-3;
                nextCorner.y = /*-transform.localPosition.y * 5f*/-3;

                AddTriangle(currentBufferedCorner, currentCorner, currentInnerCorner);
                AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor3);
                AddTerrainIndexes(this, this, (currentNeighbor != null ? currentNeighbor : this));

                AddTriangle(nextBufferedCorner, nextInnerCorner, nextCorner);
                AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor3);
                AddTerrainIndexes(this, this, (currentNeighbor != null ? currentNeighbor : this));


            }
            //else/* (prevNeigbor == null || nextNeighbor == null || currentNeighbor == null)*/
            //{
            //    AddTriangle(currentBufferedCorner, currentCorner, currentInnerCorner);
            //    AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor3);
            //    AddTerrainIndexes(ressource, ressource, ressource);

            //    AddTriangle(nextBufferedCorner, nextInnerCorner, nextCorner);
            //    AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor3);
            //    AddTerrainIndexes(ressource, ressource, ressource);
            //}

        }








        // triangles for the Corner of the Hexagon




        pertulate(GridManager.Instance.mapSettings);
        mesh = new Mesh();
        mesh.name = "HexMesh " + AxialCoordinate;
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        mesh.SetUVs(2, uvs.ToArray());
        //mesh.uv = uvs.ToArray();
        mesh.colors = vertexColors.ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        if (Application.isPlaying)
        {
            meshCollider = GetComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;
        }
        GetComponent<MeshFilter>().mesh = mesh;

        Debug.Log("----------------------------------------------");
        foreach(KeyValuePair<Direction,Vector3> kvp in Points)
        {
            Debug.Log("Direction: " + kvp.Key + ", Vector: " + kvp.Value);
        }
        Debug.Log("----------------------------------------------");
    }

    public void RecalculateTerrain()
    {
        mesh = new Mesh();
        vertexColors = new List<Color>();
        uvs = new List<Vector3>();

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

            AddTriangleColors(SplatMapColor1, SplatMapColor1, SplatMapColor1);
            AddTriangleColors(SplatMapColor1, SplatMapColor1, SplatMapColor1);
            AddTriangleColors(SplatMapColor1, SplatMapColor1, SplatMapColor1);
            AddTerrainIndexes(this, this, this);
            AddTerrainIndexes(this, this, this);
            AddTerrainIndexes(this, this, this);

            // Bridge Triangles to directly adjacent Neighbors
            if (i < 3 && currentNeighbor != null)
            {
                // big bridge
                AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor2);
                AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor1);
                AddTerrainIndexes(this, (currentNeighbor != null ? currentNeighbor : this), (currentNeighbor != null ? currentNeighbor : this));
                AddTerrainIndexes(this, (currentNeighbor != null ? currentNeighbor : this), this);

                AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor2);
                AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor1);
                AddTerrainIndexes(this, (currentNeighbor != null ? currentNeighbor : this), (currentNeighbor != null ? currentNeighbor : this));
                AddTerrainIndexes(this, (currentNeighbor != null ? currentNeighbor : this), this);

                AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor2);
                AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor1);
                AddTerrainIndexes(this, (currentNeighbor != null ? currentNeighbor : this), (currentNeighbor != null ? currentNeighbor : this));
                AddTerrainIndexes(this, (currentNeighbor != null ? currentNeighbor : this), this);

            }
            else if (currentNeighbor == null)
            {
                // Walls on worlds edge
                AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor2);
                AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor1);
                AddTerrainIndexes(this, this, this);
                AddTerrainIndexes(this, this, this);

                AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor2);
                AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor1);
                AddTerrainIndexes(this, this, this);
                AddTerrainIndexes(this, this, this);

                AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor2);
                AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor1);
                AddTerrainIndexes(this, this, this);
                AddTerrainIndexes(this, this, this);
            }

            // Triangles on the Corner of the Hexes
            if (currentNeighbor != null && nextNeighbor != null && i < 2)
            {
                // triangles between 3 hexes
                AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor3);
                AddTerrainIndexes(this, (currentNeighbor != null ? currentNeighbor : this), (nextNeighbor != null ? nextNeighbor : this));

                // small triangle
                AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor3);
                AddTerrainIndexes(this, this, (currentNeighbor != null ? currentNeighbor : this));

            }
            else
            {
                // triangles on World Edge
                AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor3);
                AddTerrainIndexes(this, this, (currentNeighbor != null ? currentNeighbor : this));

                AddTriangleColors(SplatMapColor1, SplatMapColor2, SplatMapColor3);
                AddTerrainIndexes(this, this, (currentNeighbor != null ? currentNeighbor : this));
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.SetUVs(2, uvs.ToArray());
        mesh.colors = vertexColors.ToArray();

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
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

        //uvs.Add(new Vector3(a.x,a.y, a.z)+transform.localPosition);
        //uvs.Add(new Vector3(b.x,b.y, b.z) + transform.localPosition);
        //uvs.Add(new Vector3(c.x,c.y, c.z) + transform.localPosition);

    }

    public void AddTerrainIndexes(Ressource resA, Ressource resB, Ressource resC)
    {
        AddTriangleTerrainTypes(getTerrainIndex(resA), getTerrainIndex(resB), getTerrainIndex(resC));
    }

    public void AddTerrainIndexes(GridTile tileA, GridTile tileB, GridTile tileC)
    {
        try
        {
            int resa = getTerrainIndexWithDesat(tileA);
            int resb = getTerrainIndexWithDesat(tileB);
            int resc = getTerrainIndexWithDesat(tileC);
            AddTriangleTerrainTypes(resa, resb, resc);
        }
        catch
        {
            Debug.LogWarning("CATCHBLOCK: --------------> Function AddTerrainIndexes has been called with a GridTile == null. \n Setting terrainIndexes to my own default ressource now");
            AddTriangleTerrainTypes(getTerrainIndex(ressource), getTerrainIndex(ressource), getTerrainIndex(ressource));
        }
    }

    public int getTerrainIndexWithDesat(GridTile tile)
    {
        if (tile.currentGridState.StateValue() < 0)
        {
            return 4;
        }
        else if (tile.currentGridState.StateValue() == 0)
        {
            return getTerrainIndex(tile.ressource) + 4;
        }
        else
        {
            return getTerrainIndex(tile.ressource);
        }
    }

    public int getTerrainIndex(Ressource ressource)
    {
        switch (ressource)
        {
            case Ressource.ressourceA: return 0;
            case Ressource.ressourceB: return 1;
            case Ressource.ressourceC: return 2;
            default: return 3;
        }
    }

    public void AddTriangleTerrainTypes(int a, int b, int c)
    {
        uvs.Add(new Vector3((float)a, (float)b, (float)c));
        uvs.Add(new Vector3((float)a, (float)b, (float)c));
        uvs.Add(new Vector3((float)a, (float)b, (float)c));
    }

    public Vector2 Remap(Vector2 input)
    {
        Vector3 result = new Vector3();

        result /= 2f;
        result += Vector3.one * .5f;

        return result;
    }

    public void pertulate(MapSettings mapsettings)
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            if (Points.ContainsValue(vertices[i]))
            {
                Direction myDirection = Points.FirstOrDefault(x => x.Value == vertices[i]).Key;
                vertices[i] += mapsettings.GetIrregularityNoiseData(vertices[i] + transform.localPosition) * mapsettings.IrregularityFactor;
                Points[myDirection] = vertices[i];
                continue;
            }
            vertices[i] += mapsettings.GetIrregularityNoiseData(vertices[i] + transform.localPosition) * mapsettings.IrregularityFactor;
            
        }
    }

    public void AddTriangleColors(Color colorA, Color colorB, Color colorC)
    {
        vertexColors.Add(colorA);
        vertexColors.Add(colorB);
        vertexColors.Add(colorC);
        //uvs.Add(Vector3.one* 2);
        //uvs.Add(Vector3.one* 2);
        //uvs.Add(Vector3.one* 2);
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
                Instantiate(moveTileHighlightPrefab, transform.localPosition, Quaternion.identity, transform);
                PlayerManager.Instance.selectedPlayer.transform.DOLookAt(new Vector3(transform.position.x, PlayerManager.Instance.selectedPlayer.transform.position.y, transform.position.z), .1f,up:Vector3.up);
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

    public void UpdateMyNeighbors()
    {
        myNeighbors = new List<GridTile>();
        List<Vector2Int> neighborCoords = HexGridUtil.AxialNeighbors(AxialCoordinate);
        foreach (Vector2Int coord in neighborCoords)
        {
            if (GridManager.Instance.Grid.ContainsKey(coord))
            {
                myNeighbors.Add(GridManager.Instance.Grid[coord]);
            }
        }
    }
}
