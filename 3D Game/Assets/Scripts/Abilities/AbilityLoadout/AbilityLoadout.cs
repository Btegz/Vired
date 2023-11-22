using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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



    // Start is called before the first frame update
    void Start()
    {
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
        if (ChosenAbilityList.Count < amountToChoose)
        {
            //abilityLoadoutButton.transform.SetParent(ChosenAbilitiesLayout.transform);
            abilityLoadoutButton.GetComponent<Button>().enabled = false;
            ChosenAbilityList.Add(abilityLoadoutButton.ability);
            if (ChosenAbilityList.Count == amountToChoose)
            {
                ConfirmButton.gameObject.SetActive(true);
            }
            else
            {
                ConfirmButton.gameObject.SetActive(false);
            }
        }
    }

    public void RemoveAbilityChoice(AbilityLoadoutButton abilityLoadoutButton)
    {
        ChosenAbilityList.Remove(abilityLoadoutButton.ability);
        if (ConfirmButton.gameObject.activeSelf)
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
}
