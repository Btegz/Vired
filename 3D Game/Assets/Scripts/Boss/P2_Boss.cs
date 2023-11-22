using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2_Boss : Boss
{
    public GameObject Boss1;
    public GameObject Boss2;
    public GameObject Boss3;

    public Enemy enemy;
    [HideInInspector][SerializeField] List<Vector2Int> possibleTiles;
    [HideInInspector][SerializeField] int randomTile1;
    [HideInInspector][SerializeField] int randomTile2;
    [HideInInspector][SerializeField] int randomTile3;
    int BossAlive = 3;


    private void Start()
    {
        location = new List<Vector2Int>();
        EventManager.OnEndTurnEvent += BossNeighbors;

        

        
        Boss1 = Instantiate(Boss1);
        RandomTiles();
       
        Boss1.GetComponent<Enemy>().currentHealth = 4;
        Boss1.GetComponent<Enemy>().Setup(GridManager.Instance.BossEnemySO, GridManager.Instance.Grid[possibleTiles[randomTile1]]);
        Spawn(possibleTiles[randomTile1], Boss1);
        BossNeighbors();
        
        Boss2 = Instantiate(Boss2);
        Boss2.GetComponent<Enemy>().currentHealth = 4;
        Boss2.GetComponent<Enemy>().Setup(GridManager.Instance.BossEnemySO, GridManager.Instance.Grid[possibleTiles[randomTile2]]);
        Spawn(possibleTiles[randomTile2], Boss2);
        BossNeighbors();

        Boss3 = Instantiate(Boss3);
        Boss3.GetComponent<Enemy>().currentHealth = 4;
        Boss3.GetComponent<Enemy>().Setup(GridManager.Instance.BossEnemySO, GridManager.Instance.Grid[possibleTiles[randomTile3]]);
        Spawn(possibleTiles[randomTile3], Boss3);
        BossNeighbors();
        
        

    }

    public void Update()
    {
        Death();
    }

    public void Death()
    {
        if (Boss1 != null && Boss1.GetComponent<Enemy>().currentHealth <= 0)
        {
            BossAlive--;
            location.Remove(possibleTiles[randomTile1]);
        }

        if (Boss2 != null && Boss2.GetComponent<Enemy>().currentHealth <= 0)
        {
            BossAlive--;
            location.Remove(possibleTiles[randomTile2]);

        }

        if (Boss3 != null && Boss3.GetComponent<Enemy>().currentHealth <= 0)
        {
            BossAlive--;
            location.Remove(possibleTiles[randomTile3]);

        }


        if (BossAlive == 0)
        {
            Destroy(gameObject);
            
        }

    }

    public void RandomTiles()
    {
        foreach (KeyValuePair<Vector2Int, GridTile> kvp in GridManager.Instance.Grid)
        {
            if ((kvp.Value.currentGridState == GridManager.Instance.gS_Neutral) || (kvp.Value.currentGridState == GridManager.Instance.gS_Positive))
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

    }

    public void OnDestroy()
    {
        EventManager.OnPhaseChange();
    }
}
