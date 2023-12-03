using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipContent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string Header;
    public string Content;

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipUI.Instance.DisplayTooltip(Header, Content);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.Instance.HideTooltip();
    }
}
