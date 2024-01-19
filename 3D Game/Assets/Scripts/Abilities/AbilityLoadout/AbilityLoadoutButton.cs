using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class AbilityLoadoutButton : AbilityButton, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    CanvasGroup canvasGroup;

    [SerializeField] GridLayoutGroup originalParent;
    [SerializeField] GridLayoutGroup currentParent;
    [SerializeField] GridLayoutGroup AOption;
    public AbilityLoadoutButton a;
    public bool DragEnd;
 

    public bool inPlayerArea = false;
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentState != ButtonState.newInLoadout)
        {
            return;
        }
        transform.SetParent(originalParent.GetComponentInParent<Canvas>().transform);

        foreach (Player p in PlayerManager.Instance.Players)
        {
           /* if (p.AbilityInventory.Contains(ability))
            {
                Ability abilityRemoved = ability;
                p.AbilityInventory.Remove(ability);
            }*/
        }

        //abilityLoadout.Ability();
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentState != ButtonState.newInLoadout)
        {
            return;
        }
        Vector2 mousePos = Pointer.current.position.ReadValue();

        transform.position = new Vector3(mousePos.x, mousePos.y, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (currentState != ButtonState.newInLoadout)
        {
            return;
        }
        GameObject pointedObj = eventData.pointerCurrentRaycast.gameObject;
        inPlayerArea = false;
        if (pointedObj.GetComponentInParent<UI_PlayerABLInventory>())
        {
            UI_PlayerABLInventory playerArea = pointedObj.GetComponentInParent<UI_PlayerABLInventory>();
           // if (!playerArea.player.AbilityInventory.Contains(ability))
            {
                inPlayerArea = true;
                //playerArea.player.AbilityInventory.Add(ability);
                transform.SetParent(playerArea.InventoryArea.transform);
                EventManager.OnAbilityChosen(this,playerArea.player);
                currentParent = playerArea.InventoryArea;
            }

        }
        else if (pointedObj.GetComponentInParent<GridLayoutGroup>())
        {
            GridLayoutGroup optionArea = pointedObj.GetComponentInParent<GridLayoutGroup>();
            if (optionArea == originalParent)
            {
                if (currentParent.GetComponentInParent<UI_PlayerABLInventory>())
                {
                    UI_PlayerABLInventory plpl = currentParent.GetComponentInParent<UI_PlayerABLInventory>();
                    plpl.player.AbilityInventory.Remove(ability);
                    EventManager.OnLoadoutAbilityChoiceRemove(this);
                    currentState = ButtonState.newInLoadout;
                    inPlayerArea = false;
                }
                transform.SetParent(originalParent.transform);
                currentParent = originalParent;
                //inPlayerArea = true; 
            }
        }
        if (!inPlayerArea)
        {
            transform.SetParent(currentParent.transform);
        }
        canvasGroup.blocksRaycasts = true;
        eventData.hovered.Clear();
      

  
        
    }

    public void Setup(Ability ability, GridLayoutGroup fatherrrr)
    {
        Debug.Log(ability);
        ability.StarterAbility();
        RectData();
        this.ability = ability;
        MakeAbilityToGrid();
        CorrectBackground();
        Button b = GetComponent<Button>();
        originalParent = fatherrrr;
        currentParent = originalParent;
    }

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
}
