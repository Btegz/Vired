using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "GS_Pofl", menuName = "GridStates/GS_Pofl")]

public class GS_Pofl : GridState
{
    [SerializeField] GameObject PointOfInterest;
    [SerializeField] public GameObject pofi;
    public AudioData PofISound;
    public AudioMixerGroup soundEffect;

 
    public override GridState CurrentState()
    {
        return this;
    }

    public override void EnterState(GridTile parent)
    {
        parent.meshRenderer.material = parent.gridTileSO.PofIMaterial;

    }

    public override void ExitState(GridTile parent)
    {

    }

    public override void PlayerEnters(GridTile parent)
    {
        PlayerManager.Instance.move = false;
        pofi = Instantiate(PointOfInterest, PointOfInterest.transform.position, Quaternion.identity);
        parent.ChangeCurrentState(GridManager.Instance.gS_Neutral);
        Destroy(parent.gameObject.GetComponentInChildren<PofIVisuals>().gameObject);

        AudioManager.Instance.PlaySoundAtLocation(PofISound, soundEffect, null);  
    }

    public override int StateValue()
    {
        return 4;
    }
}
