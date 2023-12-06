using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class AbilityLoadout : MonoBehaviour
{
    [SerializeField] List<Ability> abilityCollection;

    [SerializeField] public List<Ability> ChosenAbilityList;

    [SerializeField] AbilityLoadoutButton abloadoutButton;

    [SerializeField] public GridLayoutGroup ChosenAbilitiesLayout;

    [SerializeField] public int amountToChoose;

    [SerializeField] GridLayoutGroup BlueAbilityLayout;
    [SerializeField] GridLayoutGroup OrangeAbilityLayout;
    [SerializeField] GridLayoutGroup RedAbilityLayout;
    [SerializeField] GridLayoutGroup GreenAbilityLayout;

    [SerializeField] Button ConfirmButton;

    [SerializeField] public List<UI_PlayerABLInventory> PlayerInventoryAreas;
    [SerializeField] UI_PlayerABLInventory playerABLInventoryPrefab;
    [SerializeField] GameObject playersArea;

    [SerializeField] Camera MinimapCam;
    [SerializeField] Button MiniMapZoomButton;
    [SerializeField] Image MinMapImage;
    bool miniMapIsZoomed;



    // Start is called before the first frame update
    void Start()
    {
        MinimapCam.orthographicSize = GridManager.Instance.mapSettings.NoiseDataSize.x;

        PlayerManager playerManager = PlayerManager.Instance;

        playerManager.AbilityLoadoutActive = true;

        ChosenAbilityList = new List<Ability>();

        foreach (Player player in playerManager.Players)
        {
            UI_PlayerABLInventory ini = Instantiate(playerABLInventoryPrefab, playersArea.transform);

            ini.Setup(player);

            foreach (Ability ability in player.AbilityInventory)
            {
                ChosenAbilityList.Add(ability);
                abilityCollection.Remove(ability);
                AbilityLoadoutButton button = Instantiate(abloadoutButton);
                button.transform.SetParent(ini.InventoryArea.transform);
                button.Setup(ability, ini.InventoryArea);
            }
        }

        AbilityLoadoutButton instance;
        foreach (Ability ability in abilityCollection)
        {
            ability.StarterAbility();
            switch (ability.MyCostRessource)
            {
                case Ressource.ressourceA:
                    instance = Instantiate(abloadoutButton, BlueAbilityLayout.transform);
                    instance.Setup(ability, BlueAbilityLayout);
                    break;
                case Ressource.ressourceB:
                    instance = Instantiate(abloadoutButton, OrangeAbilityLayout.transform);
                    instance.Setup(ability, OrangeAbilityLayout);
                    break;
                case Ressource.ressourceC:
                    instance = Instantiate(abloadoutButton, RedAbilityLayout.transform);
                    instance.Setup(ability, RedAbilityLayout);
                    break;
                case Ressource.resscoureD:
                    instance = Instantiate(abloadoutButton, GreenAbilityLayout.transform);
                    instance.Setup(ability, GreenAbilityLayout);
                    break;
                default: instance = null; break;
            }
            instance.currentState = ButtonState.inLoadout;

            instance.ability.StarterAbility();
        }
        EventManager.OnAbilityChosenEvent += AddAbilityChoice;
        EventManager.LoadOutAbilityChoiseRemoveEvent += RemoveAbilityChoice;
    }

    private void OnDestroy()
    {
        EventManager.OnAbilityChosenEvent -= AddAbilityChoice;
        EventManager.LoadOutAbilityChoiseRemoveEvent -= RemoveAbilityChoice;
    }

    public void AddAbilityChoice(AbilityLoadoutButton abilityLoadoutButton)
    {
        if (!ChosenAbilityList.Contains(abilityLoadoutButton.ability))
        {
            //abilityLoadoutButton.transform.SetParent(ChosenAbilitiesLayout.transform);
            abilityLoadoutButton.GetComponent<Button>().enabled = false;
            ChosenAbilityList.Add(abilityLoadoutButton.ability);
        }
        if (ChosenAbilityList.Count == amountToChoose)
        {
            ConfirmButton.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("I SHOULD BE DISABLED");
            ConfirmButton.gameObject.SetActive(false);
        }
    }

    public void RemoveAbilityChoice(AbilityLoadoutButton abilityLoadoutButton)
    {
        ChosenAbilityList.Remove(abilityLoadoutButton.ability);
        if (ChosenAbilityList.Count == amountToChoose)
        {
            ConfirmButton.gameObject.SetActive(true);
        }
        else
        {
            ConfirmButton.gameObject.SetActive(false);
        }
    }

    public void AbilityLoadoutConfirmed()
    {
        PlayerManager.Instance.AbilityLoadoutActive = false;
        PlayerManager.Instance.abilitInventory.AddRange(ChosenAbilityList);
        EventManager.OnConfirmButton();
        Destroy(gameObject);
    }

    public void ToggleMiniMapZoom()
    {
        if (miniMapIsZoomed)
        {
            MinMapImage.rectTransform.DOAnchorPos(new Vector2(646f, -237.83f), .5f);
            //MinMapImage.rectTransform.DOMove(new Vector3(646f, -237.83f, 0f), .5f);
            //MinMapImage.transform.DOMove(new Vector3(646f, -237.83f,0f), .5f);
            MinMapImage.rectTransform.DOScale(Vector3.one, .5f).OnComplete(() => miniMapIsZoomed = false);
        }
        else
        {
            MinMapImage.rectTransform.DOAnchorPos(Vector3.zero, .5f);
            MinMapImage.rectTransform.DOScale(Vector3.one * 2, .5f).OnComplete(() => miniMapIsZoomed = true);
        }
    }
}
