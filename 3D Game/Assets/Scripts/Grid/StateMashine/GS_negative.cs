using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="GS_Negative",menuName ="GridStates/GS_Negative")]
public class GS_negative : GridState
{

    public override GridState CurrentState()
    {
        return this;
    }

    public override void EnterState(GridTile parent)
    {
        parent.meshRenderer.material = parent.gridTileSO.negativeMaterial;
        parent.transform.DOComplete();
        parent.GetComponent<RessourceVisuals>().SpawnEnemyMass();

        parent.transform.DOPunchRotation(Vector3.one*TweenScale,.5f);

    }

    public override void ExitState(GridTile parent)
    {
        parent.GetComponent<RessourceVisuals>().DestroyEnemyMasses();
    }

    public override void PlayerEnters(GridTile parent)
    {
        //tirgger Ressource loss for player
    }

    public override int StateValue()
    {
        return -1;
    }
}
