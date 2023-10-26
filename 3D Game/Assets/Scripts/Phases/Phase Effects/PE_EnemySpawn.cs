using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Phase Effect responsible to Spawn Enemies
/// </summary>
[CreateAssetMenu]
public class PE_EnemySpawn : PhaseEffect
{
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
        if(turnCounter % everyXRounds == 0)
        {
            for(int i = 0; i< amount; i++)
            {
                Enemy newEnemy = Instantiate(enemyPrefab);
                newEnemy.Setup(enemySOs[Random.Range(0, enemySOs.Count)]);
                GridTile randomLocation = gridManager.PickRandomTile();
                while (randomLocation.currentGridState == gridManager.gS_Enemy || randomLocation.currentGridState == gridManager.gS_Boss || randomLocation.AxialCoordinate == HexGridUtil.CubeToAxialCoord(PlayerManager.Instance.playerPosition))
                {
                    randomLocation = gridManager.PickRandomTile();
                }
                randomLocation.ChangeCurrentState(gridManager.gS_Enemy);
                newEnemy.transform.parent = randomLocation.transform;
                newEnemy.transform.position = randomLocation.transform.position;
            }
        }
    }
}
