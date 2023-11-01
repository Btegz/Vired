using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    public delegate void EndTurnDelegate();
    public static event EndTurnDelegate OnEndTurnEvent;

    public static void OnEndTurn()
    {
        OnEndTurnEvent?.Invoke();
    }

    public delegate void AbilityCast();
    public static event AbilityCast OnAbilityCastEvent;

    public static void OnAbilityCast()
    {
        OnAbilityCastEvent?.Invoke();
    }

    public delegate void ConfirmButtonDelegate();
    public static event ConfirmButtonDelegate OnConfirmButtonEvent;

    public static void OnConfirmButton()
    {
        OnConfirmButtonEvent?.Invoke();
    }
}
