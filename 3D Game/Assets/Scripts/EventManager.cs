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
}
