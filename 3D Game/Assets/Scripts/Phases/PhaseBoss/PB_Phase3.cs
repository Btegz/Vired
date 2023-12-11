using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PB_Phase3", menuName = "PhaseBoss/PB_Phase3")]

public class PB_Phase3 : PhaseBoss
{
    public override void BossSpread(Vector2Int Bosslocation)
    {
        
        GridManager.Instance.Boss = Instantiate(GridManager.Instance.BossPrefab);
        GridManager.Instance.Boss.Setup(/*GridManager.Instance.BossEnemySO, */GridManager.Instance.Grid[Vector2Int.zero]);

        GridManager.Instance.Boss.transform.parent = GridManager.Instance.Grid[Vector2Int.zero].transform;
        GridManager.Instance.Boss.transform.position = GridManager.Instance.Grid[Vector2Int.zero].transform.position;
        GridManager.Instance.Grid[Vector2Int.zero].ChangeCurrentState(GridManager.Instance.gS_Boss);
    }


}
