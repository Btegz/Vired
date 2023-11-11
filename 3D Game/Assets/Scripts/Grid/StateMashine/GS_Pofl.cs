using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GS_Pofl", menuName = "GridStates/GS_Pofl")]

public class GS_Pofl : GridState
{
    [SerializeField] GameObject PointOfInterest;
    [SerializeField] EventButton eventButton;
    [SerializeField] PofIManager PofI;
    public override GridState CurrentState()
    {
        return this;
    }

    public override void EnterState(GridTile parent)
    {
        parent.meshRenderer.material = parent.gridTileSO.PoflMaterial;

    }

    public override void ExitState(GridTile parent)
    {
        throw new System.NotImplementedException();
    }

    public override void PlayerEnters(GridTile parent)
    {
        parent.ChangeCurrentState(GridManager.Instance.gS_Neutral);
        GameObject pofi =  Instantiate(PointOfInterest, PointOfInterest.transform.position, Quaternion.identity);
        /*Debug.Log(PofI.pofi1);
        pofi.transform.GetChild(PofI.pofi1).gameObject.SetActive(true);
        pofi.transform.GetChild(PofI.pofi2).gameObject.SetActive(true);
        */

        
    }

    public override int StateValue()
    {
        return 4;
    }
}
