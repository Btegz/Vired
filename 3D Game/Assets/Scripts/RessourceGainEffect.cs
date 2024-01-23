using Coffee.UIExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Audio; 

public class RessourceGainEffect : MonoBehaviour
{
    [SerializeField] UIParticle RessourceAPart;
    [SerializeField] UIParticle RessourceBPart;
    [SerializeField] UIParticle RessourceCPart;
    [SerializeField] UIParticle RessourceDPart;

    [SerializeField] UIParticle EffectAfterRessourceGain;

    public AudioData collectResource;
    public AudioMixerGroup soundEffect;
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
            case Ressource.ressourceD:
                RessourceDPart.Play();
                break;
        }

        AudioManager.Instance.PlaySoundAtLocation(collectResource, soundEffect, null);
        transform.DOJump(goal, 200,1, 2f).OnComplete(()=> Destroy(gameObject));

        Invoke("completedRessourceGain", 1.5f);

    }

    public void completedRessourceGain()
    {
        UIParticle gain = Instantiate(EffectAfterRessourceGain, UIManager.Instance.transform);
        //UIParticle gain = Instantiate(EffectAfterRessourceGain, GridManager.Instance.Grid[PlayerManager.Instance.selectedPlayer.CoordinatePosition].transform);
        GridTile parent = GridManager.Instance.Grid[PlayerManager.Instance.selectedPlayer.CoordinatePosition];

        switch (parent.ressource)
        {
            case Ressource.ressourceA:
                
                gain.transform.position = UIManager.Instance.ressourceAText.transform.position;
                break;
            case Ressource.ressourceB:
                gain.transform.position = UIManager.Instance.ressourceBText.transform.position;
                break;
            case Ressource.ressourceC:
                gain.transform.position = UIManager.Instance.ressourceCText.transform.position;
                break;
            case Ressource.ressourceD:
                gain.transform.position = UIManager.Instance.ressourceDText.transform.position;
                break;
        }

    }
}
