using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UpgradeHexGrid : MonoBehaviour
{
    [SerializeField] Sprite EmptyHexSprite;
    [SerializeField] Sprite PlayerHexSprite;
    [SerializeField] Sprite DamageHexSprite;
    [SerializeField] Sprite PositiveHexSprite;

    public Dictionary<Vector2Int, UpgradeGridHex> Grid;
    public Dictionary<Vector2Int, Effect> AbilityGrid;
    [SerializeField] UpgradeGridHex upgradeHexPrefab;

    [SerializeField] Ability loadedAbility;

    // Start is called before the first frame update
    void Start()
    {
        MakeGrid();


        //MakeAbilityToGrid(loadedAbility);
    }

    public void MakeGrid()
    {
        Grid = new Dictionary<Vector2Int, UpgradeGridHex>();
        List<Vector2Int> gridCoords = HexGridUtil.GenerateHexagonalShapedGrid(4);

        foreach (Vector2Int coordinate in gridCoords)
        {
            UpgradeGridHex newHex = Instantiate(upgradeHexPrefab, this.transform);
            Vector3 wordPos = HexGridUtil.AxialHexToPixel(coordinate, 50);
            newHex.transform.localPosition = new Vector2(wordPos.x, wordPos.z);
            newHex.coordinate = coordinate;
            Grid.Add(coordinate, newHex);
        }

        Grid[Vector2Int.zero].Fill(PlayerHexSprite, "Player");
    }

    public void MakeAbilityToGrid(Ability ability)
    {
        AbilityGrid = new Dictionary<Vector2Int, Effect>();
        for (int i = 0; i < ability.Coordinates.Count; i++)
        {
            Sprite sp = GetFittingSprite(ability.Effects[i], out string text);
            if (Grid.ContainsKey(ability.Coordinates[i]))
            {
                AbilityGrid.Add(ability.Coordinates[i], ability.Effects[i]);
                Grid[ability.Coordinates[i]].Fill(sp, text);
                Grid[ability.Coordinates[i]].effect = ability.Effects[i];
            }
        }
    }

    public Sprite GetFittingSprite(Effect effect, out string text)
    {
        switch (effect)
        {
            case Effect.Positive:
                text = "";
                return PositiveHexSprite;
            case Effect.Neutral:
                text = "";
                return EmptyHexSprite;
            case Effect.Movement:
                text = "";
                return EmptyHexSprite;
            case Effect.Negative100:
                text = "100";
                return DamageHexSprite;
            case Effect.Negative200:
                text = "200";
                return DamageHexSprite;
            case Effect.Negative300:
                text = "300";
                return DamageHexSprite;
            case Effect.Negative400:
                text = "400";
                return DamageHexSprite;
            case Effect.Negative500:
                text = "500";
                return DamageHexSprite;
            default:
                text = "";
                return EmptyHexSprite;
        }
    }

    public void UpgradeAbility(Vector2Int coord, Effect effect)
    {
        Effect newEffect = effect;
        if (AbilityGrid.ContainsKey(coord) && newEffect == Effect.Negative100)
        {
            switch (AbilityGrid[coord])
            {
                case Effect.Negative100: newEffect = Effect.Negative200; break;
                case Effect.Negative200: newEffect = Effect.Negative300; break;
                case Effect.Negative300: newEffect = Effect.Negative400; break;
                case Effect.Negative400: newEffect = Effect.Negative500; break;
            }
            AbilityGrid[coord] = newEffect;
        }
        else
        {
            AbilityGrid.Add(coord, newEffect);
        }
        Sprite newFill = GetFittingSprite(newEffect, out string text);

        Grid[coord].Fill(newFill, text);
        Grid[coord].effect = newEffect;
    }

    public void LoadAbility(Ability ability)
    {
        if (loadedAbility != null)
        {
            ConfirmAbilityUpgrade();
        }
        MakeAbilityToGrid(ability);
        loadedAbility = ability;
    }

    public void ConfirmAbilityUpgrade()
    {
        if (loadedAbility != null)
        {
            foreach (KeyValuePair<Vector2Int, Effect> upgradedAbilityGrid in AbilityGrid)
            {
                if (!loadedAbility.Coordinates.Contains(upgradedAbilityGrid.Key))
                {
                    loadedAbility.Coordinates.Add(upgradedAbilityGrid.Key);
                    loadedAbility.Effects.Add(upgradedAbilityGrid.Value);
                }
                else
                {
                    int index = loadedAbility.Coordinates.IndexOf(upgradedAbilityGrid.Key);
                    if(upgradedAbilityGrid.Value != loadedAbility.Effects[index])
                    {
                        loadedAbility.Effects[index] = upgradedAbilityGrid.Value;
                    }
                }
            }
        }
    }
    
    public void CancelAbilityUpgrade()
    {
        if(loadedAbility != null)
        {
            MakeAbilityToGrid(loadedAbility);
        }
    }
}
