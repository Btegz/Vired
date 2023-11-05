using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu (fileName ="SpreadAroundEnemy",menuName ="SpreadBehaviours/SpreadAroundEnemy")]
public class SpreadAroundEnemy : Spreadbehaviours
{
    public override bool TargetTile(Vector3Int enemyPosition, out Vector3Int target, Vector2Int playerPosition)
    {
        List<Vector3Int> possibleTargets = new List<Vector3Int>();
        for (int i = 2; possibleTargets.Count <= 0; i++)
        {

            List<Vector3Int> circle = HexGridUtil.CoordinatesReachable(enemyPosition, i, HexGridUtil.AxialToCubeCoord(GridManager.Instance.Grid.Keys.ToList<Vector2Int>()));
            foreach (Vector3Int coord in circle)
            {
                Vector2Int axcoord = HexGridUtil.CubeToAxialCoord(coord);
                if (GridManager.Instance.Grid[axcoord].currentGridState.StateValue() >= 0)
                {
                    possibleTargets.Add(coord);
                }
            }
        }
        target = possibleTargets[Random.Range(0, possibleTargets.Count)];
        return true;
    }
}
