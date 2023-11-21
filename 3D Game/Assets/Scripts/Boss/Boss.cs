using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class Boss : MonoBehaviour
{


    [SerializeField] List<Vector3Int> BossReachableTiles;
    [SerializeField] List<Spreadbehaviours> BossSpread;
    [SerializeField] int SkillPoints;
    [SerializeField] int AbilityLoadout;
    
    Vector2Int location;

    private void Start()
    {
       
      //  EventManager.OnEndTurnEvent += BossNeighbors;
        
    }

    public void Spawn(Vector2Int location, GameObject boss)
    {
        
        boss.transform.parent = GridManager.Instance.Grid[location].transform;
        boss.transform.position = GridManager.Instance.Grid[location].transform.position;
        GridManager.Instance.Grid[location].ChangeCurrentState(GridManager.Instance.gS_Boss);
    }

    public void BossNeighbors(Vector2Int location)
    {
        BossReachableTiles = HexGridUtil.CoordinatesReachable(HexGridUtil.AxialToCubeCoord(location), 3, HexGridUtil.AxialToCubeCoord(GridManager.Instance.Grid.Keys.ToList<Vector2Int>()));
        BossReachableTiles.Remove(HexGridUtil.AxialToCubeCoord(location));

        foreach (Vector3Int neighbor in BossReachableTiles)
        {
            if (GridManager.Instance.Grid.ContainsKey(HexGridUtil.CubeToAxialCoord(neighbor)))
                GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(neighbor)].ChangeCurrentState(GridManager.Instance.gS_BossNegative);
        }
    }

  



}
