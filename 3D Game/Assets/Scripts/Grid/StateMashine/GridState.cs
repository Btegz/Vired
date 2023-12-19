using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// Abstract Class for the states of the Statemashine of the Tile.
/// </summary>
public abstract class GridState : ScriptableObject
{
    public float TweenScale;

    [SerializeField] public int NegativeSpreadCost;


    /// <summary>
    /// Called when entering the State
    /// </summary>
    /// <param name="parent">Provide The Tile for Reference.</param>
    public abstract void EnterState(GridTile parent);

    /// <summary>
    /// Called when exiting the State
    /// propably for some cleanupstuff mby at some point LUL
    /// </summary>
    /// <param name="parent">Provide The Tile for Reference.</param>
    public abstract void ExitState(GridTile parent);

    public abstract GridState CurrentState();

    /// <summary>
    /// Called by Player, when moving. Handles Ressource gaining and stuff for player.
    /// This might not be used not sure yet.
    /// </summary>
    /// <param name="parent">Provide The Tile for Reference.</param>
    public abstract void PlayerEnters(GridTile parent);

    /// <summary>
    /// puts the Gridstates to an integer value where:
    /// <br>4 = Pofi </br>
    /// <br>1 = positive </br>
    /// <br>0 = Neutral </br>
    /// <br>-1 = Negative and BossNegative </br>
    /// <br>-2 = Enemy </br>
    /// <br>-10 = Boss </br>
    /// </summary>
    /// <returns></returns>
    public abstract int StateValue();
}
