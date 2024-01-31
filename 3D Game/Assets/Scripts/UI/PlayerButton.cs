using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public Player player;
    public PlayerButton MainPlayerButton;
    public Image Background;
    public Image PlayerModel;

    [SerializeField] Image playerPic;

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
        //GetComponentInChildren<TMP_Text>().text = player.name;
        playerPic.sprite = player.pic;
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

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // hier soll der Glitcheffekt abgespielt werden uwu
    }


    public IEnumerator EnableModel()
    {
        Background.enabled = true; 
        PlayerModel.enabled = true;
        yield return null;
    }

    public IEnumerator Disable()
    {
        Background.enabled = false;
        PlayerModel.enabled = false;
        yield return null;
    }
}
