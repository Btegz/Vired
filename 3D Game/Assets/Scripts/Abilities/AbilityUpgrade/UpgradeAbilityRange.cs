using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.Audio;

public class UpgradeAbilityRange : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] UpgradeHexGrid upgradeHexGrid;

    [SerializeField] int UpgradeCost = 0;

    [SerializeField]TMP_Text costText;
    public AudioMixerGroup soundEffect;
    public AudioData NoMonetos;

    private void Start()
    {
        try
        {
            UpdateCost(PlayerManager.Instance.selectedPlayer.AbilityInventory[0]);
        }
        catch
        {

        }
        
        EventManager.UpgradeAbilitySelectEvent += UpdateCost;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(PlayerManager.Instance.selectedPlayer.AbilityInventory.Count>0)
        {
            if (PlayerManager.Instance.SkillPoints >= UpgradeCost && upgradeHexGrid.loadedAbility.MyTierLevel <= upgradeHexGrid.loadedAbility.MyMaxTierLevel)
            {
                    PlayerManager.Instance.SkillPoints -= UpgradeCost;
                    upgradeHexGrid.pointsSpent += UpgradeCost;
                    upgradeHexGrid.loadedAbility.MyTierLevel++;
                    upgradeHexGrid.UpdateGrid();
                    upgradeHexGrid.TierUPgrades++;
                    UpdateCost(upgradeHexGrid.loadedAbility);
            }
            else
            {
                transform.DOComplete();
                transform.DOPunchRotation(Vector3.back*10,0.25f);
                AudioManager.Instance.PlaySoundAtLocation(NoMonetos, soundEffect, null, true);

            }

        }
    }

    public void UpdateCost(Ability ability)
    {
        UpgradeCost = ability.MyRangeUpgradeCost;
        costText.text = UpgradeCost.ToString();
    }
}
