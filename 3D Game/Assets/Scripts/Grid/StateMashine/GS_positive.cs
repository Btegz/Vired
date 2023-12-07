using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GS_Positive", menuName = "GridStates/GS_Positive")]
public class GS_positive : GridState
{

    public override GridState CurrentState()
    {
        return this;
    }

    public override void EnterState(GridTile parent)
    {
        switch (parent.ressource)
        {
            case Ressource.ressourceA:
                parent.meshRenderer.material = parent.gridTileSO.resourceAMaterial; break;
            case Ressource.ressourceB:
                parent.meshRenderer.material = parent.gridTileSO.resourceBMaterial; break;
            case Ressource.ressourceC:
                parent.meshRenderer.material = parent.gridTileSO.resourceCMaterial; break;
            case Ressource.resscoureD:
                parent.meshRenderer.material = parent.gridTileSO.resourceDMaterial; break;
        }
        parent.transform.DOComplete();

        parent.transform.DOPunchRotation(Vector3.one * TweenScale, .5f);

    }

    public override void ExitState(GridTile parent)
    {
        //throw new System.NotImplementedException();
    }

    public override void PlayerEnters(GridTile parent)
    {
        RessourceGainEffect rsg = Instantiate(PlayerManager.Instance.ressourceGainEffect);
        switch (parent.ressource)
        {
            case Ressource.ressourceA:
                PlayerManager.Instance.RessourceAInventory++;
                
                break;
            case Ressource.ressourceB:
                PlayerManager.Instance.RessourceBInventory++;
                break;
            case Ressource.ressourceC:
                PlayerManager.Instance.RessourceCInventory++;
                break;
            case Ressource.resscoureD:
                PlayerManager.Instance.RessourceDInventory++;
                break;
        }
        parent.ChangeCurrentState(GridManager.Instance.gS_Neutral);
    }

    public override int StateValue()
    {
        return 1;
    }
}
