using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;

public class UpgradeHex : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler
{
    [SerializeField] public Effect effect;

    [SerializeField] UpgradeHexGrid upgradeHexGrid;

    [SerializeField] UpgradeGridHex newHex;

    [SerializeField] UpgradeGridHexPreview gridHexPrefab;

    UpgradeGridHexPreview gridHex;

    [SerializeField] TMP_Text CostText;

    [SerializeField] GameObject particle;

    public int Cost;

    Image image;

    private void OnEnable()
    {
        image = GetComponent<Image>();
        EventManager.UpgradeAbilitySelectEvent += UpdateCost;
        try
        {
            UpdateCost(PlayerManager.Instance.selectedPlayer.AbilityInventory[0]);
        }
        catch { }
    }

    private void OnDisable()
    {
        EventManager.UpgradeAbilitySelectEvent -= UpdateCost;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (PlayerManager.Instance.SkillPoints >= Cost)
        {
            gridHex = Instantiate(gridHexPrefab, this.transform);
            gridHex.Initialize(upgradeHexGrid, newHex,Cost);
            try
            {
                particle.SetActive(true);
            }
            catch
            {
                Debug.Log("I am trying to play a particle effect.");
            }
            
        }

    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        try
        {
            gridHex.Place();
            particle.SetActive(false);
        }
        catch
        {

        }

    }

    public void UpdateCost(Ability ability)
    {
        int newcost;
        switch (effect)
        {
            case Effect.Positive: newcost = ability.MyPositiveUpgradeCost; break;
            case Effect.Negative100: newcost = ability.MyDamageUpgradeCost; break;
            default: newcost = 0; break;
        }

        CostText.text = newcost.ToString();
        Cost = newcost;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (PlayerManager.Instance.SkillPoints < Cost)
        {
            image.rectTransform.DOComplete();
            image.rectTransform.DOPunchRotation(Vector3.back * 30, .25f).SetEase(Ease.OutExpo);
        }
    }
}
