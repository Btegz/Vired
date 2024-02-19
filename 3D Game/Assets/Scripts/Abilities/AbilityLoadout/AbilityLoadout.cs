using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;
using System.Linq;

public class AbilityLoadout : MonoBehaviour
{
    [SerializeField] List<Ability> abilityCollection;

    [SerializeField] public List<Ability> ChosenAbilityList;
    [SerializeField] int previousChosenAbilityList;
    [SerializeField] int currentChosenAbilityList;

    [SerializeField] AbilityLoadoutButton abloadoutButton;
    [SerializeField] AbilityLoadoutButton NewAbilityLoadoutButton;

    [SerializeField] public GridLayoutGroup ChosenAbilitiesLayout;

    [SerializeField] public int amountToChoose;

    [SerializeField] HorizontalLayoutGroup BlueAbilityLayout;
    [SerializeField] HorizontalLayoutGroup OrangeAbilityLayout;
    [SerializeField] HorizontalLayoutGroup RedAbilityLayout;
    [SerializeField] HorizontalLayoutGroup GreenAbilityLayout;

    [SerializeField] Button ConfirmButton;

    [SerializeField] public List<UI_PlayerABLInventory> PlayerInventoryAreas;
    [SerializeField] UI_PlayerABLInventory playerABLInventoryPrefab;
    [SerializeField] GameObject playersArea;

    [SerializeField] Camera MinimapCam;
    [SerializeField] Button MiniMapZoomButton;
    [SerializeField] Image MinMapImage;
    [SerializeField] public Image Interactable;
    bool miniMapIsZoomed;

    public new List<Ability> AbilitiesA;
    public new List<Ability> AbilitiesB;
    public new List<Ability> AbilitiesC;
    public new List<Ability> AbilitiesD;

    public Ability ability_;
    public Ability NewAbility;

    private void OnEnable()
    {

        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.AbilityLoadoutActive = true;
        }

        ChosenAbilityList = new List<Ability>();

        currentChosenAbilityList = 0;

