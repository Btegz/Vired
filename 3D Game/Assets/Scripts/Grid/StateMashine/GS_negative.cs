using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName ="GS_Negative",menuName ="GridStates/GS_Negative")]
public class GS_negative : GridState
{
    public AudioMixerGroup soundEffect;
    public AudioData SpreadNegativity;
    public override GridState CurrentState()
    {
        return this;
    }

    public override void EnterState(GridTile parent)
    {
        //parent.meshRenderer.material = parent.gridTileSO.negativeMaterial;
        parent.transform.DOComplete();
        parent.GetComponent<RessourceVisuals>().CleanUpKlopse();

        parent.GetComponent<RessourceVisuals>().SpawnEnemyMass();
        foreach(GridTile neighbor in parent.myNeighbors)
        {
            if (neighbor.currentGridState.StateValue() == -1)
            {
                neighbor.GetComponent<RessourceVisuals>().SpawnEnemyMass();
            }
        }

        //parent.transform.DOPunchRotation(Vector3.one*TweenScale,.5f);
        AudioManager.Instance.PlaySoundAtLocation(SpreadNegativity, soundEffect, null, true);


    

}

    public override void ExitState(GridTile parent)
    {
        parent.GetComponent<RessourceVisuals>().DestroyEnemyMasses();
    }

    public override void PlayerEnters(GridTile parent)
    {
        
        //tirgger Ressource loss for player
    }

    public override int StateValue()
    {
        return -1;
    }
}
