using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    public int totalMovement;

    
    public PofIManager PofIs;
    public int PofIscollected;


    public Enemy enemies;
    public int TotalKills;

    public int DamageDealt;

    public int TotalSpread;


    public GS_positive positive;
    public int TotalResources;


    public AbilityObjScript abilityObj;
    public  int TotalHeals;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

     
    }


    private void Update()
    {
       
       PlayerPrefs.SetString("Kills",TotalKills.ToString()); 
       PlayerPrefs.SetString("PofIs",totalMovement.ToString()); 
       PlayerPrefs.SetString("Movement",PofIscollected.ToString()); 
       PlayerPrefs.SetString("Damage",DamageDealt.ToString()); 
       PlayerPrefs.SetString("Resources",TotalResources.ToString()); 
       PlayerPrefs.SetString("Heals",TotalHeals.ToString()); 
       PlayerPrefs.SetString("Spread",TotalSpread.ToString()); 


      
    }
}
