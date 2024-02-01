using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static PofIManager;
using UnityEngine.Audio;

public class EventButton : MonoBehaviour, IPointerEnterHandler
{
    public Button Option1;
    public Button Option2;
    public PofIManager PofI;
    public AudioData hoverOver;
    public AudioMixerGroup soundEffect;

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySoundAtLocation(hoverOver, soundEffect, null, true);
    }

    private void Start()
    {

        if (CompareTag("Event1"))
        {
            Option1.onClick.AddListener(() => PofI.PofIEvent(PofI.pofiEvent1));
            gameObject.transform.GetChild(PofI.pofi1).gameObject.SetActive(true);
        }

        else if (CompareTag("Event2"))
        {
            Option2.onClick.AddListener(() => PofI.PofIEvent(PofI.pofiEvent2));
            gameObject.transform.GetChild(PofI.pofi2).gameObject.SetActive(true);
        }
    }



}