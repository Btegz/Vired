using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ButtonScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Material glitchMaterial;
    public Image turnNextButton;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(1.08f, 0.15f);
        //StartCoroutine(GlitchCorroutine());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1f, 0.1f);
       // StopCoroutine(GlitchCorroutine());
       
    }

    IEnumerator GlitchCorroutine()
    {
        turnNextButton.material = glitchMaterial;
        yield return new WaitForSeconds(0.2f);
        turnNextButton.material = null;
    }
}

    

