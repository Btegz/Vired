using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PE_MiniEnemySpawn : PhaseEffect
{

    [SerializeField] public List<Spreadbehaviours> SpreadBehaviours;

    [SerializeField] private List<EnemySO> enemySOs;

    public List<EnemySO> myEnemySoList
    {
        get { return enemySOs; }
        set { enemySOs = value; }
    }

    [SerializeField] private Enemy enemyPrefab;

    public Enemy myEnemyPrefab
    {
        get { return enemyPrefab; }
        set { enemyPrefab = value; }
    }

    [SerializeField] private int amount;

    public int MyAmount
    {
        get { return amount; }
        set { amount = value; }
    }

    public override void TriggerPhaseEffect(int turnCounter, GridManager gridManager)
    {
        if (turnCounter % everyXRounds == 0)
        {
                 for (int i = 0; i < SpreadBehaviours.Count; i++)
            {
                Vector3Int target;
                if (SpreadBehaviours[i].TargetTile(Vector3Int.zero, out target, PlayerManager.Instance.playerPosition))
                {
                    Enemy enemy1 = Instantiate(enemyPrefab);
                    GridTile targetLocation = GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(target)];
                    enemy1.Setup(enemySOs[Random.Range(0, enemySOs.Count)], targetLocation);
                    enemy1.currentHealth = 1;
                    targetLocation.ChangeCurrentState(gridManager.gS_Enemy);
                    enemy1.transform.parent = targetLocation.transform;
                    enemy1.transform.position = targetLocation.transform.position;
                }
            }
        }
    }
}
