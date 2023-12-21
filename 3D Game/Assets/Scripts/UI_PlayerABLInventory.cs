using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UI_PlayerABLInventory : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public GridLayoutGroup InventoryArea;

    [SerializeField] public Player player;

    [SerializeField] Image PlayerPortrait;

    [SerializeField] int Playerindex;
    
    [SerializeField] public List<AbilityLoadoutButton> AbilityLoadoutButtonInstances;


    private void Start()
    {
        try
        {
            player = PlayerManager.Instance.Players[Playerindex];
            PlayerPortrait.sprite = PlayerManager.Instance.PlayerSprites[Playerindex];
        }
        catch
        {
            Debug.LogWarning("Couldnt find a Player of my Index");
        }

        if(player != null )
        {
            EventManager.OnAbilityChosenEvent += AddAbility;
            EventManager.LoadOutAbilityChoiseRemoveEvent += RemoveAbility;
            foreach (Ability ability in player.AbilityInventory)
            {
                //ChosenAbilityList.Add(ability);
                //abilityCollection.Remove(ability);
                //AbilityLoadoutButton button = Instantiate(abloadoutButtonPrefab);
                //button.transform.SetParent(ini.InventoryArea.transform);
                //button.Setup(ability, ini.InventoryArea);
            }
        }        
    }

    private void RemoveAbility(AbilityLoadoutButton abilityButton)
    {
            player.AbilityInventory.Remove(abilityButton.ability);
            AbilityLoadoutButtonInstances.Remove(abilityButton);
    }

    public void Setup(Player player)
    {
        this.player = player;
        for(int i = 0; i < PlayerManager.Instance.Players.Count; i++)
        {
            if(player == PlayerManager.Instance.Players[i])
            {
                try
                {
                    PlayerPortrait.sprite = PlayerManager.Instance.PlayerSprites[i];
                }
                catch
                {
                    Debug.Log("Player cant find his Portrait");
                }
            }
        }

    }

    public void AddAbility(AbilityLoadoutButton abilityLoadOUtButton, Player player)
    {
        if(this.player == player)
        {
            player.AbilityInventory.Add(abilityLoadOUtButton.ability);
            AbilityLoadoutButtonInstances.Add(abilityLoadOUtButton);

            abilityLoadOUtButton.transform.SetParent(InventoryArea.transform);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //EventManager.OnAbilityChosenEvent -= AddAbility;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //EventManager.OnAbilityChosenEvent += AddAbility;
    }

    private void OnDestroy()
    {
        EventManager.OnAbilityChosenEvent -= AddAbility;
    }
}
