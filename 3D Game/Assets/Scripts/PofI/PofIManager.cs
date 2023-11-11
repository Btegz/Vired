using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PofIManager : MonoBehaviour
{
    public EventButton event1;
    [SerializeField] int pofi1;
    [SerializeField] int pofi2;
    private PofI pofiEvent1;
    private PofI pofiEvent2;

    public enum PofI
    {
        PofI_NewResource,
        PofI_SkillPoints,
        PofI_ResourceSwitch,
        PofI_NewAbility
    };

    private void OnEnable()
    {
        randomPofI();
    }

    private void Update()
    {
        if(event1.PofIEvent1 == true && event1.PofIEvent2 == false)
        {
            PofIEvent(pofiEvent1);
        }

        else if(event1.PofIEvent2 == true && event1.PofIEvent1== false)
        {   
            PofIEvent(pofiEvent2);
        }
    }


    private void randomPofI()
    {
        int pofi1 = (int)Random.Range(0, 3);
        PofI pofiEvent1 = (PofI)pofi1;

        int pofi2 = (int)Random.Range(0, 3);
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
                Debug.Log("ResourceSwitch");
                break;

            case PofI.PofI_SkillPoints:
                PlayerManager.Instance.SkillPoints++;
                break;

        }

    }
}
