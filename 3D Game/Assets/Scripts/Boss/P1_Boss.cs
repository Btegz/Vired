using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1_Boss : Boss
{
    public Enemy enemy;
    public BossManager bossManager;
    private void Start()
    {
        EventManager.OnEndTurnEvent += BossNeighbors;

        location = new List<Vector2Int>();

        enemy.Setup(GridManager.Instance.BossEnemySO, GridManager.Instance.Grid[GridManager.Instance.BossSpawn]);
        Spawn(GridManager.Instance.BossSpawn, gameObject);
        location.Add(GridManager.Instance.BossSpawn);
        BossNeighbors();
        enemy.currentHealth = 5;
    }


   


    public void OnDestroy()
     {
        EventManager.OnPhaseChange();
        BossDeath(location[0]);      
        PlayerManager.Instance.SkillPoints += 2;
        
    }
}
