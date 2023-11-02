using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Boss : MonoBehaviour
{

    [SerializeField] Vector2Int location;
    [SerializeField] List<Vector3Int> BossReachableTiles = new List<Vector3Int>();

    private void Start()
    {
        EventManager.OnEndTurnEvent += BossNeighbors;
        location = GetComponentInParent<GridTile>().AxialCoordinate;
    }

    public void BossNeighbors()
    {
        

            BossReachableTiles =  HexGridUtil.CoordinatesReachable(HexGridUtil.AxialToCubeCoord(location), 4);
        foreach(Vector3Int neighbor in BossReachableTiles)
        {   
            if (GridManager.Instance.Grid.ContainsKey(HexGridUtil.CubeToAxialCoord (neighbor)))
            GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(neighbor)].ChangeCurrentState(GridManager.Instance.gS_BossNegative); 
        }
    }

    public void OnDestroy()
    {
        EventManager.OnEndTurnEvent -= BossNeighbors;
    }

}