        if (ChosenAbilityList.Count != amountToChoose)
        {
            ConfirmButton.gameObject.SetActive(false);
        }
        EventManager.OnAbilityChosenEvent += AddAbilityChoice;
        EventManager.OnAbilityChosenEvent += RenewAbility;
        EventManager.LoadOutAbilityChoiseRemoveEvent += RemoveAbilityChoice;
    }

    private void OnDisable()
    {
        EventManager.OnAbilityChosenEvent -= AddAbilityChoice;
        EventManager.OnAbilityChosenEvent += RenewAbility;
        EventManager.LoadOutAbilityChoiseRemoveEvent -= RemoveAbilityChoice;
    }

    // Start is called before the first frame update
    void Start()
    {
        MinimapCam.orthographicSize = GridManager.Instance.mapSettings.NoiseDataSize.x;
        Ability();
    }
    public void Ability()
    {
        for (int i = 0; i < 4; i++)
        {
            switch (i)
            {
                case (int)Ressource.ressourceA:
                    if (BlueAbilityLayout.GetComponentsInChildren<AbilityLoadoutButton>().Length == 0)
                    {
                        NewAbility = AbilitiesA[0];
                        AbilitiesA.RemoveAt(0);
                        NewAbilityLoadoutButton = Instantiate(abloadoutButton, BlueAbilityLayout.transform);
                        NewAbilityLoadoutButton.Setup(NewAbility, BlueAbilityLayout);
                    }
                    break;
                case (int)Ressource.ressourceB:
                    if (OrangeAbilityLayout.GetComponentsInChildren<AbilityLoadoutButton>().Length == 0)
                    {
                        NewAbility = AbilitiesB[0];
                        AbilitiesB.RemoveAt(0);
                        NewAbilityLoadoutButton = Instantiate(abloadoutButton, OrangeAbilityLayout.transform);
                        NewAbilityLoadoutButton.Setup(NewAbility, OrangeAbilityLayout);
                    }
                    break;
                case (int)Ressource.ressourceC:
                    if (RedAbilityLayout.GetComponentsInChildren<AbilityLoadoutButton>().Length == 0)
                    {
                        NewAbility = AbilitiesC[0];
                        AbilitiesC.RemoveAt(0);
                        NewAbilityLoadoutButton = Instantiate(abloadoutButton, RedAbilityLayout.transform);
                        NewAbilityLoadoutButton.Setup(NewAbility, RedAbilityLayout);
                    }
                    break;
                case (int)Ressource.ressourceD:
                    if (GreenAbilityLayout.GetComponentsInChildren<AbilityLoadoutButton>().Length == 0)
                    {
                        NewAbility = AbilitiesD[0];
                        AbilitiesD.RemoveAt(0);
                        NewAbilityLoadoutButton = Instantiate(abloadoutButton, GreenAbilityLayout.transform);
                        NewAbilityLoadoutButton.Setup(NewAbility, GreenAbilityLayout);
                    }
                    break;
                default: NewAbilityLoadoutButton = null; break;
            }
            NewAbilityLoadoutButton.currentState = ButtonState.newInLoadout;
        }
    }


    public void RenewAbility(AbilityLoadoutButton abilityLoadoutButton, Player player)
    {
        if (ChosenAbilityList.Count != 0)
        {
            try
            {
                switch (abilityLoadoutButton.ability.MyCostRessource)
                {
                    case Ressource.ressourceA:
                        if (BlueAbilityLayout.GetComponentsInChildren<AbilityLoadoutButton>().Length == 0)
                        {
                            NewAbility = AbilitiesA[0];
                            AbilitiesA.RemoveAt(0);
                            NewAbilityLoadoutButton = Instantiate(abloadoutButton, BlueAbilityLayout.transform);
                            NewAbilityLoadoutButton.Setup(NewAbility, BlueAbilityLayout);
                        }
                        break;
                    case Ressource.ressourceB:
                        if (OrangeAbilityLayout.GetComponentsInChildren<AbilityLoadoutButton>().Length == 0)
                        {
                            NewAbility = AbilitiesB[0];
                            AbilitiesB.RemoveAt(0);
                            NewAbilityLoadoutButton = Instantiate(abloadoutButton, OrangeAbilityLayout.transform);
                            NewAbilityLoadoutButton.Setup(NewAbility, OrangeAbilityLayout);
                        }
                        break;
                    case Ressource.ressourceC:
                        if (RedAbilityLayout.GetComponentsInChildren<AbilityLoadoutButton>().Length == 0)
                        {
                            NewAbility = AbilitiesC[0];
                            AbilitiesC.RemoveAt(0);
                            NewAbilityLoadoutButton = Instantiate(abloadoutButton, RedAbilityLayout.transform);
                            NewAbilityLoadoutButton.Setup(NewAbility, RedAbilityLayout);
                        }
                        break;
                    case Ressource.ressourceD:
                        if (GreenAbilityLayout.GetComponentsInChildren<AbilityLoadoutButton>().Length == 0)
                        {
                            NewAbility = AbilitiesD[0];
                            AbilitiesD.RemoveAt(0);
                            NewAbilityLoadoutButton = Instantiate(abloadoutButton, GreenAbilityLayout.transform);
                            NewAbilityLoadoutButton.Setup(NewAbility, GreenAbilityLayout);
                        }
                        break;
                    default: NewAbilityLoadoutButton = null; break;
                }
                NewAbilityLoadoutButton.currentState = ButtonState.newInLoadout;
                currentChosenAbilityList++;
                return;
            }
            catch { }
        }
        else
        {
            return;
        }
    }

    public void SwitchtoMain()
    {
        CameraRotation.Instance.CameraCenterToPlayer(PlayerManager.Instance.selectedPlayer);
        CameraRotation.Instance.Worldcam.Priority = 3;
        CameraRotation.Instance.AbilityUpgradeCam.Priority = 1;
        CameraRotation.Instance.TopDownCam.Priority = 0;
        CameraRotation.Instance.AbilityLoadoutCam.Priority = 2;
        CameraRotation.Instance.CameraCenterToPlayer(PlayerManager.Instance.selectedPlayer);
        PlayerManager.Instance.selectedPlayer.GetComponentInChildren<PlayerVisuals>().PlayerSelection(PlayerManager.Instance.selectedPlayer);
        CameraRotation.Instance.MainCam = true;
    }

    public void AddAbilityChoice(AbilityLoadoutButton abilityLoadoutButton, Player player)
    {

        /* if (!ChosenAbilityList.Contains(abilityLoadoutButton.ability))
         {*/
        //abilityLoadoutButton.transform.SetParent(ChosenAbilitiesLayout.transform);
        abilityLoadoutButton.GetComponent<Button>().enabled = false;
        ChosenAbilityList.Add(abilityLoadoutButton.ability);
        // }
        if (ChosenAbilityList.Count == amountToChoose)
        {
            ConfirmButton.gameObject.SetActive(true);
        }
        else
        {
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

        foreach (UI_PlayerABLInventory aBLInventory in PlayerInventoryAreas)
        {
            foreach (AbilityLoadoutButton ablB in aBLInventory.AbilityLoadoutButtonInstances)
            {
                ablB.currentState = ButtonState.fixedInLoadout;
            }
        }

        gameObject.SetActive(false);
        Interactable.enabled = false;
    }

    /* public void ToggleMiniMapZoom()
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
     }*/
}
