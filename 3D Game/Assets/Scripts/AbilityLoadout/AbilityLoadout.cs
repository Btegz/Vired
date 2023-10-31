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

    [SerializeField] Canvas mainGameCanvas;

    // Start is called before the first frame update
    void Start()
    {        
        AbilityLoadoutButton instance;
        foreach (Ability ability in PlayerManager.Instance.abilitInventory)
        {
            instance = Instantiate(abloadoutButton, ChosenAbilitiesLayout.transform);
            instance.Setup(ability, this);
            abilityCollection.Remove(ability);
        }

        //AbilityLoadoutButton instance;
        foreach (Ability ability in abilityCollection)
        {
            switch (ability.costs[0])
            {
                case Ressource.ressourceA:
                    instance = Instantiate(abloadoutButton, BlueAbilityLayout.transform);
                    instance.Setup(ability, this);
                    break;

                case Ressource.ressourceB:
                    instance = Instantiate(abloadoutButton, OrangeAbilityLayout.transform);
                    instance.Setup(ability, this);
                    break;

                case Ressource.ressourceC:
                    instance = Instantiate(abloadoutButton, RedAbilityLayout.transform);
                    instance.Setup(ability, this);
                    break;
                case Ressource.resscoureD:
                    instance = Instantiate(abloadoutButton, GreenAbilityLayout.transform);
                    instance.Setup(ability, this);
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddAbilityChoice(AbilityLoadoutButton abilityLoadoutButton)
    {
        if (ChosenAbilityList.Count < amountToChoose)
        {
            abilityLoadoutButton.transform.parent = ChosenAbilitiesLayout.transform;
            ChosenAbilityList.Add(abilityLoadoutButton.ability);
            if (ChosenAbilityList.Count == amountToChoose)
            {
                ConfirmButton.gameObject.SetActive(true);
            }
        }
    }

    public void AbilityLoadoutConfirmed()
    {
        //Debug.Log("Selection confirmed");
        PlayerManager.Instance.abilitInventory.AddRange(ChosenAbilityList);
        EventManager.OnConfirmButton();
        Destroy(gameObject);
    }
}
