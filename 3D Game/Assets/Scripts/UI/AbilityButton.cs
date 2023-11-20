using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    [SerializeField] protected Sprite EmptyHexSprite;
    [SerializeField] protected Sprite PlayerHexSprite;
    [SerializeField] protected Sprite DamageHexSprite;
    [SerializeField] protected Sprite PositiveHexSprite;
    [SerializeField] protected UpgradeGridHex upgradeHexPrefab;

    [SerializeField] protected Sprite A_Background;
    [SerializeField] protected Sprite B_Background;
    [SerializeField] protected Sprite C_Background;
    [SerializeField] protected Sprite D_Background;

    [SerializeField] protected int index;

    [SerializeField] public Ability ability;
    [SerializeField] protected Sprite emptyAbilitySlotSprite;

    public Dictionary<Vector2Int, UpgradeGridHex> UIGrid;


    public void CorrectBackground()
    {

        switch (ability.MyCostRessource)
        {
            case Ressource.ressourceA:
                GetComponent<Image>().sprite = A_Background;
                break;
            case Ressource.ressourceB:
                GetComponent<Image>().sprite = B_Background;
                break;
            case Ressource.ressourceC:
                GetComponent<Image>().sprite = C_Background;
                break;
            case Ressource.resscoureD:
                GetComponent<Image>().sprite = D_Background;
                break;
        }
    }

    public void UpdateUI(Dictionary<Vector2Int, UpgradeGridHex> Grid, Ability ability)
    {
        if (ability == this.ability)
        {
            UIGrid = Grid;
            foreach (KeyValuePair<Vector2Int, UpgradeGridHex> kvp in UIGrid)
            {
                UpgradeGridHex newHex = Instantiate(kvp.Value);
                newHex.transform.SetParent(this.transform, false);
                newHex.transform.localScale = Vector2.one * 0.2f;
                Vector3 wordPos = HexGridUtil.AxialHexToPixel(kvp.Key, 10);

                newHex.transform.localPosition = new Vector2(wordPos.x, wordPos.z);
            }
        }
    }

    public void MakeGrid(int radius)
    {
        if (UIGrid != null)
        {
            foreach (KeyValuePair<Vector2Int, UpgradeGridHex> kvp in UIGrid)
            {
                if (kvp.Value != null)
                {
                    Destroy(kvp.Value.gameObject);
                }
            }
            UIGrid.Clear();
        }
        else
        {
            UIGrid = new Dictionary<Vector2Int, UpgradeGridHex>();
        }
        List<Vector2Int> gridCoords = HexGridUtil.GenerateHexagonalShapedGrid(radius);

        foreach (Vector2Int coordinate in gridCoords)
        {
            UpgradeGridHex newHex = Instantiate(upgradeHexPrefab, this.transform);
            newHex.transform.localScale = Vector2.one * 0.2f;
            Vector3 wordPos = HexGridUtil.AxialHexToPixel(coordinate, 10);
            newHex.transform.localPosition = new Vector2(wordPos.x, wordPos.z);
            newHex.coordinate = coordinate;
            UIGrid.Add(coordinate, newHex);
        }

        UIGrid[Vector2Int.zero].Fill(PlayerHexSprite, "Player");
    }

    public void MakeAbilityToGrid()
    {
        ResetButton();
        MakeGrid(ability.MyTierLevel);
        for (int i = 0; i < ability.Coordinates.Count; i++)
        {
            Sprite sp = GetFittingSprite(ability.Effects[i], out string text);
            if (UIGrid.ContainsKey(ability.Coordinates[i]))
            {
                UIGrid[ability.Coordinates[i]].Fill(sp, text);
                UIGrid[ability.Coordinates[i]].effect = ability.Effects[i];
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

    public void ResetButton()
    {
        UpgradeGridHex[] children = GetComponentsInChildren<UpgradeGridHex>();
        foreach (UpgradeGridHex rt in children)
        {
            Destroy(rt.gameObject);
        }
        GetComponent<Image>().sprite = emptyAbilitySlotSprite;
    }
}
