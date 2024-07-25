using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler
{
    [HideInInspector] public GameObject currentSlot;
    
    public PositionSwitch posSwitch;
    private int p;
    private int p2;
    public List<Player> Player;
    public PofIManager PofIManager;
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

    }

    public void OnButtonClicked()
    {
        Debug.Log(Player[0].name);
        Debug.Log(Player[1].name);
        for (int i = 0; i < PlayerManager.Instance.Players.Count; i++)
        {
            
                if (PlayerManager.Instance.Players[i].transform.position == Player[0].transform.position)
                {
                    p = i;
                }

                if (PlayerManager.Instance.Players[i].transform.position == Player[1].transform.position)
                {
                    p2 = i;
                }
            
        }

        
       //Vector2 gridtile = GridManager.Instance.Grid[PlayerManager.Instance.Players[p].CoordinatePosition].transform.position;
        Vector2Int playerPosition = (Vector2Int) PlayerManager.Instance.Players[p].CoordinatePosition;
        Vector2Int player2Position = (Vector2Int)PlayerManager.Instance.Players[p2].CoordinatePosition;
        PlayerManager.Instance.Players[p2].CoordinatePosition = playerPosition;
        PlayerManager.Instance.Players[p].CoordinatePosition = player2Position;
        PlayerManager.Instance.Players[p2].transform.position = GridManager.Instance.Grid[PlayerManager.Instance.Players[p2].CoordinatePosition].transform.position;
        PlayerManager.Instance.Players[p].transform.position = GridManager.Instance.Grid[PlayerManager.Instance.Players[p].CoordinatePosition].transform.position;
    
        Destroy(PofIManager.posSwitch);

       
    }
}
