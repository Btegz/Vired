using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeAbilityRange : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] UpgradeHexGrid upgradeHexGrid;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(upgradeHexGrid.loadedAbility != null)
        {
            upgradeHexGrid.loadedAbility.MyTierLevel++;
            upgradeHexGrid.LoadAbility(upgradeHexGrid.loadedAbility);
        }
    }
}
