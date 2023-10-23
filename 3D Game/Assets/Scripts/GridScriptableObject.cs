using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GridScriptableObject : ScriptableObject
{
    [SerializeField] public List<Vector2Int> GridCoords;
    [SerializeField] public List<GridTile> GridTiles;

    [SerializeField] GridTile gridTilePrefab;

    [SerializeField] public Dictionary<Vector2Int, GridTile> Grid
    {
        get {
            Dictionary<Vector2Int, GridTile> returnGrid = new Dictionary<Vector2Int, GridTile>();
            for(int i = 0; i<GridCoords.Count; i++)
            {
                if (GridTiles[i] == null)
                {
                    continue;
                }
                returnGrid.Add(GridCoords[i], GridTiles[i]);
            }
            return returnGrid;
        }
        set
        {
            GridCoords = new List<Vector2Int>();
            GridTiles = new List<GridTile>();
            foreach(KeyValuePair<Vector2Int,GridTile> kvp in value)
            {
                GridCoords.Add(kvp.Key);
                GridTiles.Add(kvp.Value);
            }
        }
    }



}
