using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Phases the Game can be in. The Phase stores a List of Effects it can Trigger whenever neccessary.
/// </summary>
[CreateAssetMenu]
public class Phase : ScriptableObject
{
    
    [SerializeField,Tooltip("Drag and drop PhaseEffects in here. Remeber, order matters top to bottom.")] private List<PhaseEffect> phaseEffects;

    /// <summary>
    /// List of PhaseEffects to be triggered.
    /// </summary>
    public List<PhaseEffect> myPhaseEffects
    {
        get { return phaseEffects; }
        set { phaseEffects = value; }
    }

    /// <summary>
    /// Functions to trigger the PhaseEffects one after the other.
    /// </summary>
    /// <param name="TurnCount">required in case Effects triggere every other rounds.</param>
    /// <param name="gridManager">provides important reference to the effects.</param>
    public void TriggerPhaseEffects(int TurnCount, GridManager gridManager)
    {
        foreach(PhaseEffect phaseEffect in phaseEffects)
        {
            if(phaseEffect.everyXRounds % TurnCount == 0)
            {
                phaseEffect.TriggerPhaseEffect(TurnCount,gridManager);
            }
        }
    }
}
