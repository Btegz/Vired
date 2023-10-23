using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="GS_Positive",menuName ="GridStates/GS_Positive")]
public class GS_positive : GridState
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
        //throw new System.NotImplementedException();
    }

    public override void PlayerEnters(GridTile parent)
    {
        // Give player a Ressource
    }

    public override int StateValue()
    {
        return 1;
    }
}
