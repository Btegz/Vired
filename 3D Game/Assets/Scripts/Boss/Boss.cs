using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class Boss : Enemy
{
    List<Vector3Int> BossReachableTiles;
    [HideInInspector] public List<Vector2Int> location;
    [HideInInspector] public float AliveCounter = 0;
    [SerializeField] public SkinnedMeshRenderer SkinnedMeshRenderer;


    [Header("Enemy Spawn")]
    [SerializeField] float EnemiesEveryXTurns;
    [SerializeField] List<Enemy> enemyPrefabPool;
    [SerializeField] public List<Spreadbehaviours> enemySpawnSpreadBehaviours;

    [Header("Boss Stats")]
    [SerializeField] int NegativeRange;
    [SerializeField] public int BossNegative;


    [Header("On Death Stats")]
    [SerializeField] List<Boss> NextBosses;
    [SerializeField] Spreadbehaviours nextBossSpawnPattern;
    [SerializeField] int AbilityLoadoutReward;


    [Header("Visuals")]
    public GameObject blueParticle;
    public GameObject orangeParticle;
    public GameObject redParticle;
    public GameObject greenParticle;

    public AudioData BossDeath;
    public AudioData BossNegativity;


    private void Start()
    {
        EventManager.OnEndTurnEvent += BossNeighbors;
        EventManager.OnEndTurnEvent += Spread;
     //   BossParticle(gameObject);
    }
    private void Update()
    {
       // AudioManager.Instance.PlayMusic(AudioManager.Instance.EnemyHovern);
    }
    private void OnDestroy()
    {
        EventManager.OnEndTurnEvent -= Spread;
        EventManager.OnEndTurnEvent -= BossNeighbors;
      //  AudioManager.Instance.StopMusic(AudioManager.Instance.EnemyHovern);
    }

    public override void Setup(GridTile tile)
    {
        base.Setup(tile);
        SkinnedMeshRenderer.material = GetComponentInChildren<SkinnedMeshRenderer>().material;
        tile.ChangeCurrentState(GridManager.Instance.gS_Boss);
        BossNeighbors();
    }

    public override int AmountSpreadNextTurn()
    {
        if ((GridManager.Instance.TurnCounter + 1) % everyXTurns == 0)
        {           
            return BossNegative;
        }
        return 0;
    }

    override public void Spread()
    {
        AliveCounter++;
        int spreads = 0;
        if (AliveCounter % EnemiesEveryXTurns == 0)
        {
            foreach (Spreadbehaviours sb in enemySpawnSpreadBehaviours)
            {
                if(sb.TargetTiles(HexGridUtil.AxialToCubeCoord(axialLocation), out List<Vector3Int> targets, FindClosestPlayer().CoordinatePosition))
                {
                    for(int i = spreads; i < SpreadAmount && i < targets.Count; i++, spreads++)
                    {
                        Vector3Int target = targets[Random.Range(0, targets.Count)];
                        Enemy newEnemy = Instantiate(enemyPrefabPool[Random.Range(0, enemyPrefabPool.Count)], GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(target)].transform);
                        Vector3 goalPosition = newEnemy.transform.position;
                        newEnemy.Setup(GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(target)]);                        
                        newEnemy.transform.DOMove(goalPosition, 3).From(goalPosition + Vector3.down * .5f);
                        targets.Remove(target);
                    }
                }
            }
        }
    }

    public override void Death()
    {
        if (NextBosses == null || NextBosses.Count == 0)
        {
            GridManager.Instance.GameWon();
           
            return;
        }
        AudioManager.Instance.PlaySoundAtLocation(BossDeath, soundEffect, null, true);
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
        foreach (Boss b in NextBosses)
        {
            if (nextBossSpawnPattern == null)
            {
                Boss newBoss = Instantiate(b);
                newBoss.Setup(GridManager.Instance.Grid[Vector2Int.zero]);
                break;
            }
            if (nextBossSpawnPattern.TargetTiles(Vector3Int.zero, out List<Vector3Int> targets, Vector2Int.zero))
            {
                Boss newBoss = Instantiate(b);
                newBoss.Setup(GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(targets[Random.Range(0, targets.Count)])]);
            }
        }
        base.Death();
    }

    public void BossNeighbors()
    {
        BossReachableTiles = HexGridUtil.CoordinatesReachable(HexGridUtil.AxialToCubeCoord(axialLocation), NegativeRange, HexGridUtil.AxialToCubeCoord(GridManager.Instance.Grid.Keys.ToList<Vector2Int>()));
        BossReachableTiles.Remove(HexGridUtil.AxialToCubeCoord(axialLocation));

        foreach (Vector3Int neighbor in BossReachableTiles)
        {
            if (GridManager.Instance.Grid.ContainsKey(HexGridUtil.CubeToAxialCoord(neighbor)))
            {
                if (GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(neighbor)].currentGridState != GridManager.Instance.gS_Enemy && GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(neighbor)].currentGridState != GridManager.Instance.gS_PofI && GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(neighbor)].currentGridState != GridManager.Instance.gS_BossNegative &&!PlayerManager.Instance.PlayerPositions().Contains(HexGridUtil.CubeToAxialCoord(neighbor)))
                {
                    GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(neighbor)].ChangeCurrentState(GridManager.Instance.gS_BossNegative);

                    BossNegative++;
                    AudioManager.Instance.PlaySoundAtLocation(BossNegativity, soundEffect, null, true);
                }
            }
        }
    }

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
            case Ressource.ressourceD:
                Instantiate(greenParticle, boss.gameObject.transform.position, Quaternion.identity, boss.transform);
                break;
        }
    }
}
