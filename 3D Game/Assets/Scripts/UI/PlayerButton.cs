using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public Player player;

    public void OnPointerClick(PointerEventData eventData)
    {
        EventManager.OnSelectPlayer(player);
    }

    public void Setup(Player player)
    {
        this.player = player;
        GetComponentInChildren<TMP_Text>().text = player.name;
    }



}
