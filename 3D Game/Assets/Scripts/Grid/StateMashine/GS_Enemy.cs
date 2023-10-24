using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="GS_Enemy",menuName ="GridStates/GS_Enemy")]
public class GS_Enemy : GridState
{
    public override GridState CurrentState()
    {
        return this;
    }

    public override void EnterState(GridTile parent)
    {
        parent.meshRenderer.material = parent.gridTileSO.negativeMaterial;
    }

    public override void ExitState(GridTile parent)
    {
        throw new System.NotImplementedException();
    }

    public override void PlayerEnters(GridTile parent)
    {
        Debug.Log("ITS TIME YOU PAY");
    }

    public override int StateValue()
    {
        return -2;
    }
}
