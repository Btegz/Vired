using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeAbilityButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] int index;

    Image ButtonImage;

    Ability abilitySelected;

    [SerializeField]UpgradeHexGrid upgradeHexGrid;

    // Start is called before the first frame update
    void Start()
    {
        ButtonImage = GetComponent<Image>();
        EventManager.OnSelectPlayerEvent += SelectAbility;
    }

    private void OnDestroy()
    {
        EventManager.OnSelectPlayerEvent -= SelectAbility;
    }

    private void SelectAbility(Player player)
    {
        try
        {
            Ability ability = player.AbilityInventory[index];
            ButtonImage.sprite = player.AbilityInventory[index].AbilityUISprite;
            abilitySelected = player.AbilityInventory[index];
        }
        catch
        {
            abilitySelected = null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(abilitySelected != null)
        {
            upgradeHexGrid.LoadAbility(abilitySelected);
        }
    }
}
