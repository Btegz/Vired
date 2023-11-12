using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static PofIManager;

public class EventButton : MonoBehaviour
{
    public Button Option1;
    public Button Option2;
    // public PofIManager PofIManager;
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
    //public Button NewResource, SkillPoints, ResourceSwitch, NewAbility, PositionSwitch, Movement;

    //public Button[] PofIEvents = new Button[5];
    public PofIManager PofI;


    private void Start()
    {

        if (CompareTag("Event1"))
        {
            Option1.onClick.AddListener(() => PofI.PofIEvent(PofI.pofiEvent1));
            gameObject.transform.GetChild(PofI.pofi1).gameObject.SetActive(true);
        }

        if (CompareTag("Event2"))
        {
            Option2.onClick.AddListener(() => PofI.PofIEvent(PofI.pofiEvent2));
            gameObject.transform.GetChild(PofI.pofi2).gameObject.SetActive(true);
        }
    }



}