using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ButtonScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{ 
    [SerializeField] Button button;

    public void OnPointerEnter(PointerEventData eventData)
    {
        button.transform.DOScale(1.08f, 0.15f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        button.transform.DOScale(1f, 0.1f);
    }
}

    

