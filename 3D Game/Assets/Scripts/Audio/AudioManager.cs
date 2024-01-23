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
    public AudioSource EnemyHovern;



    public void Awake()
    {
        Instance = this;
        
    }
    public void PlaySoundAtLocation(AudioData data, AudioMixerGroup output, string path)
    {
        if (!PlayerManager.Instance.AbilityLoadoutActive)
        {
            GameObject audioPlayer = new GameObject();
            AudioSource audioSource = audioPlayer.AddComponent<AudioSource>();

            try
            {
                audios = Resources.LoadAll(path, typeof(AudioData));

                audio = (AudioData)audios[Random.Range(0, audios.Length)];
                audioSource.clip = audio.audioClip;
                if (PlayerManager.Instance.AbilityLoadoutActive)
                    audioSource.volume = 0;
                else
                {
                    audioSource.volume = audio.volume;
                    audioSource.Play();
                    audioSource.outputAudioMixerGroup = output;
                }

            }
            catch
            {
                if (PlayerManager.Instance.AbilityLoadoutActive)
                    audioSource.volume = 0;
                else
                {
                    audioSource.clip = data.audioClip;
                    audioSource.volume = data.volume;
                    audioSource.Play();
                    audioSource.outputAudioMixerGroup = output;
                }
            }

            //Destroy(audioPlayer.gameObject, data.audioClip.length);
        }
    }
    public void PlayMusic(AudioSource _audioSource)
    {
       // if (!PlayerManager.Instance.AbilityLoadoutActive)
        
            if (_audioSource.isPlaying) return;
            _audioSource.Play();
        
    }

    public void StopMusic(AudioSource _audioSource)
    {
        
            _audioSource.Stop();
        
    }


}
