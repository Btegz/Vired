using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTooltip : AbilityButton
{
    [SerializeField] AbilityCastButton abilityCastButton;

    private void Awake()
    {
        RectData();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        MakeAbilityToGrid(abilityCastButton.ability);
    }

    private void OnDisable()
    {
        ResetButton();
    }
}
