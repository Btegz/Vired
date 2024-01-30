using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Audio;

public class UpgradeHex : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public Effect effect;

    [SerializeField] UpgradeHexGrid upgradeHexGrid;

    [SerializeField] UpgradeGridHex newHex;

    [SerializeField] UpgradeGridHexPreview gridHexPrefab;

    UpgradeGridHexPreview gridHex;

    [SerializeField] TMP_Text CostText;

    [SerializeField] GameObject particle;
    public GameObject AbilityUpgrade;

    public AudioMixerGroup soundEffect;
    public AudioData NoMonetos;
    public AudioData positiveUpgrade;
    public AudioData negativeUpgrade;


    public int Cost;

    Image image;

    private void OnEnable()
    {
        // hier particles abspielen 
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
        // hier könntest du auch den On MouseEnter Particle Effect stoppen
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
            if(PlayerManager.Instance.SkillPoints > Cost)
            switch(effect)
            {
                case Effect.Positive: AudioManager.Instance.PlaySoundAtLocation(positiveUpgrade, soundEffect, null); break; 
                case Effect.Negative100: AudioManager.Instance.PlaySoundAtLocation(negativeUpgrade, soundEffect, null); break; 

            }
            gridHex.Place();
            particle.SetActive(false);
            gridHex = null;

            //if (TutorialManager.Instance != null)
            //{
            //    TutorialManager.Instance.ConfirmBlock.SetActive(false);
            //}


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
            gridHex = null;
            image.rectTransform.DOComplete();
            image.rectTransform.DOPunchRotation(Vector3.back * 30, .25f).SetEase(Ease.OutExpo);
            if(AbilityUpgrade.gameObject.activeSelf)
            AudioManager.Instance.PlaySoundAtLocation(NoMonetos, soundEffect, null);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Hier on Mouse Over Particle Effekt abspielen
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // hier Das particle Effect stoppen
        // es kann sein, dass der Particle Effect nicht vernünftig gestoppt wird
    }
}
