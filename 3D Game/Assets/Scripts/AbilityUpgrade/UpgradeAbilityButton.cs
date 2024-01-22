using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeAbilityButton : AbilityButton, IPointerClickHandler
{
    Image ButtonImage;

    [SerializeField]UpgradeHexGrid upgradeHexGrid;

    // Start is called before the first frame update
    void Start()
    {
        ButtonImage = GetComponent<Image>();
        EventManager.OnSelectPlayerEvent += SelectAbility;
        if(PlayerManager.Instance.selectedPlayer != null)
        {
            SelectAbility(PlayerManager.Instance.selectedPlayer);
        }
        else
        {
            ButtonImage.sprite = emptyAbilitySlotSprite;
        }
    }

    private void OnDestroy()
    {
        EventManager.OnSelectPlayerEvent -= SelectAbility;
    }

    private void SelectAbility(Player player)
    {
        if(player.AbilityInventory.Count > index)
        {
            this.ability = player.AbilityInventory[index];

            MakeAbilityToGrid();
            
            //CorrectResourceInAbility();
        }
        else
        {
            this.ability = null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(this.ability != null)
        {
            //upgradeHexGrid.LoadAbility(this.ability);
        }
    }
}
