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

    public delegate void AbilityButtonDelegate(Ability ability);
    public static event AbilityButtonDelegate OnAbilityButtonEvent;

    public static void OnAbilityButtonClicked(Ability ability)
    {
        Debug.Log("I INVOKE ABILITY BUTTON CLICKED");
        OnAbilityButtonEvent?.Invoke(ability);
    }

    public delegate void LoadOutAbilityChosen(AbilityLoadoutButton abilityButton);
    public static event LoadOutAbilityChosen OnAbilityChosenEvent;

    public static void OnAbilityChosen(AbilityLoadoutButton abilityLoadoutButton)
    {
        OnAbilityChosenEvent?.Invoke(abilityLoadoutButton);
    }

    public delegate void LoadOutAbilityChoiceRemoveDelegate(AbilityLoadoutButton abilityButton);
    public static event LoadOutAbilityChoiceRemoveDelegate LoadOutAbilityChoiseRemoveEvent;

    public static void OnLoadoutAbilityChoiceRemove(AbilityLoadoutButton abilityLoadoutButton)
    {
        LoadOutAbilityChoiseRemoveEvent?.Invoke(abilityLoadoutButton);
    }

    public delegate void SelectPlayerDelegate(Player player);
    public static event SelectPlayerDelegate OnSelectPlayerEvent;

    public static void OnSelectPlayer(Player player)
    {
        OnSelectPlayerEvent?.Invoke(player);
    }
    
    public delegate void Movement(Player player);
    public static event Movement OnMoveEvent;

    public static void OnMove(Player player)
    {
        OnMoveEvent?.Invoke(player);
    }

    public delegate void UpgradeAbilitySelectDelegate(Ability ability);
    public static event UpgradeAbilitySelectDelegate UpgradeAbilitySelectEvent;

    public static void OnUpgradeAbilitySelect(Ability ability)
    {
        UpgradeAbilitySelectEvent?.Invoke(ability);
    }

    public delegate void AbilityChangeDelegate(Dictionary<Vector2Int, UpgradeGridHex> Grid, Ability ability);
    public static event AbilityChangeDelegate AbilityChangeEvent;

    public static void OnAbilityChange(Dictionary<Vector2Int, UpgradeGridHex> Grid, Ability ability)
    {
        AbilityChangeEvent?.Invoke(Grid, ability);
    }

    public delegate void PhaseChangeDelegate();
    public static event PhaseChangeDelegate PhaseChangeEvent;

    public static void OnPhaseChange()
    {
        PhaseChangeEvent?.Invoke();
    }

}
