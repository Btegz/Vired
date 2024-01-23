using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Audio;

public class MenuButtonClick : MonoBehaviour, IPointerClickHandler
{
    public AudioMixerGroup soundEffect;
    public AudioData UIClick;
    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySoundAtLocation(UIClick, soundEffect, null);
    }
}
