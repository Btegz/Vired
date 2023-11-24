using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Phase1Boss_SpreadBehaviour : Spreadbehaviours
{
    public override bool TargetTile(Vector3Int enemyPosition, out Vector3Int target, Vector2Int playerPosition)
    {
        List<Vector3Int> ring = HexGridUtil.Ring(enemyPosition, 4, GridManager.Instance.Grid.Keys.ToList()); ;
        List<float> PlayerDistances =new List<float>();

        for (int i = 0; i < PlayerManager.Instance.Players.Count; i++)
        {
            float PlayerDistanceToBoss = HexGridUtil.CubeDistance(HexGridUtil.AxialToCubeCoord(PlayerManager.Instance.Players[i].CoordinatePosition), enemyPosition);
            PlayerDistances.Add(PlayerDistanceToBoss);
        }

        PlayerDistances.Sort();

        Debug.Log(PlayerDistances[0]);
        Debug.Log(PlayerDistances[1]);
        Debug.Log(PlayerDistances[2]);

        target = enemyPosition;
        return true;
    }


    
    

  
}
