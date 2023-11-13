using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PositionSwitch : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler

{
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    public Player currentPlayer;
    private Player Player1;
    private Player Player2;
    private Player Player3;
    public Drop drop;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {

        if (gameObject.CompareTag("Player1"))
        {
            Player1 = PlayerManager.Instance.Players[0];
            drop.Player.Add(Player1);
        }
        if (gameObject.CompareTag("Player2"))
        {
            Player2 = PlayerManager.Instance.Players[1];
            drop.Player.Add(Player2);
        }
        if (gameObject.CompareTag("Player3"))
        {
            Player3 = PlayerManager.Instance.Players[2];
            drop.Player.Add(Player3);
        }
        canvasGroup.blocksRaycasts = false;



    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
    }


}
