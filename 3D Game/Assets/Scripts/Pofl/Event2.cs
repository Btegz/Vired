using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Event2 : MonoBehaviour, IPointerClickHandler
{
  public bool PofIEvent2 = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("enabled 2");
        PofIEvent2 = true;
    }
}

