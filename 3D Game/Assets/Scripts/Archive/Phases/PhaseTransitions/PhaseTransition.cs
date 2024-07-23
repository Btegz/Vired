using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WhenCheckCondition { AfterCast, AfterPhaseEffects,ConfirmButtonPressed}

public abstract class PhaseTransition : ScriptableObject
{
    public abstract void InitPhaseTransitionCheck();

    public abstract void ConditionFullfilledCheck(Player player);

    public WhenCheckCondition whenCheckCondition;
}
