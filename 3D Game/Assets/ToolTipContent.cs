using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipContent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TooltipUI tooltipUI;
    public string Header;
    public string Content;

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipUI.DisplayTooltip(Header, Content);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipUI.HideTooltip();
    }
}
