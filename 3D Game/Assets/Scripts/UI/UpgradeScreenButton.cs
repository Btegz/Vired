using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class UpgradeScreenButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    [SerializeField] UpgradeManager AbilityUpgradeObj;
    public AudioData OpenUpgradeMenu;
    public AudioData ButtonHover;
    public AudioMixerGroup soundEffect;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (PlayerManager.Instance.abilityActivated)
        {
            return;
        }
        if (!AbilityUpgradeObj.gameObject.activeSelf)
        {
            AbilityUpgradeObj.gameObject.SetActive(true);
            EventManager.OnAbilityUpgrade(ButtonState.inUpgrade);
            CameraRotation.Instance.AbilityLoadOut();
            AudioManager.Instance.PlaySoundAtLocation(OpenUpgradeMenu, soundEffect, null);  
        }

        else
        {
            AbilityUpgradeObj.gameObject.SetActive(false);
            AbilityUpgradeObj.GetComponentInChildren<UpgradeHexGrid>().ConfirmAbilityUpgrade();
            EventManager.OnAbilityUpgrade(ButtonState.inMainScene);

            CameraRotation.Instance.Worldcam.Priority = PlayerPrefs.GetInt("World");
            CameraRotation.Instance.AbilityUpgradeCam.Priority = PlayerPrefs.GetInt("AbilityLoadOut");
            CameraRotation.Instance.TopDownCam.Priority = PlayerPrefs.GetInt("Topdown");
        }


    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySoundAtLocation(ButtonHover, soundEffect, null);
    }
}
