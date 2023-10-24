using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;

public class EditorAbilityMesh 
{
    [MenuItem("Utilities/Generate Ability Mesh")]
    
    public static void GenerateDefaultMesh()
    {
        List<Ability> abilityAssets = new List<Ability>()
        {
            AssetDatabase.LoadAssetAtPath<Ability>("Assets/Scripts/Player/AbilitiesScriptableObjects/A_Positiv.asset"),
            AssetDatabase.LoadAssetAtPath<Ability>("Assets/Scripts/Player/AbilitiesScriptableObjects/A_Mega.asset"),
            AssetDatabase.LoadAssetAtPath<Ability>("Assets/Scripts/Player/AbilitiesScriptableObjects/A_Schaden.asset"),
            
            AssetDatabase.LoadAssetAtPath<Ability>("Assets/Scripts/Player/AbilitiesScriptableObjects/B_Positiv.asset"),
            AssetDatabase.LoadAssetAtPath<Ability>("Assets/Scripts/Player/AbilitiesScriptableObjects/B_Mega.asset"),
            AssetDatabase.LoadAssetAtPath<Ability>("Assets/Scripts/Player/AbilitiesScriptableObjects/B_Schaden.asset"),
            
            AssetDatabase.LoadAssetAtPath<Ability>("Assets/Scripts/Player/AbilitiesScriptableObjects/C_Positiv.asset"),
            AssetDatabase.LoadAssetAtPath<Ability>("Assets/Scripts/Player/AbilitiesScriptableObjects/C_Mega.asset"),
            AssetDatabase.LoadAssetAtPath<Ability>("Assets/Scripts/Player/AbilitiesScriptableObjects/C_Schaden.asset"),
            
            AssetDatabase.LoadAssetAtPath<Ability>("Assets/Scripts/Player/AbilitiesScriptableObjects/D_Positiv.asset"),
            AssetDatabase.LoadAssetAtPath<Ability>("Assets/Scripts/Player/AbilitiesScriptableObjects/D_Mega.asset"),
            AssetDatabase.LoadAssetAtPath<Ability>("Assets/Scripts/Player/AbilitiesScriptableObjects/D_Schaden.asset"),
            
            AssetDatabase.LoadAssetAtPath<Ability>("Assets/Scripts/Player/AbilitiesScriptableObjects/Movement.asset"),
        
        };
        
        foreach (Ability a in abilityAssets)
        {
            List<Vector2Int> coords = new List<Vector2Int>();
                
            foreach (Vector2Int vector in a.Coordinates)
            {
                coords.Add(vector);
            }

            List<Mesh> hexagone = new List<Mesh>();

            List<GridTile> GridTileList = new List<GridTile>();

                
            foreach (Vector2Int coord in coords)
            {
                GridTile gridTileInstanz =
                    (GridTile)PrefabUtility.InstantiatePrefab(
                        AssetDatabase.LoadAssetAtPath<GridTile>("Assets/Prefabs/GridTile.prefab"));

                gridTileInstanz.transform.position = HexGridUtil.AxialHexToPixel(coord, 1);
                
                GridTileList.Add(gridTileInstanz);
                
                hexagone.Add(gridTileInstanz.DrawMesh());
                
               
            }

            CombineInstance[] combine = new CombineInstance[hexagone.Count];
            int i = 0;
            while (i < hexagone.Count)
            {
                combine[i].mesh = hexagone[i];
                combine[i].transform = GridTileList[i].transform.localToWorldMatrix;
            //       meshFilters[i].gameObject.SetActive(false);
                i++;
            }
            Mesh mesh = new Mesh();
            mesh.CombineMeshes(combine,false);
            a.previewShape = mesh;
            AssetDatabase.CreateAsset(a.previewShape, $"Assets/Meshes/{a.Name}.mesh");
            foreach (GridTile gr in GridTileList)
            {
                GameObject.DestroyImmediate(gr.gameObject);
            }
            //GameObject.DestroyImmediate(GridTileList[].gameObject);
        }
        
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
        
    }
    

}
