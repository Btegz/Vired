using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPreviewTile : MonoBehaviour
{
    [SerializeField] GameObject PreviewParent;
    [SerializeField] GameObject ParticleParent;

    public AbilityObjScript parent;

    [SerializeField] float AnimationDuration;

    private void Start()
    {
        PreviewParent.SetActive(true);
        ParticleParent.SetActive(false);
    }

    public void Shoot()
    {
        if (parent == null) { return; }

        PreviewParent.SetActive(false);

        ParticleParent.transform.position = parent.transform.position;
        ParticleParent.SetActive(true);
        foreach (ParticleSystem part in ParticleParent.GetComponentsInChildren<ParticleSystem>())
        {
            part.Play();
        }
        ParticleParent.transform.DOJump(transform.position, 2, 1, AnimationDuration).OnComplete(() => parent.shooting = false);
    }
}
