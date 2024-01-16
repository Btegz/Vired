using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NegativeBarTooltip : ToolTipContent
{
    int TilesTotal;
    string defaultContent;

    private void Start()
    {
        defaultContent = Content;
        TilesTotal = GridManager.Instance.Grid.Count;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        Content = defaultContent;
        Content += "<br>Negative Tiles: " + currentlyNegativeCount() + " / " + TilesTotal * (2f/3f) + ".";
        Content += "<br>Next Turn: +"+nextTurnNegative()+"."; 
        base.OnPointerEnter(eventData);
    }
    int currentlyNegativeCount()
    {
        int result = 0;
        result = GridManager.Instance.GetTilesWithState(GridManager.Instance.gS_Negative).Count;
        result += GridManager.Instance.GetTilesWithState(GridManager.Instance.gS_Boss).Count;
        result += GridManager.Instance.GetTilesWithState(GridManager.Instance.gS_Enemy).Count;
        return result;
    }

    public int nextTurnNegative()
    {
        // still to do: add the bosstiles currently not bossnegative. if enemies dont spread every turn this will be wrong.

        int amount = 0;
        foreach(Enemy enemy in GridManager.Instance.gameObject.GetComponentsInChildren<Enemy>())
        {
            Debug.Log("ONE ENEMY");
            amount += enemy.AmountSpreadNextTurn();
            
        }
        return amount;

    }
}
