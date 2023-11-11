using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static PofIManager;
using System;

public class RessourceSwitch : MonoBehaviour
{
    public Button RessourceA;
    public Button RessourceB;
    public Button RessourceC;
    public Button RessourceD;

    public Ability ability;

    void Start()
    {
        RessourceA.onClick.AddListener(() => Switch(0));
        RessourceB.onClick.AddListener(() => Switch(1));
        RessourceC.onClick.AddListener(() => Switch(2));
        RessourceD.onClick.AddListener(() => Switch(3));
    }

    // Update is called once per frame
    // Why are u adding 20???
    void Switch(int buttonNo)
    {
        switch(buttonNo)
        {
            case 0:
                PlayerManager.Instance.RessourceAInventory += 5;
                break;
            case 1:
                PlayerManager.Instance.RessourceBInventory += 5;
                break;
            case 2:
                PlayerManager.Instance.RessourceCInventory += 5;
                break; 
            case 3:
                PlayerManager.Instance.RessourceDInventory += 5;
                break;
        }
    }
}
