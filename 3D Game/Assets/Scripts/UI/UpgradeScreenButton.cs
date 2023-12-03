using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeScreenButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] UpgradeManager AbilityUpgradeObj;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!AbilityUpgradeObj.gameObject.activeSelf)
        {
            AbilityUpgradeObj.gameObject.SetActive(true);
        }
        else
        {
            AbilityUpgradeObj.gameObject.SetActive(false);
        }
    }
}
