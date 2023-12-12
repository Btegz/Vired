using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="GS_Neutral",menuName ="GridStates/GS_Neutral")]
public class GS_neutral : GridState
{

    public override GridState CurrentState()
    {
        return this;
    }

    public override void EnterState(GridTile parent)
    {
        parent.meshRenderer.material = parent.gridTileSO.neutralMaterial;
        parent.transform.DOComplete();
        parent.transform.DOPunchRotation(Vector3.one * TweenScale, .5f);
    }

    public override void ExitState(GridTile parent)
    {
    }

    public override void PlayerEnters(GridTile parent)
    {
        // do nothing
    }

    public override int StateValue()
    {
        return 0;
    }
}
