using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P3_Boss : Boss
{

    private void Start()
    {
        GetComponent<Enemy>().currentHealth = 8;
        GetComponent<Enemy>().Setup(GridManager.Instance.BossEnemySO, GridManager.Instance.Grid[Vector2Int.zero]);
        Spawn(Vector2Int.zero, gameObject);
        BossNeighbors(Vector2Int.zero);
        
    }


    public void OnDestroy()
    {
        EventManager.OnPhaseChange();
        GridManager.Instance.GameWon();
    }
}
