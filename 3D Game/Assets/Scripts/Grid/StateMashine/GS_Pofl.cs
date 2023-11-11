using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GS_Pofl", menuName = "GridStates/GS_Pofl")]

public class GS_Pofl : GridState
{
    [SerializeField] GameObject PointOfInterest;
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
        Instantiate(PointOfInterest, PointOfInterest.transform.position, Quaternion.identity);
        parent.ChangeCurrentState(GridManager.Instance.gS_Neutral);
    }

    public override int StateValue()
    {
        return 4;
    }
}
