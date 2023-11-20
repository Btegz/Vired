using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class Boss : MonoBehaviour
{


    [SerializeField] List<Vector3Int> BossReachableTiles;
    [SerializeField] List<Spreadbehaviours> BossSpread;
    [SerializeField] int Health;
    [SerializeField] int SkillPoints;
    [SerializeField] int AbilityLoadout;
    public BossManager bossmanager;
    Vector2Int location;

    private void Start()
    {
      //  EventManager.OnEndTurnEvent += BossNeighbors;
        Instantiate(bossmanager.BossList[0]);
        
    }

    public void Spawn(Vector2Int location)
    {
        
        GridManager.Instance.Boss = Instantiate(GridManager.Instance.BossPrefab);
        GridManager.Instance.Boss.Setup(GridManager.Instance.BossEnemySO, GridManager.Instance.Grid[location]);
        GridManager.Instance.Boss.transform.parent = GridManager.Instance.Grid[location].transform;
        GridManager.Instance.Boss.transform.position = GridManager.Instance.Grid[location].transform.position;
        GridManager.Instance.Grid[GridManager.Instance.BossSpawn].ChangeCurrentState(GridManager.Instance.gS_Boss);
    }

    public void BossNeighbors(Vector2Int location)
    {
        BossReachableTiles = HexGridUtil.CoordinatesReachable(HexGridUtil.AxialToCubeCoord(location), 4, HexGridUtil.AxialToCubeCoord(GridManager.Instance.Grid.Keys.ToList<Vector2Int>()));
        BossReachableTiles.Remove(HexGridUtil.AxialToCubeCoord(location));

        foreach (Vector3Int neighbor in BossReachableTiles)
        {
            if (GridManager.Instance.Grid.ContainsKey(HexGridUtil.CubeToAxialCoord(neighbor)))
                GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(neighbor)].ChangeCurrentState(GridManager.Instance.gS_BossNegative);
        }
    }

  



}
