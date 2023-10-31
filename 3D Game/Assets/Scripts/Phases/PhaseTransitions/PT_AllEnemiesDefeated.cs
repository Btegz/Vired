using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PT_AllEnemiesDefeated : PhaseTransition
{
    public override void ConditionFullfilledCheck()
    {
        if (GridManager.Instance.GetTilesWithState(GridManager.Instance.gS_Boss).Count == 0 && GridManager.Instance.GetTilesWithState(GridManager.Instance.gS_Enemy).Count == 0)
        {
            EventManager.OnAbilityCastEvent -= ConditionFullfilledCheck;
            EventManager.OnEndTurnEvent -= ConditionFullfilledCheck;
            GridManager.Instance.PhaseTransition();
        }
    }

    public override void InitPhaseTransitionCheck()
    {
        switch (whenCheckCondition)
        {
            case WhenCheckCondition.AfterCast:
                EventManager.OnAbilityCastEvent += ConditionFullfilledCheck;
                break;
            case WhenCheckCondition.AfterPhaseEffects:
                EventManager.OnEndTurnEvent += ConditionFullfilledCheck;
                break;
        }
    }
}
