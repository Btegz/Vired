using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class Spred2TilesAwayFromNegative : Spreadbehaviours
{
    public override bool TargetTile(Vector3Int enemyPosition, out Vector3Int target, Vector2Int playerPosition)
    {
        if(TargetTiles(enemyPosition, out List<Vector3Int> targets, playerPosition))
        {
            target = targets[Random.Range(0, targets.Count)];
            return true;
        }
        target = Vector3Int.zero;
        return false;
    }

    public override bool TargetTiles(Vector3Int origin, out List<Vector3Int> targets, Vector2Int closestPlayerPosition)
    {
        Dictionary<Vector2Int, GridTile> grid = GridManager.Instance.Grid;

        List<Vector3Int> possibleTiles = new List<Vector3Int>();

        foreach (KeyValuePair<Vector2Int, GridTile> kvp in grid)
        {
            if (kvp.Value.currentGridState == GridManager.Instance.gS_Negative || kvp.Value.currentGridState == GridManager.Instance.gS_Enemy || kvp.Value.currentGridState == GridManager.Instance.gS_Boss || kvp.Value.currentGridState == GridManager.Instance.gS_BossNegative || kvp.Value.currentGridState == GridManager.Instance.gS_PofI)
            {
                continue;
            }

            List<Vector3Int> reachableTiles = HexGridUtil.CoordinatesReachable(HexGridUtil.AxialToCubeCoord(kvp.Key), 3, HexGridUtil.AxialToCubeCoord(GridManager.Instance.Grid.Keys.ToList<Vector2Int>()));
            bool stillvalid = true;
            foreach (Vector3Int tile in reachableTiles)
            {
                Vector2Int axTile = HexGridUtil.CubeToAxialCoord(tile);
                if (grid.ContainsKey(axTile))
                {
                    if (grid[axTile].currentGridState == GridManager.Instance.gS_Negative || grid[axTile].currentGridState == GridManager.Instance.gS_Enemy || grid[axTile].currentGridState == GridManager.Instance.gS_Boss || grid[axTile].currentGridState == GridManager.Instance.gS_BossNegative)
                    {
                        stillvalid = false;
                        break; // ends the loop
                    }
                }
            }
            if (!stillvalid)
            {
                continue;
            }

            if (!PlayerManager.Instance.PlayerPositions().Contains(kvp.Key))
            {
                possibleTiles.Add(HexGridUtil.AxialToCubeCoord(kvp.Key));
            }

        }

        if (possibleTiles.Count == 0)
        {
            targets = new List<Vector3Int>();
            return false;
        }

        targets = possibleTiles;
        return true;
    }
}
