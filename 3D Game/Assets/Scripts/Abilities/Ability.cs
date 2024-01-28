using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum RotationMode
{
    PlayerCenter,
    SelectedPointCenter,
}

[CreateAssetMenu]
public class Ability : HexShape
{
    public List<Effect> Effects;

    public string Name;
    //public Mesh previewShape;
    public RotationMode rotato;

    [SerializeField] private Ressource myCostRessource;

    public Ressource MyCostRessource
    {
        get { return myCostRessource; }
        set { myCostRessource = value; }
    }

    [SerializeField] private int myCostAmount;

    public int MyCostAmount
    {
        get { return myCostAmount; }
        set { myCostAmount = value; }
    }

    [SerializeField] private int myStartCostAmount;
    public int MyStartCostAmount
    {
        get { return myStartCostAmount; }
        set { myStartCostAmount = value; }
    }

    [SerializeField] private int myCostAmountIncreaseEveryXUpgrades;

    public int MyCostAmountIncreaseEveryXUpgrades
    {
        get { return myCostAmountIncreaseEveryXUpgrades; }
        set { myCostAmountIncreaseEveryXUpgrades = value; }
    }


    //[SerializeField] public List<Ressource> costs;


    [SerializeField] public Sprite AbilityUISprite;


    [SerializeField] private int myTierLevel;

    public int MyTierLevel
    {
        get { return myTierLevel; }
        set { myTierLevel = value; }
    }
    [SerializeField] private int myMaxTierLevel;

    public int MyMaxTierLevel
    {
        get { return myMaxTierLevel; }
        set { myMaxTierLevel = value; }
    }

    public void RecalculatePreviewMesh(Dictionary<Vector2Int, Effect> newShape)
    {
        Coordinates = new List<Vector2Int>();
        Effects = new List<Effect>();
        foreach (KeyValuePair<Vector2Int, Effect> kvp in newShape)
        {
            Coordinates.Add(kvp.Key);
            Effects.Add(kvp.Value);
        }

        //List<Mesh> hexagone = new List<Mesh>();

        //List<GridTile> GridTileList = new List<GridTile>();


        //foreach (Vector2Int coord in Coordinates)
        //{
        //    GridTile gridTileInstanz = Instantiate(tileprefab);

        //    gridTileInstanz.transform.position = HexGridUtil.AxialHexToPixel(coord, 1);

        //    GridTileList.Add(gridTileInstanz);

        //    hexagone.Add(gridTileInstanz.DrawMesh());
        //}

        //CombineInstance[] combine = new CombineInstance[hexagone.Count];
        //int i = 0;
        //while (i < hexagone.Count)
        //{
        //    combine[i].mesh = hexagone[i];
        //    combine[i].transform = GridTileList[i].transform.localToWorldMatrix;
        //    i++;
        //}
        //Mesh mesh = new Mesh();
        //mesh.CombineMeshes(combine, false);
        //previewShape = mesh;
        //foreach (GridTile gr in GridTileList)
        //{
        //    GameObject.DestroyImmediate(gr.gameObject);
        //}
        RecalculateCost();
    }

    [SerializeField] private int myPositiveUpgradeCost;

    public int MyPositiveUpgradeCost
    {
        get { return myPositiveUpgradeCost; }
        set { myPositiveUpgradeCost = value; }
    }

    [SerializeField] private int myDamageUpgradeCost;

    public int MyDamageUpgradeCost
    {
        get { return myDamageUpgradeCost; }
        set { myDamageUpgradeCost = value; }
    }

    public int MyRangeUpgradeCost
    {
        get { return myTierLevel; }
    }

    [Header("StarterParameters")]

    [SerializeField] private int myStartAmountPositive;

    public int MyStartAmountPositive
    {
        get { return myStartAmountPositive; }
        set { myStartAmountPositive = value; }
    }

    [SerializeField] private int myStartAmountDamage;

    public int MyStartAmountDamage
    {
        get { return myStartAmountDamage; }
        set { myStartAmountDamage = value; }
    }

    [SerializeField] private int myStartTierlevel;

    public int MyStartTierLevel
    {
        get { return myStartTierlevel; }
        set { myStartTierlevel = value; }
    }

    public void StarterAbility()
    {
        Coordinates.Clear();
        Effects.Clear();
        MyTierLevel = MyStartTierLevel;
        List<Vector2Int> possibleTiles = HexGridUtil.CubeToAxialCoord(HexGridUtil.CoordinatesReachable(Vector3Int.zero, MyStartTierLevel + 1));

        Dictionary<Vector2Int, Effect> newShape = new Dictionary<Vector2Int, Effect>();
        if (MyStartAmountPositive > 0)
        {
            Vector2Int coord = new Vector2Int(1, 0);
            newShape.Add(coord, Effect.Positive);
        }

        for (int i = 1; i < MyStartAmountPositive; i++)
        {
            Vector2Int randomTile = possibleTiles[Random.Range(0, possibleTiles.Count - 1)];
            while (newShape.ContainsKey(randomTile))
            {
                randomTile = possibleTiles[Random.Range(0, possibleTiles.Count)];
            }
            newShape.Add(randomTile, Effect.Positive);
        }

        for (int i = 0; i < MyStartAmountDamage; i++)
        {
            Vector2Int randomTile = possibleTiles[Random.Range(0, possibleTiles.Count - 1)];
            while (newShape.ContainsKey(randomTile))
            {
                randomTile = possibleTiles[Random.Range(0, possibleTiles.Count)];
            }
            newShape.Add(randomTile, Effect.Negative100);
        }
        RecalculatePreviewMesh(newShape);
    }

    public void RecalculateCost()
    {
        int newCost = 0;
        foreach (Effect tile in Effects)
        {
            switch (tile)
            {
                case Effect.Positive:
                    newCost++;
                    break;
                case Effect.Negative100:
                    newCost++;
                    break;
                case Effect.Negative200:
                    newCost += 2;
                    break;
                case Effect.Negative300:
                    newCost += 3; 
                    break;
                case Effect.Negative400:
                    newCost += 4;
                    break;
                case Effect.Negative500:
                    newCost += 5;
                    break;
            }
        }
        newCost -= MyStartAmountDamage + MyStartAmountPositive;
        newCost /= myCostAmountIncreaseEveryXUpgrades;

        MyCostAmount = newCost+myStartCostAmount;
    }
}
