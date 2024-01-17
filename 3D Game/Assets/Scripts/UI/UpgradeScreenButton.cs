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
            EventManager.OnAbilityUpgrade(ButtonState.inUpgrade);
            CameraRotation.Instance.AbilityLoadOut();
           
        }
        else
        {
            AbilityUpgradeObj.gameObject.SetActive(false);
            AbilityUpgradeObj.GetComponentInChildren<UpgradeHexGrid>().ConfirmAbilityUpgrade();
            EventManager.OnAbilityUpgrade(ButtonState.inMainScene);

            CameraRotation.Instance.Worldcam.Priority = PlayerPrefs.GetInt("World");
            CameraRotation.Instance.AbilityLoadOutCam.Priority = PlayerPrefs.GetInt("AbilityLoadOut");
            CameraRotation.Instance.TopDownCam.Priority = PlayerPrefs.GetInt("Topdown");
        }

        
    }
}
