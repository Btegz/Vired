using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class AbilityLoadoutButton : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public Ability ability;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePos = Pointer.current.position.ReadValue();

        transform.position = new Vector3(mousePos.x,mousePos.y,0);
        Debug.Log(transform.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Check if position is inside of a Player Inventory Area
        // if true invoke Ability Chosen Event

        EventManager.OnAbilityChosen(this);
    }

    public void Setup(Ability ability)
    {
        this.ability = ability;
        GetComponent<Image>().sprite = ability.AbilityUISprite;
        Button b = GetComponent<Button>();
        b.onClick.AddListener(clicked);
    }

    private void clicked()
    {
        //abilityLoadout.AddAbilityChoice(this);
    }
}
