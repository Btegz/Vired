using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;

public class EditorAbilityMesh
{
    [MenuItem("Utilities/Generate Ability Mesh")]
    public static void GenerateDefaultMesh()
    {

        string[] abiltieNames = Directory.GetFiles("Assets/Scripts/Player/AbilitiesScriptableObjects/", "*.asset");
        List<Ability> abilityAssets = new List<Ability>();



        foreach (string abilities in abiltieNames)
        {
            abilityAssets.Add(AssetDatabase.LoadAssetAtPath<Ability>(abilities));
        }

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
            mesh.CombineMeshes(combine, false);
            //a.previewShape = mesh;
            //AssetDatabase.CreateAsset(a.previewShape, $"Assets/Meshes/{a.Name}.mesh");
            foreach (GridTile gr in GridTileList)
            {
                GameObject.DestroyImmediate(gr.gameObject);
            }
            //GameObject.DestroyImmediate(GridTileList[].gameObject);
        }

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();

    }

    [MenuItem("Utilities/Generate Tile Highlight Mesh")]

    public static void MakeGridHighlightMesh()
    {
        Mesh mesh = new Mesh();
        List<Face> faces = DrawFaces();
        mesh = CombineFaces(faces,mesh);
        AssetDatabase.CreateAsset(mesh, $"Assets/Meshes/TileHighLightMesh.mesh");
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
    private static List<Face> DrawFaces()
    {
        List<Face> faces = new List<Face>();
        for (int i = 0; i < 6; i++)
        {
            Face newFace = CreateFace(1, 1, i);
            faces.Add(newFace);
        }
        return faces;
    }
    private static Face CreateFace(float outerRad, float heightA, int point)
    {
        // 4 Points to form the Face.
        Vector3 p0 = GetPoint(outerRad, heightA, (point < 5) ? point + 1 : 0);
        Vector3 p1 = GetPoint(outerRad, heightA, point);
        Vector3 p2 = p0 - new Vector3(0, 1, 0);
        Vector3 p3 = p1 - new Vector3(0, 1, 0);

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        vertices.AddRange(new List<Vector3>() {/*0*/ p0,/*1*/ p1,/*2*/ p2,/*3*/ p3 });
        triangles.AddRange(new List<int>()
            {
                1,0,2,
                1,2,3
            });

        List< Vector2> uvs = new List<Vector2>();

        for (int i = 0; i < vertices.Count; i++)
        {
            uvs.Add(new Vector2(vertices[i].x, vertices[i].y));
        }
        //List<Vector2> uvs = new List<Vector2>() { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) };
        return new Face(vertices, triangles, uvs);
    }

    private static Vector3 GetPoint(float size, float height, int index)
    {
        // Triangles that form a regular Hexagon have every angle in 60°. So i multiply index with 60 to get every rotation for a Hexagon.
        float angle_deg = 60 * index;

        // translate degree of the angle into radius.
        float angle_rad = Mathf.PI / 180f * angle_deg;

        // each corner has a distance to the Center of "size". This Distance needs to be translated to x and z coordinate.
        // the x coordinate needs to multiply cos of the radial angle with size.
        // the z coordinate needs to multiply sin of the radial angle with size.
        return new Vector3((size * Mathf.Cos(angle_rad)), height, size * Mathf.Sin(angle_rad));
    }
    private static Mesh CombineFaces(List<Face> faces, Mesh mesh)
    {
        Mesh newMesh = mesh;
        List<Face> newfaces = faces;
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        for (int i = 0; i < newfaces.Count; i++)
        {
            vertices.AddRange(newfaces[i].vertices);
            //if (i + 1 % 2 == 0)
            //{
            uvs.AddRange(newfaces[0].uvs);
            //}


            int offset = 4 * i;
            foreach (int tris in newfaces[i].triangles)
            {
                triangles.Add(tris + offset);
            }
        }
        

        newMesh.vertices = vertices.ToArray();
        newMesh.triangles = triangles.ToArray();
        newMesh.uv = uvs.ToArray();
        newMesh.RecalculateNormals();
        return newMesh;
    }
}
