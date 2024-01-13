using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject optionCanvas;

    public Button gameStartButton;
    public Button optionsButton;
    public Button exitButton;
    // Start is called before the first frame update
    public void Start()
    {
        gameStartButton.onClick.AddListener(StartGame);
        optionsButton.onClick.AddListener(OptionsSwitch);
        exitButton.onClick.AddListener(ExitGame);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("PlayerScene_");
    }

    public void OptionsSwitch()
    {
        gameObject.SetActive(false);
        optionCanvas.SetActive(true);

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
