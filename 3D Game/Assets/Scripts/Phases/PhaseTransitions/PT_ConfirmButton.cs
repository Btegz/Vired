using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PT_ConfirmButton",menuName = "PhaseTransitions/PT_ConfirmButton")]
public class PT_ConfirmButton : PhaseTransition
{
    public override void ConditionFullfilledCheck()
    {
        EventManager.OnAbilityCastEvent -= ConditionFullfilledCheck;
        EventManager.OnEndTurnEvent -= ConditionFullfilledCheck;
        EventManager.OnConfirmButtonEvent -= ConditionFullfilledCheck;
        GridManager.Instance.mainCanvas.gameObject.SetActive(true);
        GridManager.Instance.PhaseTransition();
    }

    public override void InitPhaseTransitionCheck()
    {
        switch (whenCheckCondition)
        {
            case WhenCheckCondition.ConfirmButtonPressed:
                EventManager.OnConfirmButtonEvent += ConditionFullfilledCheck;                
                break;
            case WhenCheckCondition.AfterCast:
                EventManager.OnAbilityCastEvent += ConditionFullfilledCheck;
                break;
            case WhenCheckCondition.AfterPhaseEffects:
                //EventManager.OnEndTurnEvent += ConditionFullfilledCheck;
                break;
        }
    }
}
