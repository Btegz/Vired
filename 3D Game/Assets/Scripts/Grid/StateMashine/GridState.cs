using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GridState
{
    public abstract void EnterState(GridTile parent);

    public abstract void ExitState(GridTile parent);

    public abstract GridState CurrentState();

    public abstract void PlayerEnters(GridTile parent);
}
