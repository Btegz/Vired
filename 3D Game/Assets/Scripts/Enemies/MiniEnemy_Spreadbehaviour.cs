using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu]
public class MiniEnemy_Spreadbehaviour : Spreadbehaviours
{
    [SerializeField] public MapSettings mapSettings;

    public override bool TargetTile(Vector3Int enemyPosition, out Vector3Int target, Vector2Int playerPosition)
    {
        if(TargetTiles(enemyPosition,out List<Vector3Int> possibleTargets, playerPosition))
        {
            target = possibleTargets[Random.Range(0, possibleTargets.Count)];
            return true;
        }
        target = Vector3Int.zero;
        return false;
    }

    public override bool TargetTiles(Vector3Int origin, out List<Vector3Int> targets, Vector2Int closestPlayerPosition)
    {
        Dictionary<Vector2Int, GridTile> grid = GridManager.Instance.Grid;

        List<Vector3Int> possibleTargets = new List<Vector3Int>();


        foreach (KeyValuePair<Vector2Int, GridTile> kvp in grid)
        {
            if (kvp.Value.currentGridState.StateValue() >= 0 && !PlayerManager.Instance.PlayerPositions().Contains(kvp.Key))
            {
                possibleTargets.Add(HexGridUtil.AxialToCubeCoord(kvp.Key));
            }
        }
        targets = possibleTargets;
        return true;
    }
}
