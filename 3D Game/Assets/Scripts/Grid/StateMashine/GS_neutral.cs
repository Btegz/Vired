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
    }

    public override void ExitState(GridTile parent)
    {
        //throw new System.NotImplementedException();
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
