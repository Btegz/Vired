using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName ="GS_BossNegative",menuName ="GridStates/GS_BossNegative")]
public class GS_BossNegative : GridState
{ 
    public override GridState CurrentState()
    {
        return this;
    }

    public override void EnterState(GridTile parent)
    {
        parent.GetComponent<RessourceVisuals>().CleanUpKlopse();
        parent.GetComponent<RessourceVisuals>().SpawnEnemyMass();

        //parent.meshRenderer.material = parent.gridTileSO.negativeMaterial;
        parent.transform.DOComplete();
        //parent.transform.DOPunchRotation(Vector3.one * TweenScale, .5f);
    }

    public override void ExitState(GridTile parent)
    {
        parent.GetComponent<RessourceVisuals>().DestroyEnemyMasses();
    }

    public override void PlayerEnters(GridTile parent)
    {
        //throw new System.NotImplementedException();
    }

    public override int StateValue()
    {
        return -1;
    }

 
}
