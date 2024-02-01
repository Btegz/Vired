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
    public AudioData audiodata;
    AudioSource audioSource;



    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }





    }
    public void PlaySoundAtLocation(AudioData data, AudioMixerGroup output, string path, bool InGame)
    {

        GameObject audioPlayer = new GameObject();
        AudioSource audioSource = audioPlayer.AddComponent<AudioSource>();

        audioPlayer.AddComponent<AudioDestroy>();

        audiodata = data;


        try
        {
            audios = Resources.LoadAll(path, typeof(AudioData));


            audio = (AudioData)audios[Random.Range(0, audios.Length)];


            audioSource.clip = audio.audioClip;

            audioSource.volume = audio.volume;
            if (PlayerManager.Instance.AbilityLoadoutActive && InGame)
                audioSource.volume = 0;
            audioSource.Play();
            audioSource.outputAudioMixerGroup = output;
            data.audioPlaying = true;



        }
        catch
        {

            if (audioSource != null)
            {
                audioSource.clip = data.audioClip;
                audioSource.volume = data.volume;
                if (PlayerManager.Instance.AbilityLoadoutActive && InGame)
                    audioSource.volume = 0;

                audioSource.Play();
                audioSource.outputAudioMixerGroup = output;
                data.audioPlaying = true;
            }





        }





        Destroy(audioPlayer.gameObject, audioSource.clip.length);





    }

    public void PlayMusic(AudioSource _audioSource)
    {
        // if (!PlayerManager.Instance.AbilityLoadoutActive)

        if (_audioSource.isPlaying)
        {
            return;
        }

        else
        {
            _audioSource.Play();

        }

    }

    public void StopMusic(AudioSource _audioSource)
    {

        _audioSource.Stop();

    }


}
