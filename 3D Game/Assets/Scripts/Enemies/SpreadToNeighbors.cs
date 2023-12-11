using DG.Tweening;
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
        List<Vector3Int> neighbors = HexGridUtil.CubeNeighbors(origin);
        List<Vector2Int> possibleTargets = new List<Vector2Int>();

        foreach (Vector3Int neighbor in neighbors)
        {

            Vector2Int axialNeighbor = HexGridUtil.CubeToAxialCoord(neighbor);
            try
            {
                if (GridManager.Instance.Grid[axialNeighbor].currentGridState == GridManager.Instance.gS_Neutral ||
                    GridManager.Instance.Grid[axialNeighbor].currentGridState == GridManager.Instance.gS_Positive)
                {
                    if (!PlayerManager.Instance.PlayerPositions().Contains(axialNeighbor))
                    {
                        possibleTargets.Add(axialNeighbor);
                    }
                }
            }
            catch (Exception e)
            {
            }
        }

        if (possibleTargets.Count == 0)
        {
            targets = new List<Vector3Int>();
            return false;
        }

        else
        {
            targets = HexGridUtil.AxialToCubeCoord(possibleTargets);
            return true;
        }
    }
}
