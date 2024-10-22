using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MovePointsDoTween : MonoBehaviour
{

    public RectTransform rectTransform;
    public Sprite usedMovepoints;
    public Sprite movepoints;
    public string AnimationName;
    public Animator animator; 


    public void Away ()
    {
        rectTransform.DOComplete();
        //rectTransform.transform.localPosition = new Vector3(-100f, 0f, 0f);
        //rectTransform.DOAnchorPos(new Vector2(1000f, 10f), 1f, false).SetEase(Ease.OutElastic);
        //rectTransform.DOPunchAnchorPos(new Vector2(20f, 0f), 1f).SetEase(Ease.OutElastic);
        rectTransform.DOPunchPosition(new Vector2(20f, 0f), 1f).SetEase(Ease.OutElastic);

        Image image = GetComponent<Image>();
        image.sprite = usedMovepoints;
        
        //image.sprite = Movepoints;
        //usedMovepoints.SetActive(true);

    }

    public void SpriteReset()
    {
        rectTransform.DOComplete();
        rectTransform.DOPunchPosition(new Vector2(20f, 0f), 1f).SetEase(Ease.OutElastic);

        Image image = GetComponent<Image>();
        image.sprite = movepoints;
  
    }

 
}
