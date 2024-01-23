using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "GS_Positive", menuName = "GridStates/GS_Positive")]
public class GS_positive : GridState
{
    public AudioMixerGroup soundEffect;
    public AudioData ResourceCollected; 
    public override GridState CurrentState()
    {
        return this;
    }

    public void Start()
    {
    }
    public override void EnterState(GridTile parent)
    {
        parent.GetComponent<RessourceVisuals>().InfestKlopse();
        Debug.Log("I ENTER POSITIVE");

        switch (parent.ressource)
        {
            case Ressource.ressourceA:
                parent.meshRenderer.material = parent.gridTileSO.resourceAMaterial; break;
            case Ressource.ressourceB:
                parent.meshRenderer.material = parent.gridTileSO.resourceBMaterial; break;
            case Ressource.ressourceC:
                parent.meshRenderer.material = parent.gridTileSO.resourceCMaterial; break;
            case Ressource.ressourceD:
                parent.meshRenderer.material = parent.gridTileSO.resourceDMaterial; break;
        }
        parent.transform.DOComplete();

        parent.transform.DOPunchRotation(Vector3.one * TweenScale, .5f);
    }

    public override void ExitState(GridTile parent)
    {
        parent.GetComponent<RessourceVisuals>().CleanUpKlopse();
        //throw new System.NotImplementedException();
    }

    public override void PlayerEnters(GridTile parent)
    {
        RessourceGainEffect rsg = Instantiate(PlayerManager.Instance.ressourceGainEffect,UIManager.Instance.transform);
        AudioManager.Instance.PlaySoundAtLocation(ResourceCollected, soundEffect, null);
        SaveManager.Instance.TotalResources++;
        Ressource res = Ressource.ressourceD;
        Vector2 startPoint = Camera.main.WorldToScreenPoint(parent.transform.position);
        Vector2 goalPoint = new Vector2();
        switch (parent.ressource)
        {
            case Ressource.ressourceA:
                PlayerManager.Instance.RessourceAInventory++;
                goalPoint = UIManager.Instance.ressourceAText.transform.position;
                res = Ressource.ressourceA;
                break;
            case Ressource.ressourceB:
                PlayerManager.Instance.RessourceBInventory++;
                goalPoint = UIManager.Instance.ressourceBText.transform.position;
                res = Ressource.ressourceB;
                break;
            case Ressource.ressourceC:
                PlayerManager.Instance.RessourceCInventory++;
                goalPoint = UIManager.Instance.ressourceCText.transform.position;
                res = Ressource.ressourceC;
                break;
            case Ressource.ressourceD:
                PlayerManager.Instance.RessourceDInventory++;
                goalPoint = UIManager.Instance.ressourceDText.transform.position;
                res = Ressource.ressourceD;
                break;
        }
        parent.ChangeCurrentState(GridManager.Instance.gS_Neutral);
        rsg.Initialize(res, startPoint, goalPoint);
        


    }

    public override int StateValue()
    {
        return 1;
    }

    
}
