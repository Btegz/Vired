using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P3_Boss : Boss
{

    private void Start()
    {
        EventManager.OnEndTurnEvent += BossNeighbors;

        location = new List<Vector2Int>();
        location.Add(Vector2Int.zero);
        GetComponent<Enemy>().currentHealth = 8;
        GetComponent<Enemy>().Setup(GridManager.Instance.BossEnemySO, GridManager.Instance.Grid[Vector2Int.zero]);
        Spawn(Vector2Int.zero, gameObject);
        BossNeighbors();
    }

  


    public void OnDestroy()
    {
        GridManager.Instance.GameWon();
        BossDeath(location[0]);  
        PlayerManager.Instance.SkillPoints += 2;
        
    }
}
