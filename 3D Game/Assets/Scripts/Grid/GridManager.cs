using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
    [SerializeField] public Dictionary<Vector2Int, GridTile> Grid;

    // Supposed to handle the q and r dimensions of the grid when gereating the shape.
    public GS_positive gS_Positive;
    public GS_neutral gS_Neutral;
    public GS_negative gS_Negative;
    public GS_Enemy gS_Enemy;
    public GS_Boss gS_Boss;
    public GS_BossNegative gS_BossNegative;
    public GS_Pofl gS_PofI;
    
    [SerializeField] public Boss boss;
    [SerializeField] public Enemy Boss;
    [SerializeField] public Enemy Boss1Phase2;
    [SerializeField] public Enemy Boss2Phase2;
    [SerializeField] public Enemy Boss3Phase2;

    public Vector2Int BossSpawn;
    public int random;
    public new Vector2Int[] coords = new Vector2Int[3];

    [Header("Map")]
    [SerializeField] MapSettings mapSettings;

    [Header("Tile Presets")]
    [SerializeField, Tooltip("should be smaller then outerSize. If Hex should be filled this will be 0.")] float innerSize;
    [SerializeField, Tooltip("The Size of a Hex tile. Size represents the radius and not the diameter of the Hex. If outerSize is 1, the Hex will have a width of 2.")] float outerSize;
    [SerializeField, Tooltip("The y Position of the tile")] float height;
    [SerializeField] List<GridTileSO> gridTileSOs;

    [Header("Shape Presets")]
    [SerializeField] List<HS_World> RessourceShapes;

    [Header("Enemy Ressources")]
    [SerializeField] public Enemy enemyPrefab;
    [SerializeField] public List<EnemySO> enemySOs;
    [SerializeField] public EnemySO BossEnemySO;
    [SerializeField] public Enemy BossPrefab;

    [Header("Phases")]
    [SerializeField] public int TurnCounter;
    [SerializeField] Phase currentPhase;
    [SerializeField] List<Phase> phases;

    [Header("UI")]
    [SerializeField] public Canvas mainCanvas;


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
        currentPhase = phases[0];
        currentPhase.myPhaseTransition.InitPhaseTransitionCheck();
        TriggerPhase();
        EventManager.OnEndTurnEvent += EndTurn;
        TransferGridSOData();

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

        //List<Vector3Int> path = HexGridUtil.BreadthFIrstPathfinding(new Vector3Int(-2, -4, 6), new Vector3Int(0, -2,2),HexGridUtil.AxialToCubeCoord(Grid.Keys.ToList<Vector2Int>()));
        //foreach(Vector3Int p in path)
        //{
        //    Vector2Int pp = HexGridUtil.CubeToAxialCoord(p);

        //    Grid[pp].gameObject.transform.position += Vector3.up * 3;
        //}

    }


    public void GameWon()
    {
        SceneManager.LoadScene("GameWonScene");
    }

    public void TransferGridSOData()
    {


        if (mapSettings == null)
        {
            Grid = new Dictionary<Vector2Int, GridTile>();
            GridTile[] gridTiles = GetComponentsInChildren<GridTile>();
            foreach (GridTile tile in gridTiles)
            {
                Grid.Add(tile.AxialCoordinate, tile);
            }
        }
        else
        {
            GridTile[] gridTiles = GetComponentsInChildren<GridTile>();
            foreach (GridTile tile in gridTiles)
            {
                Destroy(tile.gameObject);
            }

            Grid = new Dictionary<Vector2Int, GridTile>();

            Dictionary<Vector2Int, Ressource> coordRessourceDict = new Dictionary<Vector2Int, Ressource>();
            Dictionary<Vector2Int, float> mapNoise = mapSettings.NoiseData(mapSettings.M_NoiseType1, mapSettings.M_Frequency);
            Dictionary<Vector2Int, float> gridNoise1 = mapSettings.NoiseData(mapSettings.NoiseType1, mapSettings.Frequency);
            Dictionary<Vector2Int, float> gridNoise2 = mapSettings.NoiseData(mapSettings.NoiseType2, mapSettings.Frequency);

            foreach (KeyValuePair<Vector2Int, float> kvp in gridNoise1)
            {
                float noise1 = kvp.Value;
                float noise2 = gridNoise2[kvp.Key];
                float mapNoise1 = mapNoise[kvp.Key];

                if (/*noise1 > mapSettings.NoiseThresholds.y && noise2 > mapSettings.NoiseThresholds.y*/mapNoise1 > mapSettings.M_NoiseThresholds.y)
                {
                    continue;
                }
                else if (/*noise1 < mapSettings.NoiseThresholds.x && noise2 < mapSettings.NoiseThresholds.x*/mapNoise1 < mapSettings.M_NoiseThresholds.x)
                {
                    continue;
                }
                Vector2Int coordinate = kvp.Key;
                float distance = HexGridUtil.CubeDistance(HexGridUtil.AxialToCubeCoord(coordinate), Vector3Int.zero);

                if (/*(Mathf.Abs(noise1) + Mathf.Abs(noise2))*/Mathf.Abs(mapNoise1) * distance >= mapSettings.M_DistanceThreshold)
                {
                    continue;
                }

                //Debug.Log($"noise1: {noise1}, noise2: {noise2} at {coordinate}");
                Ressource res;
                if (noise1 > 0f && noise2 >= 0f)
                {
                    res = Ressource.ressourceA;
                    // ressource a
                }
                else if (noise1 > 0f && noise2 < 0f)
                {
                    res = Ressource.ressourceB;
                    // ressource b
                }
                else if (noise1 <= 0f && noise2 >= 0f)
                {
                    res = Ressource.ressourceC;
                    // ressource c
                }
                else if (noise1 <= 0f && noise2 < 0f)
                {
                    res = Ressource.resscoureD;
                    //ressource d
                }
                else
                {
                    res = Ressource.ressourceA;
                }
                coordRessourceDict.Add(coordinate, res);
            }

            List<Vector2Int> reachableCoords = HexGridUtil.CubeToAxialCoord(HexGridUtil.CoordinatesReachable(Vector3Int.zero, mapSettings.NoiseDataSize.x, HexGridUtil.AxialToCubeCoord(coordRessourceDict.Keys.ToList<Vector2Int>())));

            foreach (Vector2Int coordinates in reachableCoords)
            {
                GridTile newTile = Instantiate(GridTilePrefab);
                newTile.Setup(coordinates, coordRessourceDict[coordinates]);
                newTile.transform.parent = transform;
                newTile.transform.position = HexGridUtil.AxialHexToPixel(coordinates, 1);

                Grid.Add(coordinates, newTile);
            }      
        }
       SpawnBossAndPlayer();
     //   boss.BossNeighbors();
        SpawnPofIs();
   
    }

    private void SpawnBossAndPlayer()
    {
        Vector3Int maxX = new Vector3Int();
        Vector3Int maxY = new Vector3Int();
        Vector3Int maxZ = new Vector3Int();
        Vector3Int minX = new Vector3Int();
        Vector3Int minY = new Vector3Int();
        Vector3Int minZ = new Vector3Int();

        List<Vector3Int> CubeCoords = HexGridUtil.AxialToCubeCoord(Grid.Keys.ToList<Vector2Int>());

        foreach (Vector3Int coord in CubeCoords)
        {
            if (coord.x > maxX.x)
            {
                maxX = coord;
            }

            if (coord.x < minX.x)
            {
                minX = coord;
            }

            if (coord.y > maxY.y)
            {
                maxY = coord;
            }

            if (coord.y < minY.y)
            {
                minY = coord;
            }

            if (coord.z > maxZ.z)
            {
                maxZ = coord;
            }

            if ((coord.z < minZ.z))
            {
                minZ = coord;
            }
        }
        
        Vector3Int[] coords = { minY, minX, minZ };
        random = Random.Range(0, coords.Length);
        BossSpawn = HexGridUtil.CubeToAxialCoord(coords[random]);
        Boss = Instantiate(BossPrefab);
      
        Boss.Setup(BossEnemySO, Grid[BossSpawn]);
       
        Boss.transform.parent = Grid[BossSpawn].transform;
        Boss.transform.position = Grid[BossSpawn].transform.position;
        Grid[BossSpawn].ChangeCurrentState(gS_Boss);

        List<Player> players = PlayerManager.Instance.Players;

        players[0].SpawnPoint = HexGridUtil.CubeToAxialCoord(maxX);
        players[1].SpawnPoint = HexGridUtil.CubeToAxialCoord(maxY);
        players[2].SpawnPoint = HexGridUtil.CubeToAxialCoord(maxZ);
    }

    
    private void SpawnPofIs()
    {
        List<Vector2Int> possibleTiles = new List<Vector2Int>();

        foreach (KeyValuePair<Vector2Int, GridTile> kvp in Grid)

        {
            if (kvp.Value.currentGridState == gS_Positive && kvp.Key !=PlayerManager.Instance.Players[0].CoordinatePosition && kvp.Key != PlayerManager.Instance.Players[1].CoordinatePosition && kvp.Key != PlayerManager.Instance.Players[2].CoordinatePosition) 
            {
                possibleTiles.Add(kvp.Key);
            }
        }

        for (int i = 0; i < 4; i++)
        {
             Grid[possibleTiles[Random.Range(0, possibleTiles.Count)]].ChangeCurrentState(gS_PofI);
        }
    }

    /// <summary>
    /// Generates a Grid
    /// </summary>
    public void GenerateGrid()
    {
        Grid = new Dictionary<Vector2Int, GridTile>();

        // a Standard Hexgonal Grid for the Start
        List<Vector2Int> coords = HexGridUtil.GenerateHexagonalShapedGrid(1);

        // The Startgrid is Instantiated and filled with random States
        foreach (Vector2Int coord in coords)
        {
            GridTile tile = Instantiate(GridTilePrefab, HexGridUtil.AxialHexToPixel(coord, outerSize), Quaternion.identity, transform);
            tile.Setup(coord, gridTileSOs[Random.Range(0, gridTileSOs.Count)], gS_Enemy);
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
                shape = HexGridUtil.CubeToAxialCoord(HexGridUtil.RotateRangeClockwise(Vector3Int.zero, HexGridUtil.AxialToCubeCoord(shape), Random.Range(0, 6)));
                coords = HexGridUtil.CombineGridsAlongAxis(coords, shape, HexGridUtil.cubeDirectionVectors[Random.Range(0, HexGridUtil.cubeDirectionVectors.Length)], out shape);
                foreach (Vector2Int coord in shape)
                {
                    GridTile tile = Instantiate(GridTilePrefab, HexGridUtil.AxialHexToPixel(coord, outerSize), Quaternion.identity, transform);
                    tile.Setup(coord, hsw.MyGridTileSO, hsw.MyGridTileSO.initialGridState);
                    tile.name = $"{hsw.MyGridTileSO.ressource} Hex ({coord.x},{coord.y})";
                    tile.innerSize = innerSize;
                    tile.outerSize = outerSize;
                    tile.height = height;
                    tile.DrawMesh();
                    Grid.Add(coord, tile);
                }
                coords.AddRange(shape);
            }
        }
    }

    public void EndTurn()
    {
        TurnCounter++;
    }

    public void TriggerPhase()
    {
        currentPhase.TriggerPhaseEffects(TurnCounter, this);
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

    public List<GridTile> GetTilesWithState(GridState state)
    {
        List<GridTile> enemies = new List<GridTile>();
        foreach (KeyValuePair<Vector2Int, GridTile> kvp in Grid)
        {
            if (kvp.Value.currentGridState.StateValue() == state.StateValue())
            {
                enemies.Add(kvp.Value);
            }
        }
        return enemies;
    }

    public void PhaseTransition()
    {
        phases.RemoveAt(0);
        if (phases.Count <= 0)
        {
            GameWon();
            return;
        }
        currentPhase = phases[0];
        currentPhase.myPhaseTransition.InitPhaseTransitionCheck();
        currentPhase.TriggerPhaseEffects(TurnCounter,this);
    }
}
