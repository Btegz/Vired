using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDestroy : MonoBehaviour
{
    bool AudioPlaying;
    AudioData audiodata; 
    private void OnDestroy()
    {
        if (audiodata != null)
        {
            audiodata.audioPlaying = false;
        }
        

    }

    public void Start()
    {
        SetData();
    }

    public void SetData()
    {
        audiodata = AudioManager.Instance.audiodata;
    }


}
