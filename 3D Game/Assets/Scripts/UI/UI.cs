using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UI : MonoBehaviour
{
    public GameObject SettingsCanvas;
    public GameObject Reassurance;
    public GameObject ReassuranceMainMenu;

    public GameObject TopDownButton;
    public GameObject WorldButton;
 

    public void Settings()
    {
        SettingsCanvas.SetActive(true);
    }

    public void CloseSettings()
    {
        SettingsCanvas.SetActive(false);
    }

    public void SettingsQuit()
    {
        Reassurance.SetActive(true);
        SettingsCanvas.SetActive(false);
         

    }


    public void QuitGame()
    {
        Application.Quit(); 
    }

    public void CancelQuitGame()
    {
        Reassurance.SetActive(false);
        SettingsCanvas.SetActive(true);
    }

    public void ReassuranceMain()
    {
         ReassuranceMainMenu.SetActive(true);
        SettingsCanvas.SetActive(false);
       
      
    }

    public void MainMenu()
    {
     SceneManager.LoadScene("StartScreen");
    }

    public void CancelMain()
    {
        ReassuranceMainMenu.SetActive(false);
        SettingsCanvas.SetActive(true);

    }

    public void CancelSettings()
    {
        SettingsCanvas.SetActive(false);
    }

    public void SwitchToMain()
    {
        WorldButton.SetActive(false);
        TopDownButton.SetActive(true); 
        CameraRotation.Instance.SwitchtoMain();
    }

    public void SwitchToTopdown()
    {
        WorldButton.SetActive(true);
        TopDownButton.SetActive(false);
        CameraRotation.Instance.SwitchToTopDown();
    }
}
