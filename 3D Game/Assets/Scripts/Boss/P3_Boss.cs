using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P3_Boss : Boss
{
    public PE_EnemySpawn PE_EnemySpawn;
    public Phase Phase;
    private void Start()
    {
        EventManager.OnEndTurnEvent += BossNeighbors;
        EventManager.OnEndTurnEvent += TriggerSpread;

        location = new List<Vector2Int>();
        location.Add(Vector2Int.zero);
       
        GetComponent<Enemy>().Setup(GridManager.Instance.BossEnemySO, GridManager.Instance.Grid[Vector2Int.zero]);
        Spawn(Vector2Int.zero, gameObject);
        BossNeighbors();
        GetComponent<Enemy>().currentHealth = 8;
        BossParticle(this.gameObject);
        PE_EnemySpawn.everyXRounds = 3;
        Phase.myPhaseEffects.Remove(PE_EnemySpawn);
   
    }

  


    public void OnDestroy()
    {
        GridManager.Instance.GameWon();
      //  BossDeath(location[0]);  
      //  PlayerManager.Instance.SkillPoints += 2;
     //   EventManager.OnEndTurnEvent -= BossNeighbors;
        EventManager.OnEndTurnEvent -= TriggerSpread;
        PE_EnemySpawn.everyXRounds = 3;
        Phase.myPhaseEffects.Add(PE_EnemySpawn);

    }
}
