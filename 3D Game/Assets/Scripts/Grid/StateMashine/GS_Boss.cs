using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="GS_Boss",menuName ="GridStates/GS_Boss")]
public class GS_Boss : GridState
{ 
    public override GridState CurrentState()
    {
        return this;
    }

    public override void EnterState(GridTile parent)
    {
        parent.GetComponent<RessourceVisuals>().SpawnEnemyMass();
        foreach (GridTile neighbor in parent.myNeighbors)
        {
            if (neighbor.currentGridState.StateValue() == -1)
            {
                neighbor.GetComponent<RessourceVisuals>().SpawnEnemyMass();
            }
        }
    }

    public override void ExitState(GridTile parent)
    {
        //throw new System.NotImplementedException();
    }

    public override void PlayerEnters(GridTile parent)
    {
        //throw new System.NotImplementedException();
    }

    public override int StateValue()
    {
        return -10;
    }
}
