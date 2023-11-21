using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_PlayerABLInventory : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public GridLayoutGroup InventoryArea;

    [SerializeField] public Player player;

    [SerializeField] AbilityLoadoutButton loadoutButtonPrefab;

    [SerializeField] Image PlayerPortrait;

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
}
