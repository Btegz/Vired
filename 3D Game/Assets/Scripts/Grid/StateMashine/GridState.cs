using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GridState
{
    public abstract void EnterState(GridCell parent);

    public abstract void ExitState(GridCell parent);

    public abstract GridState CurrentState();

    public abstract void PlayerEnters(GridCell parent);
}
