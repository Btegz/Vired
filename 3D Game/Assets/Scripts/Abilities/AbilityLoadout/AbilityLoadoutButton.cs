using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityLoadoutButton : MonoBehaviour
{
    public Ability ability;

    AbilityLoadout abilityLoadout;

    public void Setup(Ability ability,AbilityLoadout abilityLoadout)
    {
        this.abilityLoadout = abilityLoadout;
        this.ability = ability;
        GetComponent<Image>().sprite = ability.AbilityUISprite;
        Button b = GetComponent<Button>();
        b.onClick.AddListener(clicked);
    }

    private void clicked()
    {
        abilityLoadout.AddAbilityChoice(this);
    }
}
