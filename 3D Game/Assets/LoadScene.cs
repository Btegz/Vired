using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{

    public AudioSource FirstSecond;
    public AudioSource Third;
    public AudioSource Fourth;

    public GameObject LoadingScreenImage;

    public Animator Loading;
    public GameObject LoadingScreenAnimation;
    public float steps;
    public ToggleObj TutorialObj;
    public string scene;
    public GameObject confetti;
    public GameObject icons;



    private void Start()
    {
        DontDestroyOnLoad(LoadingScreenImage);
        DontDestroyOnLoad(Loading);
        DontDestroyOnLoad(LoadingScreenImage);

        Loading.speed = 0.2f;
        if(confetti != null && icons != null)
        {
        confetti.SetActive(true);
        icons.SetActive(true);
        }
        

    }

    public IEnumerator SoundFade()
    {
        //if (PlayerPrefs.HasKey("Volume"))
        //          FirstSecond.volume=  PlayerPrefs.GetFloat("Volume");

    

        for (int i = 1; i <= (int)steps; i++)
        {

            FirstSecond.volume = Mathf.Lerp(0, 1, 1 - i / steps);
            PlayerPrefs.SetFloat("Volume", FirstSecond.volume);
            yield return new WaitForSeconds(0.5f);

        }


        //   PlayerPrefs.SetFloat("Volume", 1);

        yield return new WaitForSeconds(1f);


    }
    public IEnumerator LoadingScreen()
    {
     

        AsyncOperation async = SceneManager.LoadSceneAsync("MainScene");

        while (!async.isDone)
        {

            Loading.SetBool("IsLoading", true);
            yield return null;
        }
    }

        public IEnumerator Wait()
    {
        LoadingScreenImage.SetActive(true);
        Color colorBackground = LoadingScreenImage.GetComponent<Image>().color;
        while(colorBackground.a <1)
        {
            colorBackground.a += 0.1f;
            LoadingScreenImage.GetComponent<Image>().color = colorBackground;
            yield return new WaitForSeconds(0.01f);
        } 
        
       /* Color color = Loading.GetComponent<Image>().color;
       
        while(color.a <1)
        {
            color.a += 0.1f;
            Loading.GetComponent<Image>().color = color;
            yield return new WaitForSeconds(0.01f);
        }
       */
    

        

        Loading.SetBool("IsLoading", true);
        yield return new WaitForSeconds(3f);

        StartCoroutine(LoadingScreen());
    }

    public IEnumerator LoadingScreen2()
    {


        AsyncOperation async = SceneManager.LoadSceneAsync("Tutorial");

        while (!async.isDone)
        {

            Loading.SetBool("IsLoading", true);
            yield return null;
        }
    }

    public IEnumerator Wait2()
    {
        LoadingScreenImage.SetActive(true);
        Color colorBackground = LoadingScreenImage.GetComponent<Image>().color;
        while (colorBackground.a < 1)
        {
            colorBackground.a += 0.1f;
            LoadingScreenImage.GetComponent<Image>().color = colorBackground;
            yield return new WaitForSeconds(0.01f);
        }

        /* Color color = Loading.GetComponent<Image>().color;

         while(color.a <1)
         {
             color.a += 0.1f;
             Loading.GetComponent<Image>().color = color;
             yield return new WaitForSeconds(0.01f);
         }
        */




        Loading.SetBool("IsLoading", true);
        yield return new WaitForSeconds(3f);

        StartCoroutine(LoadingScreen2());
    }

    public void Sound2()
    {
        if (confetti != null)
        {
            confetti.SetActive(false);
        }
        if (icons != null)
        {
            icons.SetActive(false);
        }
        StartCoroutine(Wait2());


    }

    public void Sound()
        {
        if (confetti != null)
        {
            confetti.SetActive(false);
        }
        if(icons != null)
        {
            icons.SetActive(false);
        }
        if (TutorialObj.tutorial)
            StartCoroutine(Wait2());
        else if (!TutorialObj.tutorial)
        StartCoroutine(Wait());


        }
    }
