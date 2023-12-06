using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1_Boss : Boss
{
    public Enemy enemy;
    public BossManager bossManager;
    List<float> PlayerDistances = new List<float>();
    
    private void Start()
    {
        EventManager.OnEndTurnEvent += BossNeighbors;

        location = new List<Vector2Int>();
        //EventManager.OnEndTurnEvent += TriggerSpread;
        enemy.Setup(GridManager.Instance.BossEnemySO, GridManager.Instance.Grid[GridManager.Instance.BossSpawn]);
        Spawn(GridManager.Instance.BossSpawn, gameObject);
        location.Add(GridManager.Instance.BossSpawn);
        BossNeighbors();
        enemy.currentHealth = 5;
        BossParticle(gameObject);

       
   

    }






    public void OnDestroy()
     {
        
        //    BossDeath(location[0]);
        //    Debug.Log("location" + location[0]);
        //EventManager.OnPhaseChange();
        //PlayerManager.Instance.SkillPoints += 2;
        //EventManager.OnEndTurnEvent -= BossNeighbors;

    }
}
