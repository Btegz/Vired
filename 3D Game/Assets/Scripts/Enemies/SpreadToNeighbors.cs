using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "SpreadToNeighbors", menuName = "SpreadBehaviours/ SpreadToNeighbors")]
public class SpreadToNeighbors : Spreadbehaviours
{

    public override bool TargetTile(Vector3Int enemyPosition, out Vector3Int target, Vector2Int playerPosition)
    {
        List<Vector3Int> neighbors = HexGridUtil.CubeNeighbors(enemyPosition);
        List<Vector2Int> possibleTargets = new List<Vector2Int>();
        playerPosition = PlayerManager.Instance.playerPosition;

        foreach (Vector3Int neighbor in neighbors)
        {   
            
            Vector2Int axialNeighbor =HexGridUtil.CubeToAxialCoord(neighbor);
            try
            { 
                if (GridManager.Instance.Grid[axialNeighbor].currentGridState == GridManager.Instance.gS_Neutral ||
                  GridManager.Instance.Grid[axialNeighbor].currentGridState == GridManager.Instance.gS_Positive)
                {   
                    if(axialNeighbor != playerPosition)
                    possibleTargets.Add(axialNeighbor);
                }
            }
            catch (Exception e)
            {
                Debug.Log("Tile not valid");
            }
        }

        if (possibleTargets.Count == 0)
        {
            Debug.Log("no possible tragets found");
            target = Vector3Int.zero;
            return false;
        }

        else
        {
            target = HexGridUtil.AxialToCubeCoord(possibleTargets[Random.Range (0,possibleTargets.Count)]);
            return true;
        }
    }
}
