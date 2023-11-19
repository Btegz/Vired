using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class Boss : MonoBehaviour
{

    [SerializeField] List<Vector3Int> BossReachableTiles = new List<Vector3Int>();

    private void Start()
    {
     //   EventManager.OnEndTurnEvent += BossNeighbors;
    }

    public void BossNeighbors()
    {
        BossReachableTiles = HexGridUtil.CoordinatesReachable(HexGridUtil.AxialToCubeCoord(GridManager.Instance.BossSpawn), 4,HexGridUtil.AxialToCubeCoord(GridManager.Instance.Grid.Keys.ToList<Vector2Int>()));
        BossReachableTiles.Remove(HexGridUtil.AxialToCubeCoord(GridManager.Instance.BossSpawn));
        
        foreach (Vector3Int neighbor in BossReachableTiles)
        {
            if (GridManager.Instance.Grid.ContainsKey(HexGridUtil.CubeToAxialCoord(neighbor)))
                GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(neighbor)].ChangeCurrentState(GridManager.Instance.gS_BossNegative);
        }
    }

    public void OnDestroy()
    {
       // EventManager.OnEndTurnEvent -= BossNeighbors;
    }

 

}
