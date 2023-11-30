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

        Debug.Log(bossManager.Playtesting);
        /*
                for (int i = 0; i < PlayerManager.Instance.Players.Count; i++)
                {
                    float PlayerDistanceToBoss = HexGridUtil.CubeDistance(HexGridUtil.AxialToCubeCoord(PlayerManager.Instance.Players[i].CoordinatePosition), Vector3Int.zero);
                    PlayerDistances.Add(PlayerDistanceToBoss);
                }

                PlayerDistances.Sort();

                Debug.Log(PlayerDistances[0]);
                Debug.Log(PlayerDistances[1]);
                Debug.Log(PlayerDistances[2]);
        */

    }






    public void OnDestroy()
     {
        if (bossManager.Playtesting == true)
        {
           
            GridManager.Instance.GameWon();
            BossDeath(location[0]);
            PlayerManager.Instance.SkillPoints += 2;
            EventManager.OnEndTurnEvent -= BossNeighbors;
        }

        else
        {
            EventManager.OnPhaseChange();
            BossDeath(location[0]);
            PlayerManager.Instance.SkillPoints += 2;
            EventManager.OnEndTurnEvent -= BossNeighbors;
        }
    }
}
