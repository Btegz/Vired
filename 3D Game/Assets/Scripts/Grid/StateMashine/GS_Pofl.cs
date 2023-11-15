using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GS_Pofl", menuName = "GridStates/GS_Pofl")]

public class GS_Pofl : GridState
{
    [SerializeField] GameObject PointOfInterest;
    [SerializeField] public GameObject pofi;
 
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
        throw new System.NotImplementedException();
    }

    public override void PlayerEnters(GridTile parent)
    {
       
        pofi = Instantiate(PointOfInterest, PointOfInterest.transform.position, Quaternion.identity);
  
    }

    public override int StateValue()
    {
        return 4;
    }
}
