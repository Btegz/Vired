using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class MainMenu : MonoBehaviour
{
    public GameObject optionCanvas;
    public GameObject CreditCanvas;
    public GameObject mainMenuCanvas;
    public Button gameStartButton;
    public Button optionsButton;
    public Toggle tutorialToggle;
    public ToggleObj TutorialObj;
    public Image Fade;
    public DoFade doFade;
    public float steps;
    public ToggleObj toggle;

    public Toggle TutorialToggle;

    public Button creditsBack;

    public List<AudioSource> musicSounds;
    public GameObject SoundManager;
    public AudioSource FirstSecond;
    public AudioSource Third;
    public AudioSource Fourth;

    public Button exitButton;
    // Start is called before the first frame update

    
    public void Start()
    {
        musicSounds = new List<AudioSource>();

        gameStartButton.onClick.AddListener(Sound);
        optionsButton.onClick.AddListener(OptionsSwitch);
        exitButton.onClick.AddListener(ExitGame);
        creditsBack.onClick.AddListener(ExitBack);


        if (PlayerPrefs.HasKey("Tutorial"))
        {
            if (PlayerPrefs.GetInt("Tutorial") == 1)
            {
                TutorialObj.tutorial = true;
            }
            else
            {

                TutorialObj.tutorial = false;

            }
        }
        if (PlayerPrefs.HasKey("Tutorial"))
        {
            if (PlayerPrefs.GetInt("Tutorial") == 1)
            {
                Debug.Log(PlayerPrefs.GetInt("Tutorial"));
                TutorialToggle.isOn = true;
            }

            else
            {
                TutorialToggle.isOn = false;
            }
        }

        musicSounds.Add(FirstSecond);
        musicSounds.Add(Third);
        musicSounds.Add(Fourth);

    }

    private void Update()
    {
        musicSounds = SoundManager.GetComponentsInChildren<AudioSource>().ToList();

    }
    public IEnumerator SoundFade()
    {
        //if (PlayerPrefs.HasKey("Volume"))
        //          FirstSecond.volume=  PlayerPrefs.GetFloat("Volume");
     
            
           for(int i= 1; i <=(int) steps; i++)
            {

                FirstSecond.volume = Mathf.Lerp(0, 1,1-i / steps);  
                PlayerPrefs.SetFloat("Volume", FirstSecond.volume);
                yield return new WaitForSeconds(0.5f);
               
            }


        //   PlayerPrefs.SetFloat("Volume", 1);

        yield return new WaitForSeconds(1f);

        if (TutorialObj.tutorial == true)
        {
            SceneManager.LoadScene("Tutorial");
        }
        else
        {
            StartCoroutine(LoadASync());

        }
    }

    public void Sound()
    {
        StartCoroutine(SoundFade());
    

    }


    public void Tutorial()
    {
        if (TutorialToggle.isOn)
        {
            toggle.tutorial = true;
            PlayerPrefs.SetInt("Tutorial", 1);
        }

        else
        {
            toggle.tutorial = false;
            PlayerPrefs.SetInt("Tutorial", 0);

        }
    }


    public void OptionsSwitch()
    {
        gameObject.SetActive(false);
        optionCanvas.SetActive(true);

    }

    public void TutorialTooltip()
    {
        if (tutorialToggle.isOn == true)
        {
            TutorialObj.tutorial = true;
            PlayerPrefs.SetInt("Tutorial", 1);
            
        }

        else
        {
            TutorialObj.tutorial = false;
            PlayerPrefs.SetInt("Tutorial", 0);
        }
    }

    public void Credits()
    {
        gameObject.SetActive(false);
        CreditCanvas.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }


    public void OpenLink()
    {
        Application.OpenURL("https://itch.io/");
    }


    public void OpenSurvey()
    {
        Application.OpenURL("https://itch.io/");
    }
    public IEnumerator LoadASync()
    {
       Color color = Fade.color;
       
        while (color.a <1)
        {
            color.a += 0.1f;
            Fade.color = color;
            yield return new WaitForSeconds(0.01f);
        }

        SceneManager.LoadSceneAsync("MainScene");
        yield return null;

    }

    public void ExitBack()
    {
        mainMenuCanvas.SetActive(true);
        CreditCanvas.SetActive(false);
    }

    public void FadeStart()
    {
        gameStartButton.interactable = true;
    }
}
