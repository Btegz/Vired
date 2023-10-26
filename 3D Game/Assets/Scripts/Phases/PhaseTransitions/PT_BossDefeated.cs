using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PT_BossDefeated : PhaseTransition
{
    public override void ConditionFullfilledCheck()
    {
        Debug.Log("I AM CHECKING THE Condition for Phase Transition uwu. Bosses Alive: "+GridManager.Instance.GetTilesWithState(GridManager.Instance.gS_Boss).Count);
        if (GridManager.Instance.GetTilesWithState(GridManager.Instance.gS_Boss).Count == 0)
        {
            EventManager.OnAbilityCastEvent -= ConditionFullfilledCheck;
            EventManager.OnEndTurnEvent -= ConditionFullfilledCheck;
            GridManager.Instance.PhaseTransition();
        }
    }

    public override void InitPhaseTransitionCheck()
    {
        Debug.Log("MY Phase Transition to defeat boss has been initiated");
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
