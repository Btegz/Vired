using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

/// <summary>
/// Singleton for the Grid
/// </summary>
public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [HideInInspector] public Vector2Int BossSpawn;

    // Vector2Int holds the coordinate and GridTile is the GameObject in the Coordinate.
    // It is a Vector2Int instead of Vector3Int because this makes accessing the tiles much easier.
    // If you want the cubic Coordinate you cann access HexGridUtil.AxialToCubeCoord.
    [SerializeField] public Dictionary<Vector2Int, GridTile> Grid;

    [Header("The Boring Stuff")]
    [SerializeField] GridTile GridTilePrefab;
    public GS_positive gS_Positive;
    public GS_neutral gS_Neutral;
    public GS_negative gS_Negative;
    public GS_Enemy gS_Enemy;
    public GS_Boss gS_Boss;
    public GS_BossNegative gS_BossNegative;
    public GS_Pofl gS_PofI;

    [Header("Map")]
    [SerializeField] public MapSettings mapSettings;

    //[Header("Tile Presets")]
    //[SerializeField, Tooltip("should be smaller then outerSize. If Hex should be filled this will be 0.")] float innerSize;
    //[SerializeField, Tooltip("The Size of a Hex tile. Size represents the radius and not the diameter of the Hex. If outerSize is 1, the Hex will have a width of 2.")] float outerSize;
    //[SerializeField, Tooltip("The y Position of the tile")] float height;
    //[SerializeField] List<GridTileSO> gridTileSOs;

    [Header("PofIs")]
    [SerializeField] public GameObject PofIPrefab;
    [SerializeField][HideInInspector] public GameObject pofi;
    private List<Vector2Int> PofIList = new List<Vector2Int>();
    private int randomPofI;
    public int pofIOffset;
    [SerializeField] int PofiSpawnCount;



    [Header("Enemies")]
    [SerializeField] public List<Enemy> StartEnemyPrefabs;
    [SerializeField] int startEnemyCount;
    [SerializeField] public Boss StartBossPrefab;


    [HideInInspector] public int TurnCounter;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            TransferGridSOData();
            //currentPhase = phases[0];
            //currentPhase.myPhaseTransition.InitPhaseTransitionCheck();
            //TriggerPhase();

        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.OnEndTurnEvent += EndTurn;
        
        PlayerManager.Instance.abilityLoadout.amountToChoose = 3;
        PlayerManager.Instance.abilityLoadout.gameObject.SetActive(true);

        

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

    private void OnDestroy()
    {
        EventManager.OnEndTurnEvent -= EndTurn;
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

            List<ProceduralTileInfo> tileInfos = mapSettings.NoiseData();


            foreach (ProceduralTileInfo tileinfo in tileInfos)
            {
                if (tileinfo.valid)
                {
                    GridTile newTile = Instantiate(GridTilePrefab);
                    //newTile.ChangeCurrentState(gS_Positive);
                    
                    newTile.Setup(tileinfo.coord, tileinfo.resource, true, tileinfo);
                    newTile.transform.parent = transform;
                    newTile.transform.position = HexGridUtil.AxialHexToPixel(tileinfo.coord, 1);

                    //newTile.transform.position += Vector3.up * (1 + tileinfo.noiseValue);

                    switch (tileinfo.resource)
                    {
                        case Ressource.ressourceA:
                            newTile.transform.position += Vector3.up * mapSettings.constantHeighVarianceFactor * 1;
                            break;
                        case Ressource.ressourceB:
                            newTile.transform.position += Vector3.up * mapSettings.constantHeighVarianceFactor * 2;
                            break;
                        case Ressource.ressourceC:
                            newTile.transform.position += Vector3.up * mapSettings.constantHeighVarianceFactor * 3;
                            break;
                        case Ressource.ressourceD:
                            newTile.transform.position += Vector3.up * mapSettings.constantHeighVarianceFactor * 4;
                            break;
                    }
                    Grid.Add(tileinfo.coord, newTile);
                }
            }            
            



            foreach (KeyValuePair<Vector2Int, GridTile> kvp in Grid)
            {
                kvp.Value.Triangluate();
                kvp.Value.UpdateMyNeighbors();
                //if (mapSettings != null)
                //{
                //    kvp.Value.pertulate(mapSettings);
                //}
            }

            Texture2D cellTexture = new Texture2D(200, 200, TextureFormat.RGBA32, false, true)
            {
                filterMode = FilterMode.Point,
                wrapModeU = TextureWrapMode.Repeat,
                wrapModeV = TextureWrapMode.Clamp
            };
            Shader.SetGlobalTexture("_HexCellData", cellTexture);


            Shader.SetGlobalVector(
            "_HexCellData_TexelSize",
            new Vector4(1f / 200, 1f / 200, 200, 200));

            // Fill Map with Things
            List<Vector2Int> border = HexGridUtil.GetOuterBorderUnSorted(Grid.Keys.ToList<Vector2Int>());
            SpawnBossAndPlayer(border);


            for (int i = 0; i < startEnemyCount; i++)
            {
                SpawnEnemy();
            }
            SpawnPofIs();
        }
    }


    private void SpawnEnemy()
    {
        Enemy enemy = Instantiate(StartEnemyPrefabs[Random.Range(0, StartEnemyPrefabs.Count)]);

        List<GridTile> reachableTiles = new List<GridTile>();

        List<Vector2Int> unerlaubteFelder = new List<Vector2Int>();
        foreach (Player p in PlayerManager.Instance.Players)
        {
            unerlaubteFelder.Add(p.CoordinatePosition);
        }
        unerlaubteFelder.Add(BossSpawn);
        foreach (Vector2Int coordinate in HexGridUtil.CoordinatesReachable(HexGridUtil.AxialToCubeCoord(BossSpawn), 3))
        {
            unerlaubteFelder.Add(coordinate);
        }


        reachableTiles = GridManager.Instance.GetTilesWithState(gS_Positive);

        GridTile targetLocation = reachableTiles[Random.Range(0, reachableTiles.Count)];
        while (unerlaubteFelder.Contains(targetLocation.AxialCoordinate))
        {
            targetLocation = reachableTiles[Random.Range(0, reachableTiles.Count)];
        }
        enemy.Setup(/*enemySOs[Random.Range(0, enemySOs.Count)], */targetLocation);
        targetLocation.ChangeCurrentState(gS_Enemy);
        enemy.transform.parent = targetLocation.transform;
        enemy.transform.position = targetLocation.transform.position;
    }
    private void SpawnBossAndPlayer(List<Vector2Int> Border)
    {
        //Vector3Int maxX = new Vector3Int();
        //Vector3Int maxY = new Vector3Int();
        //Vector3Int maxZ = new Vector3Int();
        //Vector3Int minX = new Vector3Int();
        //Vector3Int minY = new Vector3Int();
        //Vector3Int minZ = new Vector3Int();

        //List<Vector3Int> CubeCoords = HexGridUtil.AxialToCubeCoord(Grid.Keys.ToList<Vector2Int>());

        //foreach (Vector3Int coord in CubeCoords)
        //{
        //    if (coord.x > maxX.x)
        //    {
        //        maxX = coord;
        //    }

        //    if (coord.x < minX.x)
        //    {
        //        minX = coord;
        //    }

        //    if (coord.y > maxY.y)
        //    {
        //        maxY = coord;
        //    }

        //    if (coord.y < minY.y)
        //    {
        //        minY = coord;
        //    }

        //    if (coord.z > maxZ.z)
        //    {
        //        maxZ = coord;
        //    }

        //    if ((coord.z < minZ.z))
        //    {
        //        minZ = coord;
        //    }
        //}

        //Vector3Int[] coords = { minY, minX, minZ };
        //random = Random.Range(0, coords.Length);
        //BossSpawn = Border[0];
        List<Player> players = PlayerManager.Instance.Players;
        players[0].SpawnPoint = Border[Border.Count / 4 * 1];
        players[1].SpawnPoint = Border[Border.Count / 4 * 2];
        players[2].SpawnPoint = Border[Border.Count / 4 * 3];

        players[0].CoordinatePosition = Border[Border.Count / 4 * 1];
        players[1].CoordinatePosition = Border[Border.Count / 4 * 2];
        players[2].CoordinatePosition = Border[Border.Count / 4 * 3];

        Boss newBoss = Instantiate(StartBossPrefab);
        List<Vector2Int> newBossTiles = HexGridUtil.AxialNeighbors(Border[0]);
        BossSpawn = Border[0];
        //foreach (Vector2Int coordinate in newBossTiles)
        //{
        //    if (!Grid.ContainsKey(coordinate))
        //    {
        //        GridTile newTile = Instantiate(GridTilePrefab);
        //        newTile.Setup(coordinate, (Ressource)Random.Range(0, 4), true);
        //        newTile.transform.parent = transform;
        //        newTile.transform.position = HexGridUtil.AxialHexToPixel(coordinate, 1);
        //        newTile.currentGridState = gS_BossNegative;
        //        Grid.Add(coordinate, newTile);
        //    }
        //}
        newBoss.Setup(Grid[Border[0]]);


    }


    private void SpawnPofIs()
    {
        List<Vector2Int> possibleTiles = new List<Vector2Int>();

        foreach (KeyValuePair<Vector2Int, GridTile> kvp in Grid)

        {
            if (kvp.Value.currentGridState == gS_Positive && kvp.Key != PlayerManager.Instance.Players[0].CoordinatePosition && kvp.Key != PlayerManager.Instance.Players[1].CoordinatePosition && kvp.Key != PlayerManager.Instance.Players[2].CoordinatePosition)
            {
                if (kvp.Value.currentGridState != gS_BossNegative && kvp.Value.currentGridState != gS_Negative && kvp.Value.currentGridState != gS_Enemy && kvp.Value.currentGridState != gS_Boss)
                    possibleTiles.Add(kvp.Key);
            }
        }
        for (int i = 0; i < PofiSpawnCount; i++)
        {
            randomPofI = Random.Range(0, possibleTiles.Count);
            Grid[possibleTiles[randomPofI]].ChangeCurrentState(gS_PofI);
            PofIList.Add(possibleTiles[randomPofI]);
        }
        foreach (Vector2Int pofI in PofIList)
        {
            GridTile targetLocation = GridManager.Instance.Grid[pofI];
            pofi = Instantiate(PofIPrefab);
            pofi.transform.parent = targetLocation.transform;
            pofi.transform.position = new Vector3(targetLocation.transform.position.x, pofIOffset, targetLocation.transform.position.z);


        }
    }


    /// <summary>
    /// Generates a Grid
    /// </summary>
    //public void GenerateGrid()
    //{
    //    Grid = new Dictionary<Vector2Int, GridTile>();

    //    // a Standard Hexgonal Grid for the Start
    //    List<Vector2Int> coords = HexGridUtil.GenerateHexagonalShapedGrid(1);

    //    // The Startgrid is Instantiated and filled with random States
    //    foreach (Vector2Int coord in coords)
    //    {
    //        GridTile tile = Instantiate(GridTilePrefab, HexGridUtil.AxialHexToPixel(coord, outerSize), Quaternion.identity, transform);
    //        tile.Setup(coord, gridTileSOs[Random.Range(0, gridTileSOs.Count)], gS_Enemy,true);
    //        tile.name = $"Hex{coord.x},{coord.y}";
    //        tile.innerSize = innerSize;
    //        tile.outerSize = outerSize;
    //        tile.height = height;
    //        tile.DrawMesh();
    //        Grid.Add(coord, tile);
    //    }


    //}

    public void EndTurn()
    {
        TurnCounter+=1;
       

    }

    //public void TriggerPhase()
    //{
    //    currentPhase.TriggerPhaseEffects(TurnCounter, this);
    //}


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

    //public void PhaseTransition()
    //{
    //    phases.RemoveAt(0);
    //    if (phases.Count <= 0)
    //    {
    //        //GameWon();
    //        return;
    //    }
    //    currentPhase = phases[0];
    //    currentPhase.myPhaseTransition.InitPhaseTransitionCheck();
    //    currentPhase.TriggerPhaseEffects(TurnCounter, this);
    //}
}
