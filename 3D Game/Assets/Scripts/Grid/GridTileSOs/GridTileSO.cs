using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GridTileSO : ScriptableObject
{
    public GridState initialGridState;
    public Ressource ressource;

    [Header("Materials")]
    public Material resourceAMaterial;
    public Material resourceBMaterial;
    public Material resourceCMaterial;
    public Material resourceDMaterial;
    public Material neutralMaterial;
    public Material negativeMaterial;
    public Material PofIMaterial;

}
