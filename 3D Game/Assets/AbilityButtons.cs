using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButtons : MonoBehaviour

{
    public Image AbilityButton1; 
    public Image AbilityButton2; 
    public Image AbilityButton3;

    public Player player;

    // Start is called before the first frame update
    void Start()
    {

        if(player.AbilityInventory.Count < 1)
        {

        }

        else if(player.AbilityInventory.Count==1)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
