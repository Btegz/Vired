using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject optionCanvas;
    public GameObject CreditCanvas;
    public Button gameStartButton;
    public Button optionsButton;
    public Toggle tutorialToggle;
    public ToggleObj TutorialObj;

    public Button exitButton;
    // Start is called before the first frame update
    public void Start()
    {
        gameStartButton.onClick.AddListener(StartGame);
        optionsButton.onClick.AddListener(OptionsSwitch);
        exitButton.onClick.AddListener(ExitGame);


        if (PlayerPrefs.HasKey("Tutorial"))
        {
            if (PlayerPrefs.GetInt("Tutorial") == 1)
            {
                TutorialObj.tutorial = true;
            }
            else
            {

                TutorialObj.tutorial = false;

            }
        }


    }

    public void StartGame()
    {
        if (TutorialObj.tutorial == true)
        {
            SceneManager.LoadScene("Tutorial");
        }
        else
        {
            SceneManager.LoadScene("PlayerScene_");

        }

    }

    public void OptionsSwitch()
    {
        gameObject.SetActive(false);
        optionCanvas.SetActive(true);

    }

    public void Tutorial()
    {
        if (tutorialToggle.isOn == true)
        {
            TutorialObj.tutorial = true;
            PlayerPrefs.SetInt("Tutorial", 1);
        }

        else
        {
            TutorialObj.tutorial = false;
            PlayerPrefs.SetInt("Tutorial", 0);
        }
    }

    public void Credits()
    {
        gameObject.SetActive(false);
        CreditCanvas.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }


    public void OpenLink()
    {
        Application.OpenURL("https://itch.io/");
    }
}
