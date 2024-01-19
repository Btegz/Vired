using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
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


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    private void Update()
    {
       
        Movement.text = totalMovement.ToString();
        PofICollection.text = PofIscollected.ToString();        
        Kills.text = TotalKills.ToString(); 
        Damage.text = DamageDealt.ToString();
        Resources.text = TotalResources.ToString(); 
        TotalHeals = abilityObj.Heals;
        Heals.text = TotalHeals.ToString();
        Spread.text = TotalSpread.ToString();
    }
}
