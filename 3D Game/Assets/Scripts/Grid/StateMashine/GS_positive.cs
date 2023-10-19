using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GS_positive : GridState
{

    public override GridState CurrentState()
    {
        return this;
    }

    public override void EnterState(GridCell parent)
    {
        throw new System.NotImplementedException();
    }

    public override void ExitState(GridCell parent)
    {
        throw new System.NotImplementedException();
    }

    public override void PlayerEnters(GridCell parent)
    {
        // Give player a Ressource
    }
}
