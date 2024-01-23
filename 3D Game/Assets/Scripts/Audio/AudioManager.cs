using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    //  [SerializeField] AudioSource audioPlayerPrefab;
    new Object[] audios;
    new AudioData audio;



    public void Awake()
    {
        Instance = this;
    }
    public void PlaySoundAtLocation(AudioData data, AudioMixerGroup output, string path)
    {
        GameObject audioPlayer = new GameObject();
        AudioSource audioSource = audioPlayer.AddComponent<AudioSource>();

        try
        {
            audios = Resources.LoadAll(path, typeof(AudioData));
          
            audio = (AudioData)audios[Random.Range(0, audios.Length)];
            Debug.Log(audio);
            audioSource.clip = audio.audioClip;
            audioSource.volume = audio.volume;
            audioSource.Play();
            audioSource.outputAudioMixerGroup = output;

        }
        catch
        {

            audioSource.clip = data.audioClip;
            audioSource.volume = data.volume;
            audioSource.Play();
            audioSource.outputAudioMixerGroup = output;
        }

      Destroy(audioPlayer.gameObject, data.audioClip.length);
    }
    public void PlayMusic(AudioSource _audioSource)
    {
        if (_audioSource.isPlaying) return;
        _audioSource.Play();
    }

    public void StopMusic(AudioSource _audioSource)
    {
        _audioSource.Stop();
    }


}
