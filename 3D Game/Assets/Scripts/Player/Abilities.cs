using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Abilities
{
    private string name;

    public Ability ability;


    private Dictionary<Vector3, Enum> _bSchaden = new Dictionary<Vector3, Enum>()
    {
        //beispiel... hier müssen die Koordinaten noch angepasst werden, sind im Moment nicht in abhängigkeit der Playerposition
        { new Vector3(0,0), Effect.Negative },
        { new Vector3(0, 1 ), Effect.Negative },
        { new Vector3(1, 2 ), Effect.Negative },
    };
    
    void Update()
    {
        
    }
    
    
    
    public void UsingEffect(Effect ef)
    {
        switch (ef)
        {
            case Effect.Positive:
                //Feld Zustand positiv 
                //-100
        
            
            case Effect.Negative:
                // Feld Zustand negativ 
                // -100
         
            
            case Effect.Movement:
                break;
            // Player Movement 
        }
        
        
    }
    
    // Effekt 
    // Form 
    // Kosten 
    // Ressource (Schere/Stein/Papier System)
}
