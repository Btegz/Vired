using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EventButton : MonoBehaviour
{
    public Button Option1;
    public Button Option2;
    public PofIManager PofIManager;
    //public bool PofIEvent1 = false;
    //public bool PofIEvent2 = false;
    /* public void OnPointerClick(PointerEventData eventData)
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
     }*/

    private void Start()
    {
        Option1.onClick.AddListener(() => PofIManager.PofIEvent(PofIManager.pofiEvent1));
        Option2.onClick.AddListener(() => PofIManager.PofIEvent(PofIManager.pofiEvent2));
    }

}