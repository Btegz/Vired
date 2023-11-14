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
            Debug.Log("Hello i wanna setup my PlayerInventory" + player.name);
            UI_PlayerABLInventory ini = Instantiate(playerABLInventoryPrefab, playersArea.transform);

            ini.Setup(player);

            foreach (Ability ability in player.AbilityInventory)
            {
                Debug.Log("I HAVE A CHOSEN ABILITY" + player.name + " " + ability.name);
                ChosenAbilityList.Add(ability);
                abilityCollection.Remove(ability);
                AbilityLoadoutButton button = Instantiate(abloadoutButton);
                button.Setup(ability);
                button.transform.SetParent(ini.InventoryArea.transform);
                //button.GetComponent<Button>().enabled = false;
            }
        }

        //if(PlayerManager.Instance.abilitInventory.Count > 0)
        //{
        //    ChosenAbilityList = PlayerManager.Instance.abilitInventory;
        //    foreach (Ability ability in ChosenAbilityList)
        //    {
        //        abilityCollection.Remove(ability);
        //        AbilityLoadoutButton button = Instantiate(abloadoutButton, ChosenAbilitiesLayout.transform);
        //        button.Setup(ability);
        //        button.GetComponent<Button>().enabled = false;
        //    }
        //}

        AbilityLoadoutButton instance;
        foreach (Ability ability in abilityCollection)
        {
            switch (ability.costs[0])
            {
                case Ressource.ressourceA:
                    instance = Instantiate(abloadoutButton, BlueAbilityLayout.transform);
                    instance.Setup(ability);
                    break;

                case Ressource.ressourceB:
                    instance = Instantiate(abloadoutButton, OrangeAbilityLayout.transform);
                    instance.Setup(ability);
                    break;

                case Ressource.ressourceC:
                    instance = Instantiate(abloadoutButton, RedAbilityLayout.transform);
                    instance.Setup(ability);
                    break;
                case Ressource.resscoureD:
                    instance = Instantiate(abloadoutButton, GreenAbilityLayout.transform);
                    instance.Setup(ability);
                    break;
            }
            
        }
        EventManager.OnAbilityChosenEvent += AddAbilityChoice;
    }

    private void OnDestroy()
    {
        EventManager.OnAbilityChosenEvent -= AddAbilityChoice;
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
