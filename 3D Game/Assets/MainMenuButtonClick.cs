using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class MainMenuButtonClick : MonoBehaviour, IPointerClickHandler
{
    public AudioData ButtonClick;
    public AudioMixerGroup soundEffect;
    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySoundAtLocation(ButtonClick, soundEffect, null, true);
    }
}
