using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PofIManager : MonoBehaviour
{
    public GridTile pofiTile;
    public PofIVisuals pofiPrefab;

    public EventButton event1;
    [HideInInspector][SerializeField] public int pofi1;
    [HideInInspector][SerializeField] public int pofi2;
    [HideInInspector] public int Collected;
    [HideInInspector] public PofI pofiEvent1;
    [HideInInspector] public PofI pofiEvent2;
    public GS_Pofl gS_PofI;
    [HideInInspector] public int extraMovement;
    [SerializeField] public GameObject posSwitch;

    [SerializeField] public AudioMixerGroup soundEffect;
    [SerializeField] public AudioData PofISelect;

    public GameObject SkillPoints, NewResource, Movement;

    public GameObject parent;



    public enum PofI
    {
        PofI_SkillPoints,
        PofI_NewResource,
        //PofI_PositionSwitch,
        PofI_MovementPoints,
    };


    public void Awake()
    {
        randomPofI();
        SaveManager.Instance.PofIscollected++;
     

    }

    public void randomPofI()
    {
        pofi1 = (int)Random.Range(0,3);
        pofiEvent1 = (PofI)pofi1;

        pofi2 = (int)Random.Range(0,3);
        

        while (pofi1 == pofi2)
        {
            pofi2 = (int)Random.Range(0, 3);
          
        }
        pofiEvent2 = (PofI)pofi2;
    }

    public void PofIEvent(PofI pofi)
    {
        switch (pofi)
        {
            case PofI.PofI_SkillPoints:
                AudioManager.Instance.PlaySoundAtLocation(PofISelect, soundEffect, null);
                Destroy(gS_PofI.pofi.gameObject);
                PlayerManager.Instance.SkillPoints += 2;
                break;

            case PofI.PofI_NewResource:
                AudioManager.Instance.PlaySoundAtLocation(PofISelect, soundEffect, null);
                Destroy(gS_PofI.pofi.gameObject);
                Instantiate(NewResource, NewResource.transform.position, Quaternion.identity);
                break;

          /*  case PofI.PofI_PositionSwitch:
                Destroy(gS_PofI.pofi);
                posSwitch = Instantiate(PositionSwitch, PositionSwitch.transform.position, Quaternion.identity);
                break;*/

            case PofI.PofI_MovementPoints:
                AudioManager.Instance.PlaySoundAtLocation(PofISelect, soundEffect, null);
                Destroy(gS_PofI.pofi.gameObject);
                EventManager.OnBonusMovementPointGain(3);
                PlayerManager.Instance.extraMovement += 3;
                break;
        }
        pofiPrefab.FlyAway();
        pofiTile.ChangeCurrentState(GridManager.Instance.gS_Neutral);

        PlayerManager.Instance.move = true;
    }
}
