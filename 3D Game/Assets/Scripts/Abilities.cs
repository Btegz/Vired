using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Abilities
{
    private string name;


    private Dictionary<Vector3, Enum> _bSchaden = new Dictionary<Vector3, Enum>()
    {
        //beispiel... hier müssen die Koordinaten noch angepasst werden, sind im Moment nicht in abhängigkeit der Playerposition
        { new Vector3(0, 0, 0), Effect.Negative100 },
        { new Vector3(0, 1, 0), Effect.Negative100 },
        { new Vector3(1, 2, 0), Effect.Negative100 },
    };
    
    void Update()
    {
        
    }
    
    
    
    public void UsingEffect(Effect ef)
    {
        switch (ef)
        {
            case Effect.Positive100:
                //Feld Zustand positiv 
                //-100
            
            case Effect.Positive200:
                // Feld Zustand positiv
                // -200
            
            case Effect.Negative100:
                // Feld Zustand negativ 
                // -100
            
            case Effect.Negative200:
                // Feld Zustand negativ 
                // -200
            
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
