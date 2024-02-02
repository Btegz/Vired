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
    
    public ToggleObj toggle;

    public Toggle TutorialToggle;

    public Button creditsBack;
    public Button exitButton;

    public List<AudioSource> musicSounds;
    public GameObject SoundManager;




    // Start is called before the first frame update


    public void Start()
    {


        musicSounds = new List<AudioSource>();

      //  gameStartButton.onClick.AddListener(Sound);
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


    }

    private void Update()
    {
        musicSounds = SoundManager.GetComponentsInChildren<AudioSource>().ToList();

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
        Application.OpenURL("https://moka-maceli.itch.io/vired");
    }


    public void OpenSurvey()
    {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSfnQNGLuM9T29VjpL54FPPSlZJIb2X9sCEUejPXU7SHwiTivw/viewform");
    }
  /*  public IEnumerator LoadASync()
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
  */
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


  

