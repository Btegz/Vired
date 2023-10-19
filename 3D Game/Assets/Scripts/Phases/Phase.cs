using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Phase : ScriptableObject
{
    [SerializeField] private List<PhaseEffect> phaseEffects;

    public List<PhaseEffect> myPhaseEffects
    {
        get { return phaseEffects; }
        set { phaseEffects = value; }
    }

    public void TriggerPhaseEffects(int TurnCount, GridManager gridManager)
    {
        foreach(PhaseEffect phaseEffect in phaseEffects)
        {
            if(phaseEffect.everyXRounds % TurnCount == 0)
            {
                phaseEffect.triggerPhaseEffect(gridManager);
            }
        }
    }
}
