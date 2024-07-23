using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResolutionSettings : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public ToggleObj toggle;



    public void Start()
    {
        if(PlayerPrefs.HasKey("Resolution"))
        {
           
            resolutionDropdown.value = PlayerPrefs.GetInt("Resolution");
        }
      
    }
    public void SelectResolution()
    {
        switch (resolutionDropdown.value)
        {
            case 0:
                Screen.SetResolution(1920, 1080, toggle.fullscreen);
                break;

            case 1:
                Screen.SetResolution(1366, 768, toggle.fullscreen);
                break;

            case 2:
                Screen.SetResolution(1280, 1024, toggle.fullscreen);
                break;

            case 3:
                Screen.SetResolution(1024, 768, toggle.fullscreen);
                break;
           }
        PlayerPrefs.SetInt("Resolution", resolutionDropdown.value);
        
    }
}
