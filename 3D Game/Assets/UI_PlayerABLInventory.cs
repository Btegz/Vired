using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_PlayerABLInventory : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IDropHandler
{
    [SerializeField] public HorizontalLayoutGroup InventoryArea;

    [SerializeField] Player player;

    [SerializeField] AbilityLoadoutButton loadoutButtonPrefab;

    public void Setup(Player player)
    {
        this.player = player;
        //EventManager.OnAbilityChosenEvent += AddAbility;

        //foreach(Ability ability in player.AbilityInventory)
        //{
        //    AbilityLoadoutButton abilityLoadout = Instantiate(loadoutButtonPrefab,transform);
        //    abilityLoadout.Setup(ability);
        //}
    }

    public void AddAbility(AbilityLoadoutButton abilityLoadOUtButton)
    {
        player.AbilityInventory.Add(abilityLoadOUtButton.ability);

        abilityLoadOUtButton.transform.SetParent(InventoryArea.transform);
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

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if (eventData.pointerDrag.TryGetComponent<AbilityLoadoutButton>(out AbilityLoadoutButton abilityLoadOUtButton))
            {
                player.AbilityInventory.Add(abilityLoadOUtButton.ability);

                abilityLoadOUtButton.transform.SetParent(InventoryArea.transform); 
                EventManager.OnAbilityChosen(abilityLoadOUtButton);
            }
        }
            
    }
}
