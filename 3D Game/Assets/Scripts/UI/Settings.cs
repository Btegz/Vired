using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Settings : MonoBehaviour
{
    [HideInInspector] public bool tooltip;
    public ToggleObj toggle;
    public Toggle m_Toggle;



    public void Start()
    {
        m_Toggle = GetComponent<Toggle>();

    }
    public void Tooltips()
    {
        if(m_Toggle.isOn)
        {
            toggle.tooltip = true;
           
        }

        else
        {
            toggle.tooltip = false;
           
        }
    }

    public void FullScreen()
    {
        if(m_Toggle.isOn)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            toggle.tooltip = true;
            



        }

        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            
            toggle.tooltip = false;

        }
    }
}
