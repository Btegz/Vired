using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class Boss : MonoBehaviour
{


    [SerializeField] List<Vector3Int> BossReachableTiles;
    [SerializeField] List<Vector3Int> BossTiles;
    [SerializeField] List<Spreadbehaviours> BossSpread;
    [SerializeField] int SkillPoints;
    [SerializeField] int AbilityLoadout;

    public List<Vector2Int> location;
    [SerializeField] int SpawnRange;


    private void Start()
    {

        EventManager.OnEndTurnEvent += BossNeighbors;

    }

    public void Spawn(Vector2Int location, GameObject boss)
    {

        boss.transform.parent = GridManager.Instance.Grid[location].transform;
        boss.transform.position = GridManager.Instance.Grid[location].transform.position;
        GridManager.Instance.Grid[location].ChangeCurrentState(GridManager.Instance.gS_Boss);
    }

    public void BossNeighbors()
    {
        foreach (Vector2Int loc in location)
        {

            BossReachableTiles = HexGridUtil.CoordinatesReachable(HexGridUtil.AxialToCubeCoord(loc), SpawnRange, HexGridUtil.AxialToCubeCoord(GridManager.Instance.Grid.Keys.ToList<Vector2Int>()));
            BossReachableTiles.Remove(HexGridUtil.AxialToCubeCoord(loc));

            foreach (Vector3Int neighbor in BossReachableTiles)
            {
                if (GridManager.Instance.Grid.ContainsKey(HexGridUtil.CubeToAxialCoord(neighbor)))
                {
                    GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(neighbor)].ChangeCurrentState(GridManager.Instance.gS_BossNegative);
                }
            }
        }
    }

    public void BossDeath(Vector2Int location)
    {

        if (GetComponent<Enemy>().currentHealth == 0)
        {
            BossTiles = HexGridUtil.CoordinatesReachable(HexGridUtil.AxialToCubeCoord(location), SpawnRange, HexGridUtil.AxialToCubeCoord(GridManager.Instance.Grid.Keys.ToList<Vector2Int>()));
            BossTiles.Remove(HexGridUtil.AxialToCubeCoord(location));

            foreach (Vector3Int neighbor in BossTiles)
            {
                if (GridManager.Instance.Grid.ContainsKey(HexGridUtil.CubeToAxialCoord(neighbor)))
                {
                    Debug.Log("dead");
                    GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(neighbor)].ChangeCurrentState(GridManager.Instance.gS_Positive);

                }
            }

        }
    }
}
