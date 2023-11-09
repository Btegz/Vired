using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SkillTreeAbilityVisual : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public Image AbilityVisual;
    [SerializeField] TMP_Text UseCost;
    [SerializeField] TMP_Text UpgradeCost;
    [SerializeField] public Ability ability;

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void SetUp(Ability ability)
    {
        this.ability = ability;
        AbilityVisual.sprite = ability.AbilityUISprite;
        UseCost.text = ability.costs.Count.ToString();
    }

    private void Start()
    {
        AbilityVisual.sprite = ability.AbilityUISprite;
        UseCost.text = ability.costs.Count.ToString();
    }
}
