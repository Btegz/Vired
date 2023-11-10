using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Event1 : MonoBehaviour, IPointerClickHandler
{
    public bool PofIEvent1= false;
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("enabled 1");
        PofIEvent1 = true;
    }



}
