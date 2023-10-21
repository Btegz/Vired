using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HS_World : HexShape
{
    [SerializeField] private Ressource myRessource;

    public Ressource MyRessource
    {
        get { return myRessource; }
        set { myRessource = value; }
    }

}
