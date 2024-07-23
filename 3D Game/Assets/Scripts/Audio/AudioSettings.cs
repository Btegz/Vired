using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public Slider MasterSlider;
    public Slider MusicSlider;
    public Slider SFXSlider;
    public AudioMixer mixer;

    private void Start()
    {
        if(PlayerPrefs.HasKey("MasterVolume"))
        {
            mixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume"));
            mixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume"));
            mixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
            SetSliders();
        }

        else
        {
            SetSliders();
        }
    }
    public void SetSliders()
    {
        MasterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    }

    public void UpdateMasterVol()
    {
        mixer.SetFloat("MasterVolume", MasterSlider.value);
        PlayerPrefs.SetFloat("MasterVolume", MasterSlider.value);
    }

    public void UpdateMusicVol()
    {
        mixer.SetFloat("MusicVolume", MusicSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", MusicSlider.value);
    }

    public void UpdateSFXVol()
    {
        mixer.SetFloat("SFXVolume", SFXSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", SFXSlider.value);
    }

}
