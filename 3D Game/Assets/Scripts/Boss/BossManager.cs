using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public List<Boss> BossList;
    [HideInInspector][SerializeField] int randomTile;

    
    public void Start()
    {

        EventManager.PhaseChangeEvent += Phase;
        Instantiate(BossList[0]);

    }


    public void Phase()
    { 
        BossList.Remove(BossList[0]);
        
        if (BossList[0] != null)
        {
           
            Instantiate(BossList[0]);
        }
    }
}