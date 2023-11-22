using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TooltipUI : MonoBehaviour
{
    [SerializeField] TMP_Text TooltipText;
    [SerializeField] Vector2 offset;

    public void DisplayTooltip(string header, string content)
    {
        TooltipText.text = header;
        TooltipText.text += "<br>" + content;
    }

    public void HideTooltip()
    {
        TooltipText.text = "";
    }

    private void Update()
    {
        transform.position = Pointer.current.position.ReadValue() + offset;
    }
}
