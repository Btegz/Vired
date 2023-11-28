using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Runtime.ConstrainedExecution;
using log4net.Core;


public class EditorGridGeneration
{
    [MenuItem("Utilities/Generate Grid Assets")]
    public static void GenerateDefaultEmptyGrid()
    {
        GameObject gridManagerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/GridManager.prefab");

        GameObject GridTilePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/GridTile.prefab");

        GameObject gridManager = PrefabUtility.InstantiatePrefab(gridManagerPrefab) as GameObject;
        List<GridTileSO> gridTileSOs = new List<GridTileSO>()
        {
            AssetDatabase.LoadAssetAtPath<GridTileSO>("Assets/Scripts/Grid/GridTileSOs/RessourceAGridTileSO.asset"),
            AssetDatabase.LoadAssetAtPath<GridTileSO>("Assets/Scripts/Grid/GridTileSOs/RessourceBGridTileSO.asset"),
            AssetDatabase.LoadAssetAtPath<GridTileSO>("Assets/Scripts/Grid/GridTileSOs/RessourceCGridTileSO.asset"),
            AssetDatabase.LoadAssetAtPath<GridTileSO>("Assets/Scripts/Grid/GridTileSOs/RessourceDGridTileSO.asset")
        };

        Dictionary<Vector2Int, GridTile> Grid = new Dictionary<Vector2Int, GridTile>();
        List<Vector2Int> coords = HexGridUtil.GenerateRectangleShapedGrid(15, 15);

        foreach (Vector2Int coord in coords)
        {
            GameObject newTileObj = PrefabUtility.InstantiatePrefab(GridTilePrefab) as GameObject;
            GridTile newTile = newTileObj.GetComponent<GridTile>();
            newTileObj.transform.parent = gridManager.transform;
            newTileObj.transform.position = HexGridUtil.AxialHexToPixel(coord, 1);
            newTile.currentGridState = AssetDatabase.LoadAssetAtPath<GS_positive>("Assets/Scripts/Grid/StateMashine/GS_Positive.asset");
            newTile.AxialCoordinate = coord;
            newTile.Setup(coord, gridTileSOs[Random.Range(0, gridTileSOs.Count)],false);
            Grid.Add(coord, newTile);

        }

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
}
