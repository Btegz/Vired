using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class InfoTextPopUp : MonoBehaviour
{
    [SerializeField]TMP_Text textField;

    public string Text;
    // Start is called before the first frame update
    void Start()
    {
        textField.text = Text;
        transform.DOPunchScale(Vector3.one * 1.2f, 1f);
        textField.DOFade(0, 2f).SetEase(Ease.OutExpo).OnComplete(() => Destroy(gameObject));
    }
}
