using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField] GridTile GridTilePrefab;

    public Dictionary<Vector2Int,GridTile> Grid;


    [SerializeField] Vector2 gridSize;

    [Header("Tile Presets")]
    [SerializeField] float innerSize;
    [SerializeField] float outerSize;
    [SerializeField] float height;

    [Header("Resource Materials")]
    [SerializeField] public Material resourceAMaterial;
    [SerializeField] public Material resourceBMaterial;
    [SerializeField] public Material resourceCMaterial;
    [SerializeField] public Material resourceDMaterial;
    [SerializeField] public Material neutralMaterial;
    [SerializeField] public Material negativeMaterial;

    List<GridState> gridStates;

    [Header("Enemy Ressources")]
    [SerializeField] public Enemy enemyPrefab;
    [SerializeField] public List<EnemySO> enemySOs;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            gridStates = new List<GridState>();
            gridStates.Add(new GS_positive());
            gridStates.Add(new GS_neutral());
            gridStates.Add(new GS_negative());
            gridStates.Add(new GS_Enemy());
            gridStates.Add(new GS_Boss());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();

        Debug.Log("Picking two random Tiles");
        GridTile tileA = PickRandomTile();
        GridTile tileB = PickRandomTile();

        Vector3Int coordTileA = HexGridUtil.AxialToCubeCoord(tileA.CellCoordinate);
        Vector3Int coordTileB = HexGridUtil.AxialToCubeCoord(tileB.CellCoordinate);

        Debug.Log("Coordinate of the first random tile is: " + coordTileA);
        Debug.Log("Coordinate of the second random tile is: " + coordTileB);

        int distance = HexGridUtil.CubeDistance(HexGridUtil.AxialToCubeCoord(tileA.CellCoordinate), HexGridUtil.AxialToCubeCoord(tileB.CellCoordinate));
        Debug.Log("The Distance of the two random Tiles is: " + distance);

        Debug.Log("NO I ELEVATE EVERY TILE IN A LINE BETWEEN THE TWO RANDOM TILES");

        List<Vector3Int> OnLineCube = HexGridUtil.CubeLineDraw(coordTileA,coordTileB);
        Debug.Log($"The line contains {OnLineCube.Count} Coordinates");

        List<Vector2Int> OnLine = new List<Vector2Int>();

        foreach(Vector3Int coord in OnLineCube)
        {
            Debug.Log($"Coordinate: {coord}");
            OnLine.Add(HexGridUtil.CubeToAxialCoord(coord));
        }

        Debug.Log(OnLine.Count);

        foreach (Vector2Int coord in OnLine)
        {
            Grid[coord].transform.position += Vector3.up;
        }


    }

    //private void OnValidate()
    //{
    //    if (Application.isPlaying)
    //    {
    //        foreach (GridTile cell in GetComponentsInChildren<GridTile>())
    //        {
    //            Destroy(cell.gameObject);
    //        }
    //        try
    //        {
    //            GenerateGrid();
    //        }
    //        catch
    //        {
    //            Debug.Log("I FAILED TO GENERATE GRID IN ONVALIDATE");
    //        }

    //    }
    //}

    public void GenerateGrid()
    {
        Grid = new Dictionary<Vector2Int, GridTile>();

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                GridTile tile = Instantiate(GridTilePrefab, GetPositionForHexFromCoordinate(new Vector2Int(x, y)), Quaternion.identity, transform);
                tile.Setup(new Vector2Int(x, y), (Ressource)Random.Range(0, 4), gridStates[Random.Range(0, gridStates.Count)]);
                tile.name = $"Hex{x},{y}";
                tile.innerSize = innerSize;
                tile.outerSize = outerSize;
                tile.height = height;
                tile.DrawMesh();
                Grid.Add(new Vector2Int(x, y), tile);
            }
        }
    }

    public Vector3 GetPositionForHexFromCoordinate(Vector2Int coordinate)
    {
        int column = coordinate.x;
        int row = coordinate.y;
        float width;
        float height;
        float xpos;
        float ypos;
        float offset;
        float horizontalDistance;
        float verticalDistance;
        float size = outerSize;

        width = 2f * size;
        height = Mathf.Sqrt(3f) * size;
        horizontalDistance = width * (3f / 4f);
        verticalDistance = height;

        offset = (column % 2 == 0) ? height / 2 : 0;

        xpos = column * horizontalDistance;
        ypos = row * verticalDistance - offset;

        return new Vector3(xpos, 0, ypos);
    }

    public GridTile PickRandomTile()
    {
        List<GridTile> tileCollection = new List<GridTile>();

        foreach(KeyValuePair<Vector2Int,GridTile> kvp in Grid)
        {
            tileCollection.Add(kvp.Value);
        }

        return tileCollection[Random.Range(0,tileCollection.Count)];
    }
}
