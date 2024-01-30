using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Settings : MonoBehaviour
{
    [HideInInspector] public bool tooltip;
    public ToggleObj toggle;
    public Toggle FullScreenToggle;
    public Toggle ToolTipToggle;
    public Toggle TutorialToggle;



    public void Start()
    {

       
        if (PlayerPrefs.HasKey("FullScreen"))
        {
            if (PlayerPrefs.GetInt("FullScreen") == 1)
            {
                Debug.Log(PlayerPrefs.GetInt("FullScreen"));
                FullScreenToggle.isOn = true;
            }

            else
            {
                FullScreenToggle.isOn = false;
            }
        }

        else
        {
            toggle.fullscreen = true;
        }





        if (PlayerPrefs.HasKey("ToolTip"))
        {
            if (PlayerPrefs.GetInt("ToolTip") == 1)
            {
                ToolTipToggle.isOn = true;
            }

            else
            {
                ToolTipToggle.isOn = false;
            }

        }
        else
        {
            toggle.tooltip = true;
        }



    }



    public void Tooltips()
    {
        if (ToolTipToggle.isOn)
        {
            toggle.tooltip = true;
            PlayerPrefs.SetInt("ToolTip", 1);
        }

        else
        {
            toggle.tooltip = false;
            PlayerPrefs.SetInt("ToolTip", 0);

        }
    }

    public void FullScreen()
    {
        if (FullScreenToggle.isOn)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            toggle.fullscreen = true;
            PlayerPrefs.SetInt("FullScreen", 1);

        }

        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            toggle.fullscreen = false;
            PlayerPrefs.SetInt("FullScreen", 0);

        }
    }

 
}
