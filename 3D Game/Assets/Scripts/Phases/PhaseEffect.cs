using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PhaseEffect : ScriptableObject
{
    public int everyXRounds;
    public abstract void triggerPhaseEffect(GridManager grid);
}
