using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPreview : MonoBehaviour
{

    public AbilityButton AbilityButtonParent;
    private Ability ability; 
    private void OnEnable()
    {

       ability =  AbilityButtonParent.ability;
       AbilityButtonParent.MakeAbilityToGrid(ability);
    }
}
