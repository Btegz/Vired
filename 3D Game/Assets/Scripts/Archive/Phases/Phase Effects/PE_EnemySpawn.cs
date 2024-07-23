using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Phase Effect responsible to Spawn Enemies
/// </summary>
[CreateAssetMenu]
public class PE_EnemySpawn : PhaseEffect
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
            //    for(int i = 0; i< amount; i++)
            //    {
            //        Enemy newEnemy = Instantiate(enemyPrefab);
            //        newEnemy.Setup(enemySOs[Random.Range(0, enemySOs.Count)]);
            //        GridTile randomLocation = gridManager.PickRandomTile();
            //        while (randomLocation.currentGridState == gridManager.gS_Enemy || randomLocation.currentGridState == gridManager.gS_Boss || randomLocation.AxialCoordinate == PlayerManager.Instance.playerPosition)
            //        {
            //            randomLocation = gridManager.PickRandomTile();
            //        }
            //        randomLocation.ChangeCurrentState(gridManager.gS_Enemy);
            //        newEnemy.transform.parent = randomLocation.transform;
            //        newEnemy.transform.position = randomLocation.transform.position;
            //    }


            for (int i = 0; i < SpreadBehaviours.Count; i++)
            {
                Vector3Int target;
                if (SpreadBehaviours[i].TargetTile(Vector3Int.zero, out target, PlayerManager.Instance.playerPosition))
                {
                    Enemy enemy = Instantiate(enemyPrefab);
                    GridTile targetLocation = GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(target)];
                    enemy.Setup(/*enemySOs[Random.Range(0, enemySOs.Count)], */targetLocation);
                    targetLocation.ChangeCurrentState(gridManager.gS_Enemy);
                    enemy.transform.parent = targetLocation.transform;
                    enemy.transform.position = targetLocation.transform.position;
                }
            }
        }
    }
}
