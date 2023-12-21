using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RessourceHighlight : MonoBehaviour
{
    Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        //transform.DOPunchScale(Vector3.one*1.2f,.25f);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(startPosition + Vector3.up * 50, 1f));
        sequence.Append(transform.DOMove(startPosition, 1f));

        sequence.SetLoops(-1);
        sequence.Play();

    }
}
