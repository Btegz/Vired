using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PE_MiniEnemySpawn : PhaseEffect
{
    [SerializeField] public List<Spreadbehaviours> SpreadBehaviours;
    [SerializeField] private List<EnemySO> enemySOs;
    [SerializeField] private List<Vector3Int> Zerklings;
    [SerializeField] private List<Vector3Int> ZerklingNeighbor;
    [SerializeField] private List<Vector3Int> possibleTiles;
    [SerializeField] private List<Vector3Int> possibleNeighbors;
    [SerializeField] private List<Vector3Int> ZerklingSpawn;
    public List<EnemySO> myEnemySoList
    {
        get { return enemySOs; }
        set { enemySOs = value; }
    }
    [SerializeField] private Enemy enemyPrefab;
    public Enemy myEnemyPrefab
    {
        get { return enemyPrefab; }
        set { enemyPrefab = value; }
    }
    [SerializeField] private int amount;
    public int MyAmount
    {
        get { return amount; }
        set { amount = value; }
    }
    public override void TriggerPhaseEffect(int turnCounter, GridManager gridManager)
    {
        if (turnCounter % everyXRounds == 0)
        {
            for (int i = 0; i < SpreadBehaviours.Count; i++)
            {
                Vector3Int enemyPosition;
                Vector3Int target;
                if (SpreadBehaviours[i].TargetTile(Vector3Int.zero, out target, PlayerManager.Instance.playerPosition))
                {
                    enemyPosition = target;
                    Enemy enemy1 = Instantiate(enemyPrefab);
                    GridTile targetLocation = GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(target)];
                    enemy1.Setup(/*enemySOs[Random.Range(0, enemySOs.Count)], */targetLocation);
                    enemy1.currentHealth = 1;
                    targetLocation.ChangeCurrentState(gridManager.gS_Enemy);
                    enemy1.transform.parent = targetLocation.transform;
                    enemy1.transform.position = targetLocation.transform.position;

                    
                    Zerklings = HexGridUtil.CubeNeighbors(target);
                    possibleTiles = new List<Vector3Int>();
                    foreach (Vector3Int zerkling in Zerklings)
                    {
                        if (GridManager.Instance.Grid.ContainsKey(HexGridUtil.CubeToAxialCoord(zerkling)))
                        {
                            if (GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(zerkling)].currentGridState == GridManager.Instance.gS_Positive && GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(zerkling)].AxialCoordinate != PlayerManager.Instance.playerPosition)
                            {
                                possibleTiles.Add(zerkling);
                            }
                        }
                    }
                    Vector3Int neighbor = possibleTiles[Random.RandomRange(0, possibleTiles.Count)];
                    ZerklingNeighbor = new List<Vector3Int>();
                    ZerklingNeighbor = HexGridUtil.CubeNeighbors(neighbor);

                    possibleNeighbors = new List<Vector3Int>();
                    foreach (Vector3Int neighbour in ZerklingNeighbor)
                    {
                        if (GridManager.Instance.Grid.ContainsKey(HexGridUtil.CubeToAxialCoord(neighbour)))
                        {
                            if (GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(neighbour)].currentGridState == GridManager.Instance.gS_Positive && GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(neighbour)].AxialCoordinate != PlayerManager.Instance.playerPosition)
                            {
                                if (neighbour != target)

                                    possibleNeighbors.Add(neighbour);

                            }
                        }
                    }
                    Vector3Int neighboringNeighbor = possibleNeighbors[Random.Range(0, possibleNeighbors.Count)];

                    Enemy enemy2 = Instantiate(enemyPrefab);
                    GridTile targetlocato = GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(neighboringNeighbor)];
                    enemy2.Setup(/*enemySOs[Random.Range(0, enemySOs.Count)], */targetlocato);
                    enemy2.currentHealth = 1;
                    targetlocato.ChangeCurrentState(gridManager.gS_Enemy);
                    enemy2.transform.parent = targetlocato.transform;
                    enemy2.transform.position = targetlocato.transform.position;

                    Enemy enemy3 = Instantiate(enemyPrefab);
                    GridTile targetlocatio = GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(neighbor)];
                    enemy3.Setup(/*enemySOs[Random.Range(0, enemySOs.Count)], */targetlocatio);
                    enemy3.currentHealth = 1;
                    targetlocatio.ChangeCurrentState(gridManager.gS_Enemy);
                    enemy3.transform.parent = targetlocatio.transform;
                    enemy3.transform.position = targetlocatio.transform.position;
                    

                    

                }


            }
        }
    }
}