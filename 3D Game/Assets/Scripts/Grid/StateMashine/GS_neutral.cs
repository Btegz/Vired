using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GS_neutral : GridState
{

    public override GridState CurrentState()
    {
        return this;
    }

    public override void EnterState(GridTile parent)
    {
        throw new System.NotImplementedException();
    }

    public override void ExitState(GridTile parent)
    {
        throw new System.NotImplementedException();
    }

    public override void PlayerEnters(GridTile parent)
    {
        // do nothing
    }
}
