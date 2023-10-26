using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PE_SpreadNegative : PhaseEffect
{
    public override void TriggerPhaseEffect(int turnCounter, GridManager gridManager)
    {
        if (turnCounter % everyXRounds == 0)
        {
            List<GridTile> enemieTiles = gridManager.GetTilesWithState(gridManager.gS_Enemy);
            foreach (GridTile tile in enemieTiles)
            {
                Vector3Int coordinate = HexGridUtil.AxialToCubeCoord(tile.AxialCoordinate);
                List<Vector3Int> possibleTiles = new List<Vector3Int>();
                for (int i = 1;i<10 ; i++)
                {
                    List<Vector3Int> range = HexGridUtil.CoordinatesReachable(coordinate, i);
                    foreach (Vector3Int neighbor in range)
                    {
                        Vector2Int neighborAxial = HexGridUtil.CubeToAxialCoord(neighbor);
                        if (gridManager.Grid.ContainsKey(neighborAxial))
                        {
                            if (gridManager.Grid[neighborAxial].currentGridState.Equals(gridManager.gS_Positive) || gridManager.Grid[neighborAxial].currentGridState.Equals(gridManager.gS_Neutral))
                            {
                                possibleTiles.Add(neighbor);
                            }
                        }
                    }
                    if (possibleTiles.Count > 0)
                    {
                        break;
                    }
                }
                if(possibleTiles.Count > 0)
                {
                    int randomIndex = Random.Range(0, possibleTiles.Count);
                    gridManager.Grid[HexGridUtil.CubeToAxialCoord(possibleTiles[randomIndex])].ChangeCurrentState(GridManager.Instance.gS_Negative);
                }
            }
        }
    }
}
