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
        // find GridPrefab
        // Instantiate Grid prefab and execute method Generate Grid with a rectangular shape and everything with randmo ressources and in GS_neutral
        

        GameObject gridManagerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/GridManager.prefab");
        GridScriptableObject gridSO = ScriptableObject.CreateInstance<GridScriptableObject>();
        gridSO.name = "GeneratedGridSO";





        GameObject GridTilePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/GridTile.prefab");

        GameObject gridManager = PrefabUtility.InstantiatePrefab(gridManagerPrefab) as GameObject;

        

        Dictionary<Vector2Int, GridTile> Grid = new Dictionary<Vector2Int, GridTile>();
        List<Vector2Int> coords = HexGridUtil.GenerateRectangleShapedGrid(15, 15);

        foreach (Vector2Int coord in coords)
        {
            GameObject newTileObj = PrefabUtility.InstantiatePrefab(GridTilePrefab) as GameObject;
            GridTile newTile= newTileObj.GetComponent<GridTile>();
            newTileObj.transform.parent = gridManager.transform;
            newTileObj.transform.position = HexGridUtil.AxialHexToPixel(coord, 1);

            newTile.AxialCoordinate = coord;
            newTile.Setup(coord, (Ressource)Random.Range(0, 4));
            Grid.Add(coord, newTile);
            
        }
        gridSO.Grid = Grid;
        gridManager.GetComponent<GridManager>().gridSO = gridSO;

        AssetDatabase.CreateAsset(gridSO, "Assets/Scripts/" + gridSO.name + ".asset");


        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
}
