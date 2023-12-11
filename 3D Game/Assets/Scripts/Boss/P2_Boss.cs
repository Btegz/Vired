using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class P2_Boss : Boss
{
    public GameObject Boss1;
    public GameObject Boss2;
    public GameObject Boss3;

    [SerializeField] public Dictionary<Vector2Int, GameObject> Boss;


    public PE_EnemySpawn PE_EnemySpawn;
    public PE_MiniEnemySpawn PE_MiniEnemySpawn;
    public Phase Phase;


    public Enemy enemy;
    [HideInInspector][SerializeField] List<Vector2Int> possibleTiles;
    [HideInInspector][SerializeField] int randomTile1;
    [HideInInspector][SerializeField] int randomTile2;
    [HideInInspector][SerializeField] int randomTile3;
    int BossAlive = 3;


    private void Start()
    {
        //enemy.FirstAndLast = false;
        location = new List<Vector2Int>();
        EventManager.OnEndTurnEvent += BossNeighbors;
        PE_EnemySpawn.everyXRounds = 2;
       
        Boss = new Dictionary<Vector2Int, GameObject>();
        Boss1 = Instantiate(Boss1);
        Boss2 = Instantiate(Boss2);
        Boss3 = Instantiate(Boss3);
        RandomTiles();
     

        

        foreach(KeyValuePair<Vector2Int, GameObject> kvp in Boss)
        {
           
            kvp.Value.GetComponent<Enemy>().currentHealth = 4;
            kvp.Value.GetComponent<Enemy>().Setup(/*GridManager.Instance.BossEnemySO, */GridManager.Instance.Grid[kvp.Key]);
            //Spawn(kvp.Key, kvp.Value);
            BossNeighbors();
            BossParticle(kvp.Value);
        }


        //Boss1 = Instantiate(Boss1);
        //RandomTiles();
       
        //Boss1.GetComponent<Enemy>().currentHealth = 4;
        //Boss1.GetComponent<Enemy>().Setup(GridManager.Instance.BossEnemySO, GridManager.Instance.Grid[possibleTiles[randomTile1]]);
        //Spawn(possibleTiles[randomTile1], Boss1);
        //BossNeighbors();
        //BossParticle(Boss1);
        
        //Boss2 = Instantiate(Boss2);
        //Boss2.GetComponent<Enemy>().currentHealth = 4;
        //Boss2.GetComponent<Enemy>().Setup(GridManager.Instance.BossEnemySO, GridManager.Instance.Grid[possibleTiles[randomTile2]]);
        //Spawn(possibleTiles[randomTile2], Boss2);
        //BossNeighbors();
        //BossParticle(Boss2);

        //Boss3 = Instantiate(Boss3);
        //Boss3.GetComponent<Enemy>().currentHealth = 4;
        //Boss3.GetComponent<Enemy>().Setup(GridManager.Instance.BossEnemySO, GridManager.Instance.Grid[possibleTiles[randomTile3]]);
        //Spawn(possibleTiles[randomTile3], Boss3);
        //BossNeighbors();
        //BossParticle(Boss3);


        for(int i=0; i<3; i++)
        {
            Phase.myPhaseEffects.Add(PE_MiniEnemySpawn);
        }
      
 

    }

   public void Update()
    {
        Death();
    }

    public void Death()
    {

        foreach(KeyValuePair<Vector2Int, GameObject> kvp in Boss)
        { 
        if (kvp.Value != null && kvp.Value.GetComponent<Enemy>().currentHealth <= 0)
        {
            BossAlive--;
            BossDeath(kvp.Key);
            location.Remove(kvp.Key);
            PlayerManager.Instance.SkillPoints += 2;
            
                Destroy(kvp.Value);
                
            for (int i = 0; i < 1; i++)
            {
                Phase.myPhaseEffects.Remove(PE_MiniEnemySpawn);

            }
        }

        //if (Boss2 != null && Boss2.GetComponent<Enemy>().currentHealth <= 0)
        //{
        //    BossAlive--;
        //    BossDeath(possibleTiles[randomTile2]);
        //    location.Remove(possibleTiles[randomTile2]);
        //    PlayerManager.Instance.SkillPoints += 2;
        //    for (int i = 0; i <1; i++)
        //    {
        //        Phase.myPhaseEffects.Remove(PE_MiniEnemySpawn);

        //    }

        //}

        //if (Boss3 != null && Boss3.GetComponent<Enemy>().currentHealth <= 0)
        //{
        //    BossAlive--;
        //    BossDeath(possibleTiles[randomTile3]);
        //    location.Remove(possibleTiles[randomTile3]);
        //    PlayerManager.Instance.SkillPoints += 2;
        //    for (int i = 0; i < 1; i++)
        //    {
        //        Phase.myPhaseEffects.Remove(PE_MiniEnemySpawn);

        //    }


        }


        if (BossAlive == 0)
        {
            EventManager.OnPhaseChange();
            Destroy(gameObject);
            

        }

    }

    public void RandomTiles()
    {
      
        foreach (KeyValuePair<Vector2Int, GridTile> kvp in GridManager.Instance.Grid)
        {
            if (kvp.Value.currentGridState == GridManager.Instance.gS_Negative || kvp.Value.currentGridState == GridManager.Instance.gS_Enemy || kvp.Value.currentGridState == GridManager.Instance.gS_Boss || kvp.Value.currentGridState == GridManager.Instance.gS_BossNegative || kvp.Value.currentGridState == GridManager.Instance.gS_PofI)
            {
                continue;
            }

            List<Vector3Int> reachableTiles = HexGridUtil.CoordinatesReachable(HexGridUtil.AxialToCubeCoord(kvp.Key), 3, HexGridUtil.AxialToCubeCoord(GridManager.Instance.Grid.Keys.ToList<Vector2Int>()));
            bool stillvalid = true;
            foreach (Vector3Int tile in reachableTiles)
            {
                Vector2Int axTile = HexGridUtil.CubeToAxialCoord(tile);
                if (GridManager.Instance.Grid.ContainsKey(axTile))
                {
                    if (GridManager.Instance.Grid[axTile].currentGridState == GridManager.Instance.gS_Negative || GridManager.Instance.Grid[axTile].currentGridState == GridManager.Instance.gS_Enemy || GridManager.Instance.Grid[axTile].currentGridState == GridManager.Instance.gS_Boss || GridManager.Instance.Grid[axTile].currentGridState == GridManager.Instance.gS_BossNegative)
                    {
                        stillvalid = false;
                        break; 
                    }
                }
            }
            if (!stillvalid)
            {
                continue;
            }

            if (!PlayerManager.Instance.PlayerPositions().Contains(kvp.Key))
            {
                possibleTiles.Add(kvp.Key);
            }

        }

        randomTile1 = Random.Range(1, possibleTiles.Count);
        randomTile2 = Random.Range(1, possibleTiles.Count);
        randomTile3 = Random.Range(1, possibleTiles.Count);

        location.Add(possibleTiles[randomTile1]);
        location.Add(possibleTiles[randomTile2]);
        location.Add(possibleTiles[randomTile3]);

        Boss.Add(possibleTiles[randomTile1], Boss1);
        Boss.Add(possibleTiles[randomTile2], Boss2);
        Boss.Add(possibleTiles[randomTile3], Boss3);

    }

    public void OnDestroy()
    {
        for (int i = 0; i <3; i++)
        {
            Phase.myPhaseEffects.Remove(PE_MiniEnemySpawn);

        }
        //enemy.FirstAndLast = true;
        PE_EnemySpawn.everyXRounds = 2;


    }
}
