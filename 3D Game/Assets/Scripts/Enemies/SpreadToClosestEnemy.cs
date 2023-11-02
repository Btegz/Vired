using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpreadToClosestEnemy : Spreadbehaviours
{
    public override bool TargetTile(Vector3Int enemyPosition, out Vector3Int target)
    {
        // We get every Enemie's Coordiante
        List<GridTile> EnemyTiles = GridManager.Instance.GetTilesWithState(GridManager.Instance.gS_Enemy);
        List<Vector3Int> EnemyCoordinates = new List<Vector3Int>();
        foreach(GridTile gt in EnemyTiles)
        {
            EnemyCoordinates.Add(HexGridUtil.AxialToCubeCoord(gt.AxialCoordinate));
        }

        // We Remove the Enemy that we cuurently handle
        EnemyCoordinates.Remove(enemyPosition);

        // We extract Coordinates from the Grid
        List<Vector3Int> GridCoordinates = new List<Vector3Int>();
        foreach(KeyValuePair<Vector2Int,GridTile> kvp in GridManager.Instance.Grid)
        {
            GridCoordinates.Add(HexGridUtil.AxialToCubeCoord(kvp.Key));
        }

        // We seek the shortest path from each enemies path
        int Length = int.MaxValue;
        List<Vector3Int> shortestPath = new List<Vector3Int>();

        foreach(Vector3Int enemyLoc in EnemyCoordinates)
        {
            int pathcost = 0;
            List<Vector3Int> path = HexGridUtil.CostHeuristicPathFind(enemyPosition, enemyLoc,GridManager.Instance.Grid,out pathcost);
            if(pathcost == 0)
            {
                continue;
            }
            if(pathcost < Length)
            {
                Length = path.Count;
                shortestPath = path;
            }
        }

        foreach(Vector3Int pathNode in shortestPath)
        {
            if (GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(pathNode)].currentGridState == GridManager.Instance.gS_Neutral || GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(pathNode)].currentGridState == GridManager.Instance.gS_Positive)
            {
                target = pathNode;
                return true;
            }
        }
        target = Vector3Int.zero;

        return false;
    }

}
