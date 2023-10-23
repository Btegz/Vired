using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Singleton for the Grid
/// </summary>
public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField] GridTile GridTilePrefab;

    [SerializeField] public GridScriptableObject gridSO;

    // Vector2Int holds the coordinate and GridTile is the GameObject in the Coordinate.
    // It is a Vector2Int instead of Vector3Int because this makes accessing the tiles much easier.
    // If you want the cubic Coordinate you cann access HexGridUtil.AxialToCubeCoord.
    public Dictionary<Vector2Int, GridTile> Grid;

    // Supposed to handle the q and r dimensions of the grid when gereating the shape.
    [SerializeField] Vector2Int gridSize;
    public Dictionary<string,GridState> gridStates;

    [Header("Tile Presets")]
    [SerializeField, Tooltip("should be smaller then outerSize. If Hex should be filled this will be 0.")] float innerSize;
    [SerializeField, Tooltip("The Size of a Hex tile. Size represents the radius and not the diameter of the Hex. If outerSize is 1, the Hex will have a width of 2.")] float outerSize;
    [SerializeField, Tooltip("The y Position of the tile")] float height;

    [Header("Shape Presets")]
    [SerializeField] List<HS_World> RessourceShapes;

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

    [Header("Phases")]
    [SerializeField] public int TurnCounter;
    [SerializeField] Phase currentPhase;
    [SerializeField] List<Phase> phases;

    [Header("UI")]
    [SerializeField] Image negativeBarFill;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        TransferGridSOData(); 
        List<GridTile> negativeTiles = GetTilesWithState("negative");
        negativeTiles.AddRange(GetTilesWithState("enemy"));
        negativeTiles.AddRange(GetTilesWithState("boss"));
        if (negativeTiles.Count > 0)
        {
            negativeBarFill.fillAmount = negativeTiles.Count / Grid.Count;
        }
        else
        {
            negativeBarFill.fillAmount = 0;
        }
        
        //GenerateGrid();

        // Das ist nur spielerei.
        //GridTile tileA = PickRandomTile();
        //GridTile tileB = PickRandomTile();

        //Vector3Int coordTileA = HexGridUtil.AxialToCubeCoord(tileA.AxialCoordinate);
        //Vector3Int coordTileB = HexGridUtil.AxialToCubeCoord(tileB.AxialCoordinate);

        //int distance = HexGridUtil.CubeDistance(HexGridUtil.AxialToCubeCoord(tileA.AxialCoordinate), HexGridUtil.AxialToCubeCoord(tileB.AxialCoordinate));

        //List<Vector3Int> OnLineCube = HexGridUtil.CubeLineDraw(coordTileA,coordTileB);

        //List<Vector2Int> OnLine = new List<Vector2Int>();

        //foreach(Vector3Int coord in OnLineCube)
        //{
        //    OnLine.Add(HexGridUtil.CubeToAxialCoord(coord));
        //}

        //foreach (Vector2Int coord in OnLine)
        //{
        //    Grid[coord].transform.position += Vector3.up;
        //}
    }

    private void Update()
    {

    }

    /// <summary>
    /// Will need to delete soon. It Generates another Grid whenever something chhanges in Playmode.
    /// </summary>
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
    //            foreach (GridTile cell in GetComponentsInChildren<GridTile>())
    //            {
    //                Destroy(cell.gameObject);
    //            }
    //        }

    //    }
    //}

    public void TransferGridSOData()
    {
        Grid = gridSO.Grid;
    }

    /// <summary>
    /// Generates a Grid
    /// </summary>
    public void GenerateGrid()
    {
        gridStates = new Dictionary<string, GridState>();
        gridStates.Add("positive", new GS_positive());
        gridStates.Add("neutral", new GS_neutral());
        gridStates.Add("negative", new GS_negative());
        gridStates.Add("enemy", new GS_Enemy());
        gridStates.Add("boss", new GS_Boss());


        Grid = new Dictionary<Vector2Int, GridTile>();

        // a Standard Hexgonal Grid for the Start
        List<Vector2Int> coords = HexGridUtil.GenerateHexagonalShapedGrid(1);

        // The Startgrid is Instantiated and filled with random States
        foreach (Vector2Int coord in coords)
        {
            GridTile tile = Instantiate(GridTilePrefab, HexGridUtil.AxialHexToPixel(coord, outerSize), Quaternion.identity, transform);
            tile.Setup(coord, (Ressource)Random.Range(0, 4), gridStates["enemy"]);
            tile.name = $"Hex{coord.x},{coord.y}";
            tile.innerSize = innerSize;
            tile.outerSize = outerSize;
            tile.height = height;
            tile.DrawMesh();
            Grid.Add(coord, tile);
        }

        // Every RessourceShape is Generated and combined with the basegrid.
        for (int i = 0; i < 3; i++)
        {
            foreach (HS_World hsw in RessourceShapes)
            {
                List<Vector2Int> shape = hsw.Coordinates;
                shape = HexGridUtil.CubeToAxialCoord(HexGridUtil.RotateRangeClockwise(Vector3Int.zero, HexGridUtil.AxialToCubeCoord(shape), Random.Range(0,6)));
                coords = HexGridUtil.CombineGridsAlongAxis(coords, shape, HexGridUtil.cubeDirectionVectors[Random.Range(0, HexGridUtil.cubeDirectionVectors.Length)], out shape);
                foreach (Vector2Int coord in shape)
                {
                    GridTile tile = Instantiate(GridTilePrefab, HexGridUtil.AxialHexToPixel(coord, outerSize), Quaternion.identity, transform);
                    tile.Setup(coord, hsw.MyRessource, gridStates["positive"]);
                    tile.name = $"{hsw.MyRessource} Hex ({coord.x},{coord.y})";
                    tile.innerSize = innerSize;
                    tile.outerSize = outerSize;
                    tile.height = height;
                    tile.DrawMesh();
                    Grid.Add(coord, tile);
                }
                coords.AddRange(shape);
            }
        }

        //List<Vector2Int> gridB = HexGridUtil.GenerateRombusShapedGrid(2, 2);
        //List<Vector2Int> baseGrid = HexGridUtil.GenerateHexagonalShapedGrid(1);

        //Vector3Int[] diagonalDirections = HexGridUtil.cubeDiagonalDirectionVectors;
        //Vector3Int[] neighborDirections = HexGridUtil.cubeDirectionVectors;

        //List<Vector2Int> coords = baseGrid;

        //for (int i = 0; i < neighborDirections.Length; i+=1)
        //{
        //    coords = HexGridUtil.CombineGridsAlongAxis(coords, gridB, neighborDirections[i]);
        //    gridB = HexGridUtil.CubeToAxialCoord(HexGridUtil.RotateRangeCounterClockwise(Vector3Int.zero, HexGridUtil.AxialToCubeCoord(gridB)));
        //}
        //gridB = HexGridUtil.CubeToAxialCoord(HexGridUtil.RotateRangeCounterClockwise(Vector3Int.zero, HexGridUtil.AxialToCubeCoord(gridB)));
        //for (int i=0; i<diagonalDirections.Length; i++)
        //{
        //    coords = HexGridUtil.CombineGridsAlongAxis(coords, gridB, diagonalDirections[i]);
        //    gridB = HexGridUtil.CubeToAxialCoord(HexGridUtil.RotateRangeCounterClockwise(Vector3Int.zero, HexGridUtil.AxialToCubeCoord(gridB)));
        //}
    }

    public void TriggerPhase()
    {
        currentPhase.TriggerPhaseEffects(TurnCounter,this);
    }


    /// <summary>
    /// Picks a random tile from the Grid
    /// </summary>
    /// <returns>random Tile from grid.</returns>
    public GridTile PickRandomTile()
    {
        List<GridTile> tileCollection = new List<GridTile>();

        foreach (KeyValuePair<Vector2Int, GridTile> kvp in Grid)
        {
            tileCollection.Add(kvp.Value);
        }

        return tileCollection[Random.Range(0, tileCollection.Count)];
    }

    public List<GridTile> GetTilesWithState(string state)
    {
        List<GridTile> enemies = new List<GridTile>();
        foreach(KeyValuePair<Vector2Int,GridTile> kvp in Grid)
        {
            if(kvp.Value.currentGridState.Equals(gridStates[state]))
            {
                enemies.Add(kvp.Value);
            }
        }
        return enemies;
    }
}
