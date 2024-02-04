using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class Spread_RandomTile : Spreadbehaviours
{
    public override bool TargetTile(Vector3Int enemyPosition, out Vector3Int target, Vector2Int playerPosition)
    {
        if (TargetTiles(enemyPosition, out List<Vector3Int> targets, playerPosition))
        {
            target = targets[Random.Range(0, targets.Count)];
            return true;
        }
        target = Vector3Int.zero;
        return false;
    }

    public override bool TargetTiles(Vector3Int origin, out List<Vector3Int> targets, Vector2Int closestPlayerPosition)
    {
        List<Vector3Int> result = new List<Vector3Int>();
        foreach (KeyValuePair<Vector2Int, GridTile> kvp in GridManager.Instance.Grid)
        {
            if (kvp.Value.currentGridState.StateValue() == 0 || kvp.Value.currentGridState.StateValue() == 1)
            {
                if (!PlayerManager.Instance.PlayerPositions().Contains(kvp.Key))
                {
                    result.Add(HexGridUtil.AxialToCubeCoord(kvp.Key)); 
                }
            }

        }
        targets = result;
        return true;
    }

}

