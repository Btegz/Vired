using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ButtonState { newInLoadout,fixedInLoadout, inMainScene, inUpgrade}

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
    [SerializeField] TMP_Text CostText;

    [SerializeField] public InfoTextPopUp infoTextPopUp;

    [SerializeField] public ButtonState currentState;

    public Dictionary<Vector2Int, UpgradeGridHex> UIGrid;
    public AbilityLoadout abilityLoadout;

    protected RectTransform rectTransform;
    protected float width;
    protected float height;
    protected Vector2 centerDelta;

    [SerializeField] float HexSizePerButtonSize;
    [SerializeField] float HexSizeFactorPerAbilityTealevel;


    public void Start()
    {
        abilityLoadout = GetComponentInParent<AbilityLoadout>();
    }
    public void RectData()
    {
        rectTransform = GetComponent<RectTransform>();
        width = rectTransform.rect.width;
        height = rectTransform.rect.height;
        if(rectTransform.anchorMax == Vector2.one * .5f)
        {
            centerDelta = new Vector2(0, 0);
        }
        else
        {
            centerDelta = new Vector2(width / 2f, height / 2f);
        }
        //Debug.Log($"width: {width}, height: {height}, CenterDelta: {centerDelta}");
    }

    public void ChangeCurrentState(ButtonState newState)
    {
        Debug.Log("Changing Buttonstate from " + currentState + ", to " + newState);
        currentState = newState;
    }

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
            case Ressource.ressourceD:
                GetComponent<Image>().sprite = D_Background;
                break;
        }
    }

    public void UpdateUI(Dictionary<Vector2Int, UpgradeGridHex> Grid, Ability ability)
    {
        if (ability == this.ability)
        {
            //UIGrid = Grid;
            //foreach (KeyValuePair<Vector2Int, UpgradeGridHex> kvp in UIGrid)
            //{
            //    if(kvp.Value.effect == Effect.Neutral)
            //    {
            //        continue;
            //    }
            //    UpgradeGridHex newHex = Instantiate(kvp.Value);
            //    newHex.transform.SetParent(this.transform, false);
            //    newHex.transform.localScale = Vector2.one * 0.2f;
            //    Vector3 wordPos = HexGridUtil.AxialHexToPixel(kvp.Key, 10);
            //    newHex.transform.localPosition = new Vector2(wordPos.x, wordPos.z);
            //    CostText.text = ability.MyCostAmount.ToString();
            //}
            MakeAbilityToGrid();
        }
    }

    public void MakeAbilityToGrid()
    {
        if(ability==null)
        {
            Debug.Log("no ability to make to grid");
            return;
        }
        ResetButton();


        //MakeGrid(ability.MyTierLevel);
        if (UIGrid != null)
        {
            foreach (KeyValuePair<Vector2Int, UpgradeGridHex> kvp in UIGrid)
            {
                if (kvp.Value != null)
                {
                    Debug.Log("I DESTROY");
                    Destroy(kvp.Value.gameObject);
                }
            }
            UIGrid.Clear();
        }
        else
        {
            UIGrid = new Dictionary<Vector2Int, UpgradeGridHex>();
        }
        float buttonSize = width * HexSizePerButtonSize;
        float abilityTierSize = 1f - (HexSizeFactorPerAbilityTealevel * (0.25f*(Mathf.Pow((float)ability.MyTierLevel, 2f))));
        for (int i = 0; i < ability.Coordinates.Count; i++)
        {
            //Debug.Log("I make a tile of the ability now");
           Sprite sp = GetFittingSprite(ability.Effects[i], out string text);
            if (UIGrid.ContainsKey(ability.Coordinates[i]))
            {
                UIGrid[ability.Coordinates[i]].Fill(sp, text);
                UIGrid[ability.Coordinates[i]].effect = ability.Effects[i];
            }
            else
            {
                Debug.Log($"I instantiate a Tile now, centerDelta is {centerDelta}");
                UpgradeGridHex newHex = Instantiate(upgradeHexPrefab, this.transform);

                RectTransform newHexRect = newHex.GetComponent<RectTransform>();


                newHexRect.sizeDelta = Vector2.one * buttonSize * abilityTierSize;

                //newHex.transform.localScale = Vector2.one * 0.2f;
                Vector3 wordPos = HexGridUtil.AxialHexToPixel(ability.Coordinates[i], newHexRect.rect.width/2f);
                newHex.transform.localPosition = new Vector2(wordPos.x, wordPos.z)+centerDelta;
                newHex.coordinate = ability.Coordinates[i];
                UIGrid.Add(ability.Coordinates[i], newHex); 
                UIGrid[ability.Coordinates[i]].Fill(sp, text);
                UIGrid[ability.Coordinates[i]].effect = ability.Effects[i];
            }
        }
        UpgradeGridHex playerHex = Instantiate(upgradeHexPrefab, this.transform);

        //playerHex.transform.localScale = Vector2.one * 0.2f;
        RectTransform newPlayerHexRect = playerHex.GetComponent<RectTransform>();

        //float buttonSize = rectTransform.rect.width * HexSizePerButtonSize;
        //float abilityTierSize = HexSizeFactorPerAbilityTielevel * ability.MyTierLevel;
        newPlayerHexRect.sizeDelta = Vector2.one * buttonSize * abilityTierSize;


        Vector3 playerWorldPos = HexGridUtil.AxialHexToPixel(Vector2Int.zero, newPlayerHexRect.rect.width / 2f);
        playerHex.transform.localPosition = new Vector2(playerWorldPos.x, playerWorldPos.z)+centerDelta;
        playerHex.coordinate = Vector2Int.zero;
        UIGrid.Add(Vector2Int.zero, playerHex);
        UIGrid[Vector2Int.zero].Fill(PlayerHexSprite, "Player");

        CostText.text = ability.MyCostAmount.ToString();
        CorrectBackground();
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
        Debug.Log("reset Button is beeing called. This is destroying my Ability Preview");
        UpgradeGridHex[] children = GetComponentsInChildren<UpgradeGridHex>();
        foreach (UpgradeGridHex rt in children)
        {
            Destroy(rt.gameObject);
        }
        GetComponent<Image>().sprite = emptyAbilitySlotSprite;
        //UIGrid.Clear();
        CostText.text = "";
    }
}
