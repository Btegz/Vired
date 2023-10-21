using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract Phase Effect.
/// </summary>
public abstract class PhaseEffect : ScriptableObject
{
    [SerializeField,Tooltip("To make the Effect trigger not every round. If it's supposed to trigger every round set it to 1.")]public int everyXRounds;
    public abstract void triggerPhaseEffect(GridManager grid);
}
