using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class AbilityLoadoutButton : AbilityButton, IDragHandler, IEndDragHandler,IBeginDragHandler
{
    CanvasGroup canvasGroup;

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePos = Pointer.current.position.ReadValue();

        transform.position = new Vector3(mousePos.x,mousePos.y,0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Check if position is inside of a Player Inventory Area
        // if true invoke Ability Chosen Event
        canvasGroup.blocksRaycasts = true;
    }

    public void Setup(Ability ability)
    {
        this.ability = ability;
        MakeAbilityToGrid();
        CorrectBackground();
        Button b = GetComponent<Button>();
    }

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
}
