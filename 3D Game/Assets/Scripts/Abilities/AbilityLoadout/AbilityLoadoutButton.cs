using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class AbilityLoadoutButton : AbilityButton, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    CanvasGroup canvasGroup;

    [SerializeField] GridLayoutGroup originalParent;
    [SerializeField] GridLayoutGroup currentParent;
    [SerializeField] GridLayoutGroup AOption;
    public AbilityLoadoutButton a;
    public Sprite Nope;




    // /\_/\
    //((@v@))
    //():::()
    // VV-VV


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
                EventManager.OnAbilityChosen(this, playerArea.player);
                currentParent = playerArea.InventoryArea;
                currentState = ButtonState.selectedInLoadOut;
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

            //        / "*._         _
            //  .- *'`    `*-.._.-' /
            //< * ))     ,       (
            //  `*-._`._(__.-- * "`.\
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
        //MakeAbilityToGrid();
       
        CorrectResource();
        Button b = GetComponent<Button>();
        originalParent = fatherrrr;
        currentParent = originalParent;
    }

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentState == ButtonState.selectedInLoadOut)
        {
            GetComponent<Image>().sprite = Nope;
        }

    }  
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentState == ButtonState.selectedInLoadOut)
        {
            GetComponent<Image>().sprite = A_Background;
            
            switch (ability.MyCostRessource)
            {
                case Ressource.ressourceA:
                    GetComponent<Image>().sprite = A_Background;
                    break;

                case Ressource.ressourceB:
                    GetComponent<Image>().sprite = B_Background;

                    break;
                case Ressource.ressourceC:
                    GetComponent<Image>().sprite = C_Background;

                    break;
                case Ressource.ressourceD:
                    GetComponent<Image>().sprite = D_Background;
                    break;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentState == ButtonState.selectedInLoadOut)
        {
            switch (ability.MyCostRessource)
            {
                case Ressource.ressourceA:
                    if (!PlayerManager.Instance.abilityLoadout.AbilitiesA.Contains(ability))
                    {
                        Debug.Log(ability);
                        PlayerManager.Instance.abilityLoadout.AbilitiesA.Add(ability);
                        EventManager.OnLoadoutAbilityChoiceRemove(this);
                    }
                    break;

                case Ressource.ressourceB:
                    if (!PlayerManager.Instance.abilityLoadout.AbilitiesB.Contains(ability))
                    {
                        PlayerManager.Instance.abilityLoadout.AbilitiesB.Add(ability);
                        EventManager.OnLoadoutAbilityChoiceRemove(this);
                    }
                    break;
                case Ressource.ressourceC:
                    if (!PlayerManager.Instance.abilityLoadout.AbilitiesC.Contains(ability))
                    {
                        PlayerManager.Instance.abilityLoadout.AbilitiesC.Add(ability);
                        EventManager.OnLoadoutAbilityChoiceRemove(this);
                    }
                    break;
                case Ressource.ressourceD:
                    if (!PlayerManager.Instance.abilityLoadout.AbilitiesD.Contains(ability))
                    {
                        PlayerManager.Instance.abilityLoadout.AbilitiesD.Add(ability);
                        EventManager.OnLoadoutAbilityChoiceRemove(this);
                    }
                    break;
            }

            Destroy(gameObject);


        }
    }
}
