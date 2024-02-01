using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeScreenButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] UpgradeManager AbilityUpgradeObj;
    public AudioData OpenUpgradeMenu;
    public AudioData ButtonHover;
    public AudioMixerGroup soundEffect;
    public GameObject NextTurnBlock;
    public GameObject CamBlock;

    public Animator animator;

    
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
            AudioManager.Instance.PlaySoundAtLocation(OpenUpgradeMenu, soundEffect, null, true);
            NextTurnBlock.GetComponent<Image>().enabled = true;
            CamBlock.GetComponent<Image>().enabled = true;

            
        }

        else
        {
            AbilityUpgradeObj.gameObject.SetActive(false);
            AbilityUpgradeObj.GetComponentInChildren<UpgradeHexGrid>().ConfirmAbilityUpgrade();
            EventManager.OnAbilityUpgrade(ButtonState.inMainScene);

            CameraRotation.Instance.Worldcam.Priority = PlayerPrefs.GetInt("World");
            CameraRotation.Instance.AbilityUpgradeCam.Priority = PlayerPrefs.GetInt("AbilityLoadOut");
            CameraRotation.Instance.TopDownCam.Priority = PlayerPrefs.GetInt("Topdown");
            NextTurnBlock.GetComponent<Image>().enabled = false;
            CamBlock.GetComponent<Image>().enabled = false;

            

        }


    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySoundAtLocation(ButtonHover, soundEffect, null, true);
        animator.SetBool("isHovering", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
        animator.SetBool("isHovering", false);
    }
}
