using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1_Boss : Boss
{
    Boss boss;
    

    private void Start()
    {

        Debug.Log("phase1");
            boss.Spawn(Vector2Int.zero);

    }

    public void OnDestroy()
    {
         //EventManager.OnEndTurnEvent -= boss.BossNeighbors;
    }
}
