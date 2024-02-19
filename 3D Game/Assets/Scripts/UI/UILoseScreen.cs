using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class UILoseScreen : MonoBehaviour
{
    private TextMeshProUGUI LoseTMP;
    // Start is called before the first frame update
    void Awake()
    {
        LoseTMP = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
       // LoseTMP.DOFade(0f, 0f).SetDelay(0.5f); // Setze die Alpha auf 0 und verzögere um eine Sekunde
        LoseTMP.DOFade(1f, 1f).SetDelay(0.5f).From(0);
        transform.DOScale(1, 2f).From(0);
        //LoseTMP.DOColor("FF0003", 0.05f);
    }


}
