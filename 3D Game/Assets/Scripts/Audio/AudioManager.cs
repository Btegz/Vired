using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
  //  [SerializeField] AudioSource audioPlayerPrefab;
  



    public void Awake()
    {
        Instance = this;
    }
    public void PlaySoundAtLocation(AudioData data, AudioMixerGroup output)
    {
        GameObject audioPlayer = new GameObject();
        AudioSource audioSource = audioPlayer.AddComponent<AudioSource>();
   
        audioSource.clip = data.audioClip;
        audioSource.volume = data.volume;
        audioSource.Play();
        audioSource.outputAudioMixerGroup = output;

       Destroy(audioPlayer.gameObject, data.audioClip.length);
    }
    
 
}
