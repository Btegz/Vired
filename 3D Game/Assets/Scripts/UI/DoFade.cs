using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class DoFade : MonoBehaviour
{
    public TextMeshProUGUI menuText;
    public Image FadeImage;
    public GameObject FadeImageObject;

    public float duration;
    public float time;

    public float duartionImage;
    public float timeImage;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Fading());
        StartCoroutine(FadeOut());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public  IEnumerator Fading()
    {
        yield return new WaitForSeconds(time);
        menuText.DOFade(1, duration);
    }

    public IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(timeImage);
        FadeImage.DOFade(0, duartionImage);
        yield return new WaitForSeconds(timeImage);
        FadeImageObject.SetActive(false);
      
    }

    public IEnumerator FadingExit()
    {
        yield return new WaitForSeconds(time);
        menuText.DOFade(0, duration);
    }

    public IEnumerator FadeOutExit()
    {
        yield return new WaitForSeconds(timeImage);
        FadeImage.DOFade(1, duartionImage);

    }


   
}


