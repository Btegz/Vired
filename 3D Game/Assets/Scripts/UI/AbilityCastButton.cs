using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCastButton : MonoBehaviour
{
    [SerializeField] Sprite EmptyHexSprite;
    [SerializeField] Sprite PlayerHexSprite;
    [SerializeField] Sprite DamageHexSprite;
    [SerializeField] Sprite PositiveHexSprite;
    [SerializeField] UpgradeGridHex upgradeHexPrefab;

    [SerializeField] Sprite A_Background;
    [SerializeField] Sprite B_Background;
    [SerializeField] Sprite C_Background;
    [SerializeField] Sprite D_Background;

    [SerializeField] int index;

    [SerializeField] public Ability ability;

    public Dictionary<Vector2Int, UpgradeGridHex> UIGrid;
    [SerializeField] Sprite emptyAbilitySlotSprite;


    // Start is called before the first frame update
    void Start()
    {
        EventManager.OnSelectPlayerEvent += AssignAbility;
        GetComponent<Button>().onClick.AddListener(clicked);
        EventManager.OnConfirmButtonEvent += AssignAbility;
        EventManager.AbilityChangeEvent += UpdateUI;
    }

    public void clicked()
    {
        EventManager.OnAbilityButtonClicked(ability);
    }

    private void OnDestroy()
    {
        GetComponent<Button>().onClick.RemoveListener(clicked);
        EventManager.OnSelectPlayerEvent -= AssignAbility;
        EventManager.OnConfirmButtonEvent -= AssignAbility;
        EventManager.AbilityChangeEvent -= UpdateUI;


    }

    public void AssignAbility(Player player)
    {
        Debug.Log(player.name.ToString());
        if(player.AbilityInventory.Count > index)
        {
            ability = player.AbilityInventory[index];
            MakeAbilityToGrid();
            CorrectBackground();
        }
        else
        {
            Debug.Log("I GOT CAUGHT SHOULD I HAVE BEEN CAUGHT?");
            ability = null;
            UpgradeGridHex[] children = GetComponentsInChildren<UpgradeGridHex>();
            foreach (UpgradeGridHex rt in children)
            {
                Destroy(rt.gameObject);
            }
            GetComponent<Image>().sprite = emptyAbilitySlotSprite;
        }
    }

    public void AssignAbility()
    {
        try
        {
            ability = PlayerManager.Instance.selectedPlayer.AbilityInventory[index];
            MakeAbilityToGrid();
            CorrectBackground();
        }
        catch
        {
            ability = null;
            UpgradeGridHex[] children = GetComponentsInChildren<UpgradeGridHex>();
            foreach (UpgradeGridHex rt in children)
            {
                Destroy(rt.gameObject);
            }
        }
    }

    public void CorrectBackground()
    {

        switch (ability.MyCostRessource)
        {
            case Ressource.ressourceA:
                Debug.Log("I Wanna make correct background yo");
                GetComponent<Image>().sprite = A_Background;
                break;
            case Ressource.ressourceB:
                Debug.Log("I Wanna make correct background yo");
                GetComponent<Image>().sprite = B_Background;
                break;
            case Ressource.ressourceC:
                Debug.Log("I Wanna make correct background yo");
                GetComponent<Image>().sprite = C_Background;
                break;
            case Ressource.resscoureD:
                Debug.Log("I Wanna make correct background yo");
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
                if(kvp.Value != null)
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
}
