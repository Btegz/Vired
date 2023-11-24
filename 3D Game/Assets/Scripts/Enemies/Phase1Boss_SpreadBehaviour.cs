using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[CreateAssetMenu(fileName ="BossSpread")]
public class Phase1Boss_SpreadBehaviour : Spreadbehaviours
{    Dictionary<float, Player> sortedDict;
    public float rotationFactor;

    public HexShape SpreadShape;

    public override bool TargetTile(Vector3Int enemyPosition, out Vector3Int target, Vector2Int playerPosition)
    {
        List<Vector3Int> ring = HexGridUtil.Ring(enemyPosition, 4, GridManager.Instance.Grid.Keys.ToList()); ;
        Dictionary<float, Player> PlayerDistances =new Dictionary<float, Player>();
        Dictionary<float, Vector2Int> targetTile = new Dictionary<float, Vector2Int>();
        

        /// Player Distanzen zum Boss werden in einer Liste gespeichert 
        for (int i = 0; i < PlayerManager.Instance.Players.Count; i++)
        {
            float PlayerDistanceToBoss = HexGridUtil.CubeDistance(HexGridUtil.AxialToCubeCoord(PlayerManager.Instance.Players[i].CoordinatePosition), enemyPosition);
            PlayerDistances.Add(PlayerDistanceToBoss, PlayerManager.Instance.Players[i]);
        }

        sortedDict = PlayerDistances.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
        
        /// Player mit der kürzesten Distanz zum Boss wird genutzt
        /// Von diesem Player werden die Distanzen zum Ring außerhalb der Boss negative Spread Area in einer Liste gespeichert
        for (int i=0; i<ring.Count; i++)
        {
            float PlayerDistanceToTargetTile = HexGridUtil.CubeDistance(HexGridUtil.AxialToCubeCoord(sortedDict[0].CoordinatePosition), ring[i]);
            targetTile.Add(PlayerDistanceToTargetTile, PlayerDistances[0].CoordinatePosition);
        }

        var sortedTargetTileDict = targetTile.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

        /// target ist das Tile, welches die kürzeste Distanz zum Player besitzt 
        target = HexGridUtil.AxialToCubeCoord(sortedTargetTileDict[0]);

       
        List<Vector3Int> Shape = RotateTowards(HexGridUtil.CubeToAxialCoord(target), sortedDict[0].CoordinatePosition, HexGridUtil.AxialToCubeCoord(SpreadShape.Coordinates));
        List<Vector3Int> WorldShapeCoordinates = new List<Vector3Int>();

        foreach (Vector3Int coord in Shape)
        {
            WorldShapeCoordinates.Add(HexGridUtil.CubeAdd(target, coord));
            foreach (Vector3Int worldCoord in WorldShapeCoordinates)
            {
                GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(worldCoord)].currentGridState = GridManager.Instance.gS_BossNegative;
                
            }
        }

        return false;
    }

    public List<Vector3Int> RotateTowards(Vector2Int target, Vector2Int playerPosition, List<Vector3Int> shapeToRotate)
    {
        // target alle neighbors
        // je nachdem welcher es ist i * 60°
        // welcher ist i=0

        List<Vector3Int> targetNeighbors = HexGridUtil.CubeNeighbors(HexGridUtil.AxialToCubeCoord(target));
        List<Vector3Int> rotatedShape = shapeToRotate;

        //List<int> distances = new List<int>();
        int rotationAmount = 0;
        int maxDist = int.MaxValue;
        for (int i = 0; i < targetNeighbors.Count; i++)
        {
            int dist = HexGridUtil.CubeDistance(targetNeighbors[i], HexGridUtil.AxialToCubeCoord(playerPosition));
            if (dist < maxDist)
            {
                rotatedShape= HexGridUtil.RotateRangeClockwise(HexGridUtil.AxialToCubeCoord(target), rotatedShape, 1);
                rotationAmount++;
                maxDist = dist;
            }
            else
            {
                rotatedShape = HexGridUtil.RotateRangeCounterClockwise(HexGridUtil.AxialToCubeCoord(target), rotatedShape, 1);
                rotationAmount--; 
                break;
            }
        }
        return rotatedShape;
        
    }


    
    

  
}
