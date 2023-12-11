using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu (fileName ="SpreadAroundEnemy",menuName ="SpreadBehaviours/SpreadAroundEnemy")]
public class SpreadAroundEnemy : Spreadbehaviours
{
    public override bool TargetTile(Vector3Int enemyPosition, out Vector3Int target, Vector2Int playerPosition)
    {
        if(TargetTiles(enemyPosition, out List<Vector3Int> targets, playerPosition))
        {
            target = targets[Random.Range(0,targets.Count)];
            return true;
        }
        target = Vector3Int.zero;
        return false;
    }

    public override bool TargetTiles(Vector3Int origin, out List<Vector3Int> targets, Vector2Int closestPlayerPosition)
    {
        List<Vector3Int> possibleTargets = new List<Vector3Int>();
        for (int i = 2; possibleTargets.Count <= 0; i++)
        {

            List<Vector3Int> circle = HexGridUtil.CoordinatesReachable(origin, i, HexGridUtil.AxialToCubeCoord(GridManager.Instance.Grid.Keys.ToList<Vector2Int>()));
            foreach (Vector3Int coord in circle)
            {
                Vector2Int axcoord = HexGridUtil.CubeToAxialCoord(coord);
                if (GridManager.Instance.Grid[axcoord].currentGridState.StateValue() >= 0 && !PlayerManager.Instance.PlayerPositions().Contains(HexGridUtil.CubeToAxialCoord(coord)) && GridManager.Instance.Grid[axcoord].currentGridState.StateValue() != 4)
                {
                    possibleTargets.Add(coord);
                }
            }
        }
        targets = possibleTargets;
        return true;
    }
}
