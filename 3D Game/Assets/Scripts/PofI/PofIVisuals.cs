using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PofIVisuals : MonoBehaviour
{
    [SerializeField] Vector3 rotationPunch;

    [SerializeField] float tweenDuration;
    [SerializeField] float Elevation;

    [SerializeField] List<ParticleSystem> particles;

    [SerializeField] AnimationCurve RotationCurve;
    [SerializeField] AnimationCurve ElevationCurve;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FlyAway()
    {
        Sequence tweenSequence = DOTween.Sequence();

        foreach(ParticleSystem part in particles)
        {
            part.Play();
        }
        transform.DOPunchRotation(rotationPunch, tweenDuration, 50, 1).SetEase(RotationCurve);
        tweenSequence.Append(transform.DOMoveY(transform.position.y + Elevation, tweenDuration).SetEase(ElevationCurve));
        tweenSequence.Append(transform.DOMoveY(200, 10).OnComplete(() => Destroy(gameObject)));

    }
}
