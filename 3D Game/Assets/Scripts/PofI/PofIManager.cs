using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PofIManager : MonoBehaviour
{
    public EventButton event1;
    [HideInInspector][SerializeField] public int pofi1;
    [HideInInspector][SerializeField] public int pofi2;
    [HideInInspector] public PofI pofiEvent1;
    [HideInInspector] public PofI pofiEvent2;
    public GS_Pofl gS_PofI;
    [HideInInspector] public int extraMovement;
    [SerializeField] public GameObject posSwitch;

    public GameObject SkillPoints, NewResource, ResourceSwitch, NewAbility, PositionSwitch, Movement;



    public enum PofI
    {
        PofI_SkillPoints,
        PofI_NewResource,
        PofI_ResourceSwitch,
        PofI_NewAbility,
        PofI_PositionSwitch,
        PofI_MovementPoints,
    };


    public void Awake()
    {
        randomPofI();
    }

    public void randomPofI()
    {
        pofi1 = (int)Random.Range(0, 5);
        pofiEvent1 = (PofI)pofi1;
        pofi2 = (int)Random.Range(0, 5);
        pofiEvent2 = (PofI)pofi2;


        while (pofi1 == pofi2)
        {
            pofi2 = (int)Random.Range(0, 5);
            break;
        }
    }

    public void PofIEvent(PofI pofi)
    {
        switch (pofi)
        {
            case PofI.PofI_SkillPoints:
                Destroy(gS_PofI.pofi);
                PlayerManager.Instance.SkillPoints += 2;
                break;

            case PofI.PofI_NewResource:
                Destroy(gS_PofI.pofi);
                Instantiate(NewResource, NewResource.transform.position, Quaternion.identity);
                break;

            case PofI.PofI_ResourceSwitch:
                Destroy(gS_PofI.pofi);
                Debug.Log("RessourceSwitch");
                break;

            case PofI.PofI_NewAbility:                
                Destroy(gS_PofI.pofi);
                Debug.Log("NewAbility");
                break;

            case PofI.PofI_PositionSwitch:
                Destroy(gS_PofI.pofi);
                posSwitch = Instantiate(PositionSwitch, PositionSwitch.transform.position, Quaternion.identity);
                break;

            case PofI.PofI_MovementPoints:
                Destroy(gS_PofI.pofi);
                PlayerManager.Instance.extraMovement = 3;
                break;



        }


    }
}
