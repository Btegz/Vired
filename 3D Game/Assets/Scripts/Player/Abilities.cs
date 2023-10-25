using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    private string name;
    public GridTile gridTile;

    public Ability ability;


    public void AttachedToPlayer()
    {

    }

    public void Rotation()
    {

    }


    public void UsingEffect(Effect effect)
    {

        switch (effect)
        {
            case Effect.Positive:
                switch (gridTile.currentGridState)
                {
                    case GS_positive:
                        gridTile.ChangeCurrentState(GridManager.Instance.gS_Positive);
                        break;
                    //   NotImplementedException;
                    case GS_negative:
                        gridTile.ChangeCurrentState(GridManager.Instance.gS_Neutral);
                        break;

                    case GS_neutral:
                        gridTile.ChangeCurrentState(GridManager.Instance.gS_Positive);
                        break;

                        //upgrade Feld um eins (Negativ zu neutral zu positiv)
                }

                break;



            case Effect.Negative:
            // Schade einem Enemy 
            // Schadet man neutralen Feldern?


            case Effect.Neutral:
            // nichts 



            case Effect.Movement:
                break;
                // while ?
                // bewege Player auf dieses Feld (Feld = Landefeld??)
        }


    }

    // Kosten 
    // Ressource (Schere/Stein/Papier System)
}
