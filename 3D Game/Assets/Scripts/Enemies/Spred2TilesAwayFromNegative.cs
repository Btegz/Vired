using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Spred2TilesAwayFromNegative : Spreadbehaviours
{
    public override bool TargetTile(Vector3Int enemyPosition, out Vector3Int target, Vector2Int playerPosition)
    {
        Dictionary<Vector2Int, GridTile> grid = GridManager.Instance.Grid;

        List<Vector3Int> possibleTiles = new List<Vector3Int>();

        foreach (KeyValuePair<Vector2Int, GridTile> kvp in grid)
        {
            if (kvp.Value.currentGridState == GridManager.Instance.gS_Negative || kvp.Value.currentGridState == GridManager.Instance.gS_Enemy || kvp.Value.currentGridState == GridManager.Instance.gS_Boss || kvp.Value.currentGridState == GridManager.Instance.gS_BossNegative)
            {
                continue;
            }

            List<Vector3Int> reachableTiles = HexGridUtil.CoordinatesReachable(HexGridUtil.AxialToCubeCoord(kvp.Key), 3);
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

            if(kvp.Key != playerPosition)
            possibleTiles.Add(HexGridUtil.AxialToCubeCoord(kvp.Key));
        }

        if (possibleTiles.Count == 0)
        {
            target = Vector3Int.zero;
            return false;
        }

        target = possibleTiles[Random.Range(0, possibleTiles.Count)];
        return true;
    }
}
