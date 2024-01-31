using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Audio;

public class MenuButtonClick : MonoBehaviour, IPointerClickHandler
{
    public AudioMixerGroup soundEffect;
    public AudioData ButtonClick;
   
    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySoundAtLocation(ButtonClick, soundEffect, null);
    }



}
