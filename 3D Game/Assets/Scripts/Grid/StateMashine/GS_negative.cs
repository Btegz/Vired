using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GS_negative : GridState
{

    public override GridState CurrentState()
    {
        return this;
    }

    public override void EnterState(GridTile parent)
    {
        parent.meshRenderer.material = GridManager.Instance.negativeMaterial;
    }

    public override void ExitState(GridTile parent)
    {
        throw new System.NotImplementedException();
    }

    public override void PlayerEnters(GridTile parent)
    {
        //tirgger Ressource loss for player
    }
}
