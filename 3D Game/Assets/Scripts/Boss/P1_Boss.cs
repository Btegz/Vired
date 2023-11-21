using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1_Boss : Boss
{
    public Enemy enemy;
    public BossManager bossManager;
    private void Start()
    {
        enemy.Setup(GridManager.Instance.BossEnemySO, GridManager.Instance.Grid[GridManager.Instance.BossSpawn]);
        Spawn(GridManager.Instance.BossSpawn, gameObject);
        BossNeighbors(GridManager.Instance.BossSpawn);
        enemy.currentHealth = 5;
    }

  

     public void OnDestroy()
     {
        EventManager.OnPhaseChange();
     }

    //override Spawn
}
