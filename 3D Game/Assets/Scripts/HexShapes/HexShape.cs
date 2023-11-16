using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HexShape : ScriptableObject
{
    [SerializeField] private List<Vector2Int> myCoordinates;

    [SerializeField] public GridTile tileprefab;

    public List<Vector2Int> Coordinates
    {
        get { return myCoordinates; }
        set { myCoordinates = value; }
    }


}
