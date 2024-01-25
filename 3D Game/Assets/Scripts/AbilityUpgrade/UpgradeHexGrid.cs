using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Audio;

public class UpgradeHexGrid : MonoBehaviour
{
    [SerializeField] Sprite EmptyHexSprite;
    [SerializeField] Sprite PlayerHexSprite;
    [SerializeField] Sprite DamageHexSprite;
    [SerializeField] Sprite PositiveHexSprite;
    [SerializeField] Image NoAbilitySelected;


    public Dictionary<Vector2Int, UpgradeGridHex> Grid;
    public Dictionary<Vector2Int, Effect> AbilityGrid;
    [SerializeField] UpgradeGridHex upgradeHexPrefab;

    [SerializeField] public Ability loadedAbility;

    public int pointsSpent = 0;
    public int TierUPgrades = 0;

    public int AbilityTierLevel;

    public AudioMixerGroup soundEffect;

    public AudioData ConfirmUpgrade;
    public AudioData CancelUpgrade;



    // Start is called before the first frame update
    void OnEnable()
    {
        if (PlayerManager.Instance.selectedPlayer != null)
        {
            Player selectedPlayer = PlayerManager.Instance.selectedPlayer;
           // EventManager.OnSelectPlayerEvent += CancelAbilityUpgrade;
            EventManager.OnSelectPlayerEvent += UpdateLoadedAbilityFromPlayerSwitch;
            
            EventManager.OnAbilityButtonEvent += MakeAbilityToGrid;
            if (selectedPlayer.AbilityInventory[0] != null)
            {
                MakeAbilityToGrid(selectedPlayer.AbilityInventory[0]);
            }
            else
            {
                MakeGrid(3);
            }
        }
    }

    private void OnDisable()
    {
        EventManager.OnAbilityButtonEvent -= MakeAbilityToGrid;
        EventManager.OnSelectPlayerEvent -=  UpdateLoadedAbilityFromPlayerSwitch;
       // EventManager.OnSelectPlayerEvent -= CancelAbilityUpgrade;
    }

    public void UpdateLoadedAbilityFromPlayerSwitch(Player player)
    {
        if(player.AbilityInventory.Count>0)
        {
            CancelAbilityUpgrade();
            NoAbilitySelected.enabled = false;
            LoadAbility(player.AbilityInventory[0]);
            
        }
        else
        {
            if (Grid != null)
            {
                foreach (KeyValuePair<Vector2Int, UpgradeGridHex> kvp in Grid)
                {
                    Destroy(kvp.Value.gameObject);
                }
                Grid.Clear();
            }
            else
            {
                Grid = new Dictionary<Vector2Int, UpgradeGridHex>();
            }

            NoAbilitySelected.enabled = true;
        }
    }

    public void MakeGrid(int radius)
    {
        if(Grid != null)
        {
            foreach(KeyValuePair <Vector2Int,UpgradeGridHex> kvp in Grid)
            {
                Destroy(kvp.Value.gameObject);
            }
            Grid.Clear();
        }
        else
        {
            Grid = new Dictionary<Vector2Int, UpgradeGridHex>();
        }
        List<Vector2Int> gridCoords = HexGridUtil.GenerateHexagonalShapedGrid(radius);

        foreach (Vector2Int coordinate in gridCoords)
        {
            UpgradeGridHex newHex = Instantiate(upgradeHexPrefab, this.transform);
            Vector3 wordPos = HexGridUtil.AxialHexToPixel(coordinate, 50);
            newHex.transform.localPosition = new Vector2(wordPos.x, wordPos.z);
            newHex.coordinate = coordinate;
            Grid.Add(coordinate, newHex);
        }

        Grid[Vector2Int.zero].Fill(PlayerHexSprite, "");
    }

    public void UpdateGrid()
    {
        if(Grid == null)
        {
            return;
        }
        foreach(Vector2Int coordinate in HexGridUtil.Ring(Vector3Int.zero, loadedAbility.MyTierLevel))
        {
            UpgradeGridHex newHex = Instantiate(upgradeHexPrefab, this.transform);
            Vector3 wordPos = HexGridUtil.AxialHexToPixel(coordinate, 50);
            newHex.transform.localPosition = new Vector2(wordPos.x, wordPos.z);
            newHex.coordinate = coordinate;
            Grid.Add(coordinate, newHex);
        }
    }

    public void MakeAbilityToGrid(Ability ability,AbilityButton button)
    {
        //pointsSpent = 0;
        //TierUPgrades = 0;
        MakeAbilityToGrid(ability);
    }

    public void MakeAbilityToGrid(Ability ability)
    {


        pointsSpent = 0;
        TierUPgrades = 0;
        if (ability == null)
        {
            MakeGrid(0);
            return;
        }
        MakeGrid(ability.MyTierLevel);
        if(AbilityGrid != null)
        {
            AbilityGrid.Clear();
        }
        else
        {
            AbilityGrid = new Dictionary<Vector2Int, Effect>();
        }
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
        loadedAbility = ability;
        EventManager.OnUpgradeAbilitySelect(ability);
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
        pointsSpent++;
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
            // Hier kannst du wackeln für Schadenseffekte
            // Particle Effekte kannste hier auch machen
        }
        else
        {
            AbilityGrid.Add(coord, newEffect);

            //Hier kannst du wackeln für PositivEffekte
            //Particle Effekte kannste hier auch machen
        }
        Sprite newFill = GetFittingSprite(newEffect, out string text);

        Grid[coord].Fill(newFill, text);
        Grid[coord].effect = newEffect;
    }

    public void LoadAbility(Ability ability)
    {
        //if (loadedAbility != null)
        //{
        //    ConfirmAbilityUpgrade();
        //}
        if(loadedAbility == ability)
        {
            return;
        }
        loadedAbility = ability;
        MakeAbilityToGrid(loadedAbility);
    }

    public void ConfirmAbilityUpgrade()
    {
        if (loadedAbility != null)
        {
            foreach (KeyValuePair<Vector2Int, Effect> upgradedAbilityGrid in AbilityGrid)
            {
                if(upgradedAbilityGrid.Value == Effect.Neutral)
                {
                    continue;
                }
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
            loadedAbility.RecalculatePreviewMesh(AbilityGrid);
            EventManager.OnAbilityChange(Grid,loadedAbility);
            AudioManager.Instance.PlaySoundAtLocation(ConfirmUpgrade, soundEffect, null);

        }
        //loadedAbility = null;
    }
    
    public void CancelAbilityUpgrade()
    {
        if(loadedAbility != null)
        {
            PlayerManager.Instance.SkillPoints += pointsSpent;
            loadedAbility.MyTierLevel -= TierUPgrades;
            pointsSpent = 0;
            TierUPgrades = 0;
            MakeAbilityToGrid(loadedAbility);
            AudioManager.Instance.PlaySoundAtLocation(CancelUpgrade, soundEffect, null);


        }
    }

    public void CancelAbilityUpgrade(Player player)
    {
        Debug.Log(loadedAbility.name);

        if (loadedAbility != null)
        {
            PlayerManager.Instance.SkillPoints += pointsSpent;
            loadedAbility.MyTierLevel -= TierUPgrades;
            pointsSpent = 0;
            TierUPgrades = 0;
            MakeAbilityToGrid(loadedAbility);

        }
    }
}
