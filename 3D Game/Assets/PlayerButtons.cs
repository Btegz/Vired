using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerButtons : MonoBehaviour
{
    public Button ButtonPlayer1;
    public Button ButtonPlayer2;
    public Button ButtonPlayer3;

    [SerializeField] public Player player1;
    [SerializeField] public Player player2;
    [SerializeField] public Player player3;


    public List<Button> PlayerButton;

    public TextMeshProUGUI player1Text;
    public TextMeshProUGUI player2Text;
    public TextMeshProUGUI player3Text;

    //private List<string> playerText;

    string CurrentFirst;
    public PlayerButton pb;
 


    public void Start()
    {
        player1Text.text = ButtonPlayer1.GetComponentInChildren<TextMeshProUGUI>().text;
        player2Text.text = ButtonPlayer2.GetComponentInChildren<TextMeshProUGUI>().text;
        player3Text.text = ButtonPlayer3.GetComponentInChildren<TextMeshProUGUI>().text;

      /*  playerText.Add(player1Text.text);
        playerText.Add(player2Text.text);
        playerText.Add(player3Text.text);*/
    
        

        PlayerPrefs.SetString("Player1", player1Text.text);
        PlayerPrefs.SetString("Player2", player2Text.text);
        PlayerPrefs.SetString("Player3", player3Text.text);
    }
  
    
    public void Button2Clicked()
    {
       
        CurrentFirst = PlayerPrefs.GetString("Player1");
        PlayerPrefs.SetString("Player1", ButtonPlayer2.GetComponentInChildren<TextMeshProUGUI>().text);


        ButtonPlayer1.GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString("Player1");

        if (ButtonPlayer2.GetComponentInChildren<TextMeshProUGUI>().text == PlayerPrefs.GetString("Player1"))
            PlayerPrefs.SetString("Player2", CurrentFirst);
            

        else if (ButtonPlayer3.GetComponentInChildren<TextMeshProUGUI>().text == PlayerPrefs.GetString("Player1"))
            PlayerPrefs.SetString("Player3", CurrentFirst);

       
       

    }

    public void Button3Clicked()
    {
        CurrentFirst = PlayerPrefs.GetString("Player1");
        PlayerPrefs.SetString("Player1", ButtonPlayer3.GetComponentInChildren<TextMeshProUGUI>().text);


        ButtonPlayer1.GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString("Player1");

        if (ButtonPlayer2.GetComponentInChildren<TextMeshProUGUI>().text == PlayerPrefs.GetString("Player1"))
            PlayerPrefs.SetString("Player2", CurrentFirst);

        else if (ButtonPlayer3.GetComponentInChildren<TextMeshProUGUI>().text == PlayerPrefs.GetString("Player1"))
            PlayerPrefs.SetString("Player3", CurrentFirst);

       
    }

    public void Update()
    { 
       player2Text.text = PlayerPrefs.GetString("Player2");
        player3Text.text = PlayerPrefs.GetString("Player3");

    }

    
}
