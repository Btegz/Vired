using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveManager : MonoBehaviour
{
    
    public int totalMovement;
    public TextMeshProUGUI Movement;

    
    public PofIManager PofIs;
    public int PofIscollected;
    public TextMeshProUGUI PofICollection;


    public Enemy enemies;
    public int TotalKills;
    public TextMeshProUGUI Kills;

    public int DamageDealt;
    public TextMeshProUGUI Damage;

    public int TotalSpread;
    public TextMeshProUGUI Spread;


    public GS_positive positive;
    public int TotalResources;
    public TextMeshProUGUI Resources;


    public AbilityObjScript abilityObj;
    public int TotalHeals;
    public TextMeshProUGUI Heals;





    private void Update()
    {
        totalMovement = PlayerManager.Instance.totalSteps;
       
        Movement.text = totalMovement.ToString();

        PofIscollected = PofIs.Collected;
        PofICollection.text = PofIscollected.ToString(); 
        
        TotalKills = enemies.TotalKills;
         Debug.Log(TotalKills + "Kills");
        Kills.text = TotalKills.ToString(); 

        DamageDealt = enemies.DamageDealt;
        Damage.text = DamageDealt.ToString();
        
        TotalResources = positive.TotalResources;
        Resources.text = TotalResources.ToString(); 
        
        TotalHeals = abilityObj.Heals;
        Heals.text = TotalHeals.ToString();
        
        TotalSpread = enemies.TotalSpread;
        Spread.text = TotalSpread.ToString();
    }
}
