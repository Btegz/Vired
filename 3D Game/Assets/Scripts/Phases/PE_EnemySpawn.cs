using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    public override void triggerPhaseEffect(GridManager gridManager)
    {
        //throw new System.NotImplementedException("i still need to handle Enemies Spawning.");

        Enemy newEnemy = Instantiate(enemyPrefab);
        newEnemy.Setup(enemySOs[Random.Range(0, enemySOs.Count)]);
        GridCell randomLocation = gridManager.PickRandomTile();
        newEnemy.transform.parent = randomLocation.transform;
        newEnemy.transform.position = randomLocation.transform.position;
    }
}
