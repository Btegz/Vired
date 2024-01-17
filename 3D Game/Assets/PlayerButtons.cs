using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerButtons : MonoBehaviour
{
    public Button MainPlayerButton; 
    public Button ButtonPlayer2;
    public Button ButtonPlayer3;

    public TextMeshProUGUI player1Text;
    public TextMeshProUGUI player2Text;
    public TextMeshProUGUI player3Text;

    string CurrentFirst;
    public PlayerButton pb;

 


    public void Start()
    {
        player1Text.text = MainPlayerButton.GetComponentInChildren<TextMeshProUGUI>().text;
        player2Text.text = ButtonPlayer2.GetComponentInChildren<TextMeshProUGUI>().text;
        player3Text.text = ButtonPlayer3.GetComponentInChildren<TextMeshProUGUI>().text;
    
        PlayerPrefs.SetString("MainPlayerButton", player1Text.text);
        PlayerPrefs.SetString("Player2", player2Text.text);
        PlayerPrefs.SetString("Player3", player3Text.text);
    }
  
    
    public void Button2Clicked()
    {
       
        CurrentFirst = PlayerPrefs.GetString("MainPlayerButton");
        PlayerPrefs.SetString("MainPlayerButton", ButtonPlayer2.GetComponentInChildren<TextMeshProUGUI>().text);


        MainPlayerButton.GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString("Player1");

        if (ButtonPlayer2.GetComponentInChildren<TextMeshProUGUI>().text == PlayerPrefs.GetString("Player1"))
            PlayerPrefs.SetString("Player2", CurrentFirst);
            

        else if (ButtonPlayer3.GetComponentInChildren<TextMeshProUGUI>().text == PlayerPrefs.GetString("Player1"))
            PlayerPrefs.SetString("Player3", CurrentFirst);

       
       

    }

    public void Button3Clicked()
    {
        CurrentFirst = PlayerPrefs.GetString("MainPlayerButton");
        PlayerPrefs.SetString("MainPlayerButton", ButtonPlayer3.GetComponentInChildren<TextMeshProUGUI>().text);


        MainPlayerButton.GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString("MainPlayerButton");

        if (ButtonPlayer2.GetComponentInChildren<TextMeshProUGUI>().text == PlayerPrefs.GetString("MainPlayerButton"))
            PlayerPrefs.SetString("Player2", CurrentFirst);

        else if (ButtonPlayer3.GetComponentInChildren<TextMeshProUGUI>().text == PlayerPrefs.GetString("MainPlayerButton"))
            PlayerPrefs.SetString("Player3", CurrentFirst);

       
    }

    public void Update()
    { 
       player2Text.text = PlayerPrefs.GetString("Player2");
       player3Text.text = PlayerPrefs.GetString("Player3");

    }

    
}
