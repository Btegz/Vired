using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

/// <summary>
/// Singleton for the Grid
/// </summary>
public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField] GridTile GridTilePrefab;

    // Vector2Int holds the coordinate and GridTile is the GameObject in the Coordinate.
    // It is a Vector2Int instead of Vector3Int because this makes accessing the tiles much easier.
    // If you want the cubic Coordinate you cann access HexGridUtil.AxialToCubeCoord.
    public Dictionary<Vector2Int,GridTile> Grid;

    // Supposed to handle the q and r dimensions of the grid when gereating the shape.
    [SerializeField] Vector2Int gridSize; 
    List<GridState> gridStates;

    [Header("Tile Presets")]
    [SerializeField, Tooltip("should be smaller then outerSize. If Hex should be filled this will be 0.")] float innerSize;
    [SerializeField, Tooltip("The Size of a Hex tile. Size represents the radius and not the diameter of the Hex. If outerSize is 1, the Hex will have a width of 2.")] float outerSize;
    [SerializeField, Tooltip("The y Position of the tile")] float height;

    [Header("Resource Materials")]
    public Material resourceAMaterial;
    public Material resourceBMaterial;
    public Material resourceCMaterial;
    public Material resourceDMaterial;
    public Material neutralMaterial;
    public Material negativeMaterial;

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

        // Das ist nur spielerei.
        GridTile tileA = PickRandomTile();
        GridTile tileB = PickRandomTile();

        Vector3Int coordTileA = HexGridUtil.AxialToCubeCoord(tileA.AxialCoordinate);
        Vector3Int coordTileB = HexGridUtil.AxialToCubeCoord(tileB.AxialCoordinate);

        int distance = HexGridUtil.CubeDistance(HexGridUtil.AxialToCubeCoord(tileA.AxialCoordinate), HexGridUtil.AxialToCubeCoord(tileB.AxialCoordinate));

        List<Vector3Int> OnLineCube = HexGridUtil.CubeLineDraw(coordTileA,coordTileB);

        List<Vector2Int> OnLine = new List<Vector2Int>();

        foreach(Vector3Int coord in OnLineCube)
        {
            OnLine.Add(HexGridUtil.CubeToAxialCoord(coord));
        }

        foreach (Vector2Int coord in OnLine)
        {
            Grid[coord].transform.position += Vector3.up;
        }
    }

    /// <summary>
    /// Will need to delete soon. It Generates another Grid whenever something chhanges in Playmode.
    /// </summary>
    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            foreach (GridTile cell in GetComponentsInChildren<GridTile>())
            {
                Destroy(cell.gameObject);
            }
            try
            {
                GenerateGrid();
            }
            catch
            {
                foreach (GridTile cell in GetComponentsInChildren<GridTile>())
                {
                    Destroy(cell.gameObject);
                }
            }

        }
    }

    /// <summary>
    /// Generates a Grid
    /// </summary>
    public void GenerateGrid()
    {
        Grid = new Dictionary<Vector2Int, GridTile>();

        //List<Vector2Int> coords = HexGridUtil.GenerateRombusShapedGrid(gridSize.x, gridSize.y);
        //List<Vector2Int> coords = HexGridUtil.GenerateRectangleShapedGrid(gridSize.x, gridSize.y);
        List<Vector2Int> coords = HexGridUtil.GenerateHexagonalShapedGrid(gridSize.x);

        foreach (Vector2Int coord in coords)
        {
            GridTile tile = Instantiate(GridTilePrefab, HexGridUtil.AxialHexToPixel(coord, outerSize), Quaternion.identity, transform);
            tile.Setup(coord, (Ressource)Random.Range(0, 4), gridStates[Random.Range(0, gridStates.Count)]);
            tile.name = $"Hex{coord.x},{coord.y}";
            tile.innerSize = innerSize;
            tile.outerSize = outerSize;
            tile.height = height;
            tile.DrawMesh();
            Grid.Add(coord, tile);
        }
    }


    /// <summary>
    /// Picks a random tile from the Grid
    /// </summary>
    /// <returns>random Tile from grid.</returns>
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
