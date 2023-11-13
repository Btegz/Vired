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

    public delegate void AbilityButtonDelegate(int index);
    public static event AbilityButtonDelegate OnAbilityButtonEvent;

    public static void OnAbilityButtonClicked(int index)
    {
        OnAbilityButtonEvent?.Invoke(index);
    }

    public delegate void LoadOutAbilityChosen(AbilityLoadoutButton abilityButton);
    public static event LoadOutAbilityChosen OnAbilityChosenEvent;

    public static void OnAbilityChosen(AbilityLoadoutButton abilityLoadoutButton)
    {
        OnAbilityChosenEvent?.Invoke(abilityLoadoutButton);
    }

    public delegate void SelectPlayerDelegate(Player player);
    public static event SelectPlayerDelegate OnSelectPlayerEvent;

    public static void OnSelectPlayer(Player player)
    {
        OnSelectPlayerEvent?.Invoke(player);
    }
}
