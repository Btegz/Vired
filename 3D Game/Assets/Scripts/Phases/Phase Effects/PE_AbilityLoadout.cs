using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName ="PE_AbilityLoadout",menuName = "PhaseEffects/PE_AbilityLoadout")]
public class PE_AbilityLoadout : PhaseEffect
{
    [SerializeField] AbilityLoadout abilityLoadoutPrefab;

    [SerializeField] int AmountToChoose;

    public override void TriggerPhaseEffect(int turnCounter, GridManager grid)
    {
        AbilityLoadout abl = Instantiate(abilityLoadoutPrefab);
        abl.amountToChoose = AmountToChoose;
    }
}
