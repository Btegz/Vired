using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PE_SpreadPositive : PhaseEffect
{
    public override void TriggerPhaseEffect(int turnCounter, GridManager gridManager)
    {
        Debug.Log($"To Spread Positive TurnCounter: {turnCounter}, everyXRounds: {everyXRounds}, turnCounter%everyXRounds = {turnCounter % everyXRounds}");
        if (turnCounter % everyXRounds == 0)
        {
            List<GridTile> neutralTiles = gridManager.GetTilesWithState(gridManager.gS_Neutral);
            foreach (GridTile tile in neutralTiles)
            {
                if (tile.AxialCoordinate != HexGridUtil.CubeToAxialCoord(PlayerManager.Instance.playerPosition))
                {
                    tile.ChangeCurrentState(gridManager.gS_Positive);
                }

            }
        }
    }
}
