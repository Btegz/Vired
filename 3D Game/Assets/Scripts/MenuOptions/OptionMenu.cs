using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{

    public GameObject mainMenuCanvas;
    public Button backButton;

    void Start()
    {
        backButton.onClick.AddListener(BackToMainMenu);
    }

    
    public void BackToMainMenu()
    {
        gameObject.SetActive(false);
        mainMenuCanvas.SetActive(true);
    }

    public void SetVolume()
    {

    }

    public void SetFullscreen()
    {

    }
}
