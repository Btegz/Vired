using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Highlight : MonoBehaviour
{
    Color color;
    public IEnumerator highlight;

    public void OnEnable()
    {
        color = GetComponent<Image>().color;
        highlight = HighlightFunction();
        if (TutorialManager.Instance.enabledIsRunning)
        {
            StartCoroutine(highlight);
        }
       
    }

    public void Start()
    {
        
    }



    public IEnumerator HighlightFunction()
    {

        GetComponent<RectTransform>().DOScale(1.05f, 1f);
        GetComponent<Image>().DOFade(1f, 1f);
        GetComponent<Image>().DOColor(Color.white, 1f);
        yield return new WaitForSeconds(1f);
        GetComponent<RectTransform>().DOScale(1.0f, 1f);
        GetComponent<Image>().DOFade(0f, 1f);
        yield return null;


    }


}
