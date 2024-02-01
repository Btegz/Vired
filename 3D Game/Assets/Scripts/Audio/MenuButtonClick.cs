using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Audio;

public class MenuButtonClick : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    public AudioMixerGroup soundEffect;
    public AudioData ButtonClick;
    public AudioData ButtonHover;
   
    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySoundAtLocation(ButtonClick, soundEffect, null,true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        AudioManager.Instance.PlaySoundAtLocation(ButtonHover, soundEffect, null, true);
    }
}
