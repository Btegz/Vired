using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.Animations;

public class ButtonScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Highlight; 
    public void OnPointerEnter(PointerEventData eventData)
    {

        
        if (TutorialManager.Instance.tutorial)
        {
           
            TutorialManager.Instance.StopCoroutine(TutorialManager.Instance.enabled);
            if (Highlight.GetComponent<Highlight>().highlight != null)
            {
                TutorialManager.Instance.StopCoroutine(Highlight.GetComponent<Highlight>().highlight);
            }
            TutorialManager.Instance.enabledIsRunning = false;
            Highlight.SetActive(true);
            Highlight.GetComponent<Image>().DOFade(1f, 1f);
            TutorialManager.Instance.InventoryHighlight.SetActive(true);
            TutorialManager.Instance.InventoryHighlight.GetComponent<Image>().DOFade(1f, 1f); 

          

        }
        else
        {
            transform.DOScale(1.08f, 0.15f);
        }


    }

    public void OnPointerExit(PointerEventData eventData)
    {

        if (TutorialManager.Instance.tutorial)
        {
            if (Highlight != null)
            {
                Highlight.GetComponent<Image>().DOFade(0f, 1f);

                Highlight.SetActive(false);
                TutorialManager.Instance.InventoryHighlight.SetActive(false);
                TutorialManager.Instance.InventoryHighlight.GetComponent<Image>().DOFade(0f, 1f);
            }
          
        }
        else
        {
              transform.DOScale(1f, 0.1f);
        }
      


    }

}




