using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Audio; 

public class AbilityMenuClick : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    public AudioMixerGroup soundEffect;
    public AudioData ButtonClick;
    public AudioData ButtonHover;

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySoundAtLocation(ButtonClick, soundEffect, null, false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
              AudioManager.Instance.PlaySoundAtLocation(ButtonHover, soundEffect, null, false);
    }
}
