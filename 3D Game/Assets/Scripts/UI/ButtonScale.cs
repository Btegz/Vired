using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.Animations;

public class ButtonScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Material glitchMaterial;
    public Image turnNextButton;
    public Animator animation;
    public string AnimationName;


    public void Start()
    {
        animation = GetComponent<Animator>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
       // transform.DOScale(1.08f, 0.15f);
        StartCoroutine(GlitchCorroutine());
       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       // transform.DOScale(1f, 0.1f);
        animation.SetBool(AnimationName, false);

    }

    IEnumerator GlitchCorroutine()
    {
        animation.SetBool(AnimationName, true);
        yield return new WaitForSeconds(0.5f);
        animation.SetBool(AnimationName, false);
    }
}

    

