using Coffee.UIExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RessourceGainEffect : MonoBehaviour
{
    [SerializeField]UIParticle RessourceAPart;
    [SerializeField]UIParticle RessourceBPart;
    [SerializeField]UIParticle RessourceCPart;
    [SerializeField]UIParticle RessourceDPart;

    public AudioData collectResource;

    public void Initialize(Ressource ressource, Vector2 start, Vector2 goal)
    {
        transform.position = start;
        switch (ressource)
        {
            case Ressource.ressourceA:
                RessourceAPart.Play();
                break;
            case Ressource.ressourceB:
                RessourceBPart.Play();
                break;
            case Ressource.ressourceC:
                RessourceCPart.Play();
                break;
            case Ressource.resscoureD:
                RessourceDPart.Play();
                break;
        }

        AudioManager.Instance.PlaySoundAtLocation(collectResource);
        transform.DOJump(goal, 200,1, 2f).OnComplete(()=>Destroy(gameObject));

    }
}
