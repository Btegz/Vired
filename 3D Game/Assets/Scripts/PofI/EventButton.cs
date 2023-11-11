using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventButton : MonoBehaviour, IPointerClickHandler
{
    public bool PofIEvent1 = false;
    public bool PofIEvent2 = false;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (CompareTag("Event1"))
        {
            PofIEvent1 = true;
        }

        if (CompareTag("Event2"))
        {
            PofIEvent2 = true;
        }
    }

    public void Update()
    {
        PofIEvent1 = false;
        PofIEvent2 = false;
    }
}
