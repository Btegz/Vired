using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Abilities: MonoBehaviour
{
    private string name;

    public Ability ability;

    

    void Update()
    {
        
    }

    public void AttachedToPlayer()
    {
        
    }

    public void Rotation()
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
