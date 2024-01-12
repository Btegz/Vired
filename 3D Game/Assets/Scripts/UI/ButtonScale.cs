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

        turnNextButton.material = glitchMaterial;


    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1f, 0.1f);
        turnNextButton.material = null;
    }
}

    

