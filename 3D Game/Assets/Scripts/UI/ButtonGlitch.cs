using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonGlitch : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator animation;
    public string AnimationName;
    public void Start()
    {
        animation = GetComponent<Animator>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {

        StartCoroutine(GlitchCorroutine());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animation.SetBool(AnimationName, false);
    }

    IEnumerator GlitchCorroutine()
    {
        if (animation != null)
        {
            animation.SetBool(AnimationName, true);
            yield return new WaitForSeconds(0.5f);
            animation.SetBool(AnimationName, false);
        }
        yield return null;
    }
}
