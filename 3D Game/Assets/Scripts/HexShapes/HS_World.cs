using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HS_World : HexShape
{
    [SerializeField] private GridTileSO GridTileSO;

    public GridTileSO MyGridTileSO
    {
        get { return GridTileSO; }
        set { GridTileSO = value; }
    }

}
