using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance;

    [SerializeField] TMP_Text TooltipText;
    [SerializeField] Vector2 offset;

    [SerializeField] Canvas canvas;
    [SerializeField] RectTransform rect;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }


    public void DisplayTooltip(string header, string content)
    {
        gameObject.SetActive(true);
        TooltipText.text = "<b>" + header + "</b>";
        TooltipText.text += "<br>" + content.Replace("\\n", "\n");
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        float xOffset, yOffset;
        if (mousePos.y > (canvas.renderingDisplaySize.y / 2))
        {
            yOffset = Input.mousePosition.y - offset.y;
        }
        else
        {
            yOffset = Input.mousePosition.y + offset.y + rect.rect.height;
        }

        if (mousePos.x > (canvas.renderingDisplaySize.x / 2))
        {
            xOffset = Input.mousePosition.x - offset.x - rect.rect.width;
        }
        else
        {
            xOffset = Input.mousePosition.x + offset.x;
        }
        transform.position = new Vector2(xOffset, yOffset);
    }
}
