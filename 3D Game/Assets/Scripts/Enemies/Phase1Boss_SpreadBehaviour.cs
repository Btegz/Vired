using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading;

[CreateAssetMenu(fileName = "BossSpread")]
public class Phase1Boss_SpreadBehaviour : Spreadbehaviours
{
    Dictionary<float, Vector2Int> sortedDict;
    public float rotationFactor;
    List<Vector3Int> WorldShapeCoordinates = new List<Vector3Int>();


    public HexShape SpreadShape;
    List<Vector2Int> PlayerPosition = new List<Vector2Int>();
    int counter;
    int ringCount;
    int RotationCount;


    public override bool TargetTile(Vector3Int enemyPosition, out Vector3Int target, Vector2Int playerPosition)
    {
        List<Vector2Int> ring = HexGridUtil.Ring(enemyPosition, 2); ;
        List<Vector2Int> targetTile = new List<Vector2Int>();
        List<Vector3Int> possibleTargets = new List<Vector3Int>();
        int maxDistance = int.MaxValue;
        int maxD = int.MaxValue;

        /// Player Distanzen zum Boss werden in einer Liste gespeichert 
        /// MaxDistance i wird als counter gespeichert 
        for (int i = 0; i < PlayerManager.Instance.Players.Count; i++)
        {
            //Distanz Player zu Boss
            int PlayerDistanceToBoss = HexGridUtil.CubeDistance(HexGridUtil.AxialToCubeCoord(PlayerManager.Instance.Players[i].CoordinatePosition), enemyPosition);

            if (PlayerDistanceToBoss < maxDistance)
            {
                counter = i;
                maxDistance = PlayerDistanceToBoss;

            }
        }


        /// Distanzen vom Player i zum Ring um den negative Boss Spread 
        /// kleinste Distanz wird abgespeichert 
        /// j als counter 
        for (int j = 0; j < ring.Count; j++)
        {

            int PlayerDistanceToTargetTile = HexGridUtil.CubeDistance(HexGridUtil.AxialToCubeCoord(PlayerManager.Instance.Players[counter].CoordinatePosition), HexGridUtil.AxialToCubeCoord(ring[j]));
            if (PlayerDistanceToTargetTile < maxD)
            {
                ringCount = j;
                maxD = PlayerDistanceToTargetTile;
            }

        }



        /// target ist das Tile, welches die kürzeste Distanz zum Player besitzt 

        target = HexGridUtil.AxialToCubeCoord(ring[ringCount]);

        //target = possibleTargets[0];
        List<Vector3Int> Shape = RotateTowards(HexGridUtil.CubeToAxialCoord(target), PlayerManager.Instance.Players[counter].CoordinatePosition, HexGridUtil.AxialToCubeCoord(SpreadShape.Coordinates));
      
        foreach (Vector3Int Coord in Shape)
        {

            if (GridManager.Instance.Grid.Keys.Contains(HexGridUtil.CubeToAxialCoord(Coord)))
            {
                if (GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(Coord)].currentGridState != GridManager.Instance.gS_Negative && GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(Coord)].currentGridState != GridManager.Instance.gS_Boss&&  GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(Coord)].currentGridState != GridManager.Instance.gS_BossNegative && GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(Coord)].AxialCoordinate != PlayerManager.Instance.Players[counter].CoordinatePosition)
                {
                    GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(Coord)].ChangeCurrentState(GridManager.Instance.gS_Negative);
                    GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(target)].ChangeCurrentState(GridManager.Instance.gS_Negative);
                }
            }
        }
        return false;
    }

    public List<Vector3Int> RotateTowards(Vector2Int target, Vector2Int playerPosition, List<Vector3Int> shapeToRotate)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();
        List<Vector3Int> targetNeighbors = HexGridUtil.CubeNeighbors(HexGridUtil.AxialToCubeCoord(target));

        foreach (Vector3Int neighbor in targetNeighbors)
        {
            if (GridManager.Instance.Grid.Keys.Contains(HexGridUtil.CubeToAxialCoord(neighbor)))
            {
                neighbors.Add(neighbor);
            }
        }



        List<Vector3Int> rotatedShape = shapeToRotate;
        //List<int> distances = new List<int>();
        int maxDist = int.MaxValue;
        for (int i = 0; i < neighbors.Count; i++)
        {
            int dist = HexGridUtil.CubeDistance(neighbors[i], HexGridUtil.AxialToCubeCoord(PlayerManager.Instance.Players[counter].CoordinatePosition));

            if (dist < maxDist)
            {
                RotationCount = i;
                maxDist = dist;
            }
        }
        rotatedShape = HexGridUtil.RotateRangeClockwise(HexGridUtil.AxialToCubeCoord(target), rotatedShape, RotationCount);
        return rotatedShape;

    }

    public override bool TargetTiles(Vector3Int origin, out List<Vector3Int> targets, Vector2Int closestPlayerPosition)
    {
        throw new System.NotImplementedException();
    }
}
