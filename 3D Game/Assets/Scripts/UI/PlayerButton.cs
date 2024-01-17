using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public Player player;
    public PlayerButton MainPlayerButton;

    public void Start()
    {
        EventManager.OnSelectPlayerEvent += Click;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        EventManager.OnSelectPlayer(player);
       // Click(player);

    }

    public void Setup(Player player)
    {
        this.player = player;
        GetComponentInChildren<TMP_Text>().text = player.name;
        //if (PlayerManager.Instance.selectedPlayer == player)
        //{
        //    EventManager.OnSelectPlayer(player);
        //}
    }

    public void Click(Player p)
    {
        if (player == p)
        {
            if (this != MainPlayerButton)
            {
               
                Player previousMainPlayer = MainPlayerButton.player;
                Player MyPlayer = player;
                //player = previousMainPlayer;
                //MainPlayerButton.player = MyPlayer;
                Setup(previousMainPlayer);
                MainPlayerButton.Setup(MyPlayer);

            }
        }
    }


}
