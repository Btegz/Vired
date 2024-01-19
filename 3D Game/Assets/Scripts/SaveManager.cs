using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveManager : MonoBehaviour
{
    int totalMovement;
    public TextMeshProUGUI Movement;

    
    public PofIManager PofIs;
    int PofIscollected;
    public TextMeshProUGUI PofICollection;


    public Enemy enemies;
    int TotalKills;
    public TextMeshProUGUI Kills;

    int DamageDealt;
    public TextMeshProUGUI Damage;

    int TotalSpread;
    public TextMeshProUGUI Spread;


    public GS_positive positive;
    int TotalResources;
    public TextMeshProUGUI Resources;


    public AbilityObjScript abilityObj;
    int TotalHeals;
    public TextMeshProUGUI Heals;





    private void Update()
    {
        totalMovement = PlayerManager.Instance.totalSteps;
        Damage.text = totalMovement.ToString();

        PofIscollected = PofIs.Collected;
        PofICollection.text = PofIscollected.ToString(); 
        
        TotalKills = enemies.TotalKills;
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
