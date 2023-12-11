using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class Boss : Enemy
{
    [SerializeField] List<Vector3Int> BossReachableTiles;
    [SerializeField] List<Vector3Int> BossTiles;
    [SerializeField] public List<Spreadbehaviours> BossSpreads;

    //[SerializeField] List<GridTile> GridEnemies;   
    //[SerializeField] List<Vector3Int> ReachableTiles;
    //public int everyXRounds;
    //public int turnCounter;

    public List<Vector2Int> location;


    [SerializeField] int SpawnRange;


    [SerializeField] List<Enemy> enemyPrefabPool;
    [SerializeField] int EnemySpawnAmount;
    [SerializeField] int everyXTurns;


    [SerializeField] List<Boss> NextBosses;
    [SerializeField] Spreadbehaviours nextBossSpawnPattern;
    [SerializeField] int AbilityLoadoutReward;


    public GameObject Enemy2Prefab;

    public GameObject blueParticle;
    public GameObject orangeParticle;
    public GameObject redParticle;
    public GameObject greenParticle;

    private void Start()
    {
        EventManager.OnEndTurnEvent += BossNeighbors;
        EventManager.OnEndTurnEvent += Spread;
    }

    public override void Setup(GridTile tile)
    {
        base.Setup(tile);
        tile.ChangeCurrentState(GridManager.Instance.gS_Boss);
        BossNeighbors();
        //EventManager.OnEndTurnEvent += BossNeighbors;
    }

    override public void Spread()
    {
        if (GridManager.Instance.TurnCounter % everyXTurns != 0)
        {
            return;
        }

        foreach (Spreadbehaviours sb in spreadbehaviours)
        {
            for (int i = 0; i < EnemySpawnAmount; i++)
            {
                if (sb.TargetTile(HexGridUtil.AxialToCubeCoord(axialLocation), out Vector3Int target, FindClosestPlayer().CoordinatePosition))
                {
                    Enemy newEnemy = Instantiate(enemyPrefabPool[Random.Range(0,enemyPrefabPool.Count)], GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(target)].transform);
                    newEnemy.Setup(GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(target)]);
                }
                else
                {
                    break;
                }
            }
        }
    }

    public override void Death()
    {
        if(NextBosses == null || NextBosses.Count==0)
        {
            GridManager.Instance.GameWon();
        }

        PlayerManager.Instance.abilityLoadout.amountToChoose = AbilityLoadoutReward;
        PlayerManager.Instance.abilityLoadout.gameObject.SetActive(true);

        foreach (Vector3Int neighbor in BossReachableTiles)
        {
            GridManager.Instance.Grid[axialLocation].ChangeCurrentState(GridManager.Instance.gS_Positive);
            if (GridManager.Instance.Grid.ContainsKey(HexGridUtil.CubeToAxialCoord(neighbor)))
            {
                GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(neighbor)].ChangeCurrentState(GridManager.Instance.gS_Positive);
            }
        }

        if (GridManager.Instance.transform.GetComponentsInChildren<Boss>().Length > 1)
        {
            base.Death();
            return;
        }

        //    foreach (KeyValuePair<Vector2Int, GridTile> kvp in GridManager.Instance.Grid)
        //{
        //    if(kvp.Value.gameObject.GetComponentsInChildren<Boss>().Length >0)
        //    {
                
        //    }
        //}

        foreach(Boss b in NextBosses)
        {
            if(nextBossSpawnPattern == null)
            {
                Boss newBoss = Instantiate(b);
                newBoss.Setup(GridManager.Instance.Grid[Vector2Int.zero]);
                break;
            }
            if(nextBossSpawnPattern.TargetTiles(Vector3Int.zero,out List<Vector3Int> targets, Vector2Int.zero))
            {
                Boss newBoss = Instantiate(b);
                newBoss.Setup(GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(targets[Random.Range(0,targets.Count)])]);
            }
        }

        EventManager.OnEndTurnEvent -= BossNeighbors;


        base.Death();   
    }

    //public void Spawn(Vector2Int location, GameObject boss)
    //{

    //    boss.transform.parent = GridManager.Instance.Grid[location].transform;
    //    boss.transform.position = GridManager.Instance.Grid[location].transform.position;
    //    GridManager.Instance.Grid[location].ChangeCurrentState(GridManager.Instance.gS_Boss);
    //}

    public void BossNeighbors()
    {
    //    foreach (Vector2Int loc in location)
    //    {
            BossReachableTiles = HexGridUtil.CoordinatesReachable(HexGridUtil.AxialToCubeCoord(axialLocation), SpawnRange, HexGridUtil.AxialToCubeCoord(GridManager.Instance.Grid.Keys.ToList<Vector2Int>()));
            BossReachableTiles.Remove(HexGridUtil.AxialToCubeCoord(axialLocation));

            foreach (Vector3Int neighbor in BossReachableTiles)
            {
                if (GridManager.Instance.Grid.ContainsKey(HexGridUtil.CubeToAxialCoord(neighbor)))
                {
                    if (GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(neighbor)].currentGridState != GridManager.Instance.gS_Enemy)
                        GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(neighbor)].ChangeCurrentState(GridManager.Instance.gS_BossNegative);
                }
            }
        //}
    }

    public void BossDeath(Vector2Int location)
    {
        if (GetComponent<Enemy>().currentHealth <= 0)
        {
            BossTiles = HexGridUtil.CoordinatesReachable(HexGridUtil.AxialToCubeCoord(location), SpawnRange, HexGridUtil.AxialToCubeCoord(GridManager.Instance.Grid.Keys.ToList<Vector2Int>()));
            BossTiles.Remove(HexGridUtil.AxialToCubeCoord(location));

            foreach (Vector3Int neighbor in BossTiles)
            {
                if (GridManager.Instance.Grid.ContainsKey(HexGridUtil.CubeToAxialCoord(neighbor)))
                {
                    GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(neighbor)].ChangeCurrentState(GridManager.Instance.gS_Positive);

                }
            }

        }
        //if (GetComponent<Enemy>().FirstAndLast)
        //{
        //    EventManager.OnPhaseChange();
        //}
        PlayerManager.Instance.SkillPoints += 2;
        EventManager.OnEndTurnEvent -= BossNeighbors;

    }

    public void TriggerSpread()
    {
        // if (turnCounter % everyXRounds == 0)
        {
            for (int i = 0; i < BossSpreads.Count; i++)
            {

                BossSpreads[i].TargetTile(HexGridUtil.AxialToCubeCoord(location[0]), out Vector3Int target, PlayerManager.Instance.playerPosition);
            }
        }
    }

    /* public void BossEnemyPhase2(Vector2Int location)
     {

         ReachableTiles = HexGridUtil.CoordinatesReachable(HexGridUtil.AxialToCubeCoord(location), 5, HexGridUtil.AxialToCubeCoord(GridManager.Instance.Grid.Keys.ToList<Vector2Int>()));


         foreach(KeyValuePair<Vector2Int,GridTile> kvp in GridManager.Instance.Grid)
         {
             if (kvp.Value.currentGridState == GridManager.Instance.gS_Enemy)
                 GridEnemies.Add(kvp.Value);
         }

         foreach(GridTile enemytile in GridEnemies)
         {
            Destroy(enemytile.transform.GetChild(1).gameObject);
            Instantiate(Enemy2Prefab);
             GridTile targetLocation = enemytile;

             GetComponent<Enemy>().Setup(GridManager.Instance.enemySOs[Random.Range(0, GridManager.Instance.enemySOs.Count)], targetLocation);
             targetLocation.ChangeCurrentState(GridManager.Instance.gS_Enemy);
             GetComponent<Enemy>().transform.parent = targetLocation.transform;
             GetComponent<Enemy>().transform.position = targetLocation.transform.position;
         }


     }*/


    public void BossParticle(GameObject boss)
    {
        Ressource ressource = GetComponent<Enemy>().ressource;

        switch (ressource)
        {
            case Ressource.ressourceA:
                Instantiate(blueParticle, boss.gameObject.transform.position, Quaternion.identity, boss.transform);
                break;
            case Ressource.ressourceB:
                Instantiate(orangeParticle, boss.gameObject.transform.position, Quaternion.identity, boss.transform);
                break;
            case Ressource.ressourceC:
                Instantiate(redParticle, boss.gameObject.transform.position, Quaternion.identity, boss.transform);
                break;
            case Ressource.resscoureD:
                Instantiate(greenParticle, boss.gameObject.transform.position, Quaternion.identity, boss.transform);
                break;
        }
    }
}
