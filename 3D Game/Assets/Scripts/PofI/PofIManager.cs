using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PofIManager : MonoBehaviour
{
    public EventButton event1;
    [HideInInspector][SerializeField] public int pofi1;
    [HideInInspector][SerializeField] public int pofi2;
    public PofI pofiEvent1;
    public PofI pofiEvent2;
   
    public Button NewResource,SkillPoints,ResourceSwitch,NewAbility,PositionSwitch,Movement;

    public Button[] PofIEvents = new Button[5];
    public enum PofI
    {
        PofI_NewResource,
        PofI_SkillPoints,
        PofI_ResourceSwitch,
        PofI_NewAbility,
        PofI_PositionSwitch, 
        PofI_MovementPoints,
    };

    

    private void Start()
    {
        
        
            Button[] PofIEvents =
            {
                NewResource,
                SkillPoints,
                ResourceSwitch,
                NewAbility,
                PositionSwitch,
                Movement,
            };
    }
    /*   private void Update()
       {
           if(event1.PofIEvent1 == true && event1.PofIEvent2 == false)
           {
               PofIEvent(pofiEvent1);
           }

           else if(event1.PofIEvent2 == true && event1.PofIEvent1== false)
           {   
               PofIEvent(pofiEvent2);
           }
       }*/

    private void OnEnable()
    {
        randomPofI();
    }


    public void randomPofI()
    {
        Debug.Log("test");
        int pofi1 = (int)Random.Range(0, 5);
        PofI pofiEvent1 = (PofI)pofi1;

        int pofi2 = (int)Random.Range(0, 5);
        PofI pofiEvent2 = (PofI)pofi2;

        while (pofi1 == pofi2)
        {
            pofi2 = (int)Random.Range(0, 3);
            break;
        }

      
    }

    public void PofIEvent(PofI pofi)
    {
        switch (pofi)
        {
            case PofI.PofI_NewAbility:
               Debug.Log("NewAbility");
                break;

            case PofI.PofI_NewResource:
                Debug.Log("NewResource");
                break;

            case PofI.PofI_ResourceSwitch:
 
                Instantiate(ResourceSwitch, ResourceSwitch.transform.position,Quaternion.identity);
                break;

            case PofI.PofI_SkillPoints:
                PlayerManager.Instance.SkillPoints += 2;
                break;

            case PofI.PofI_PositionSwitch:
                break;

            case PofI.PofI_MovementPoints:
                break;

        }

    }
}
