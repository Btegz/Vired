using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class AudioData : ScriptableObject
{
    public AudioClip audioClip;
    public float volume =0.01f;
    public bool audioPlaying = false;
}
