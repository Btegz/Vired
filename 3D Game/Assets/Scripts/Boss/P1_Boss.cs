using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1_Boss : Boss
{ 
    private void Start()
    {

      Spawn(Vector2Int.zero);

    }

    public void OnDestroy()
    {
         //EventManager.OnEndTurnEvent -= boss.BossNeighbors;
    }

    //override Spawn
}
