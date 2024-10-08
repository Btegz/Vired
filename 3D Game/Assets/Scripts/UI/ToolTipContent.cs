using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipContent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string Header;
    public string Content;

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (TutorialManager.Instance == null)
        {
            TooltipUI.Instance.DisplayTooltip(Header, Content);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (TutorialManager.Instance == null)
        {
            TooltipUI.Instance.HideTooltip();
        }
    }
}
