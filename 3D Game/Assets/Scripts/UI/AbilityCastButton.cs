using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityCastButton : AbilityButton, IPointerClickHandler
{
    public void AssignAbility(Player player)
    {
        Debug.Log("Assign Ability Is called with player reference");
        if (player.AbilityInventory.Count > index)
        {
            ability = player.AbilityInventory[index];
            Debug.Log("I Am AbilityButton number " + index + ", and i will show Ability" + player.AbilityInventory[index]);
            MakeAbilityToGrid();
            CorrectBackground();
        }
        else
        {
            ResetButton();
        }
    }

    public void AssignAbility()
    {
        Debug.Log("Assign Ability Is called without player reference");
        try
        {
            ability = PlayerManager.Instance.selectedPlayer.AbilityInventory[index];
        }
        catch
        {
            ability = null;
            UpgradeGridHex[] children = GetComponentsInChildren<UpgradeGridHex>();
            foreach (UpgradeGridHex rt in children)
            {
                Destroy(rt.gameObject);
            }
            ResetButton();
            return;
        }
            MakeAbilityToGrid();
            CorrectBackground();
        //catch
        //{
        //    ability = null;
        //    UpgradeGridHex[] children = GetComponentsInChildren<UpgradeGridHex>();
        //    foreach (UpgradeGridHex rt in children)
        //    {
        //        Destroy(rt.gameObject);
        //    }
        //    ResetButton();
        //}
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.OnSelectPlayerEvent += AssignAbility;
        //GetComponent<Button>().onClick.AddListener(clicked);
        EventManager.OnConfirmButtonEvent += AssignAbility;
        EventManager.AbilityChangeEvent += UpdateUI;
    }

    public void clicked()
    {
        EventManager.OnAbilityButtonClicked(ability);
    }

    private void OnDestroy()
    {
        GetComponent<Button>().onClick.RemoveListener(clicked);
        EventManager.OnSelectPlayerEvent -= AssignAbility;
        EventManager.OnConfirmButtonEvent -= AssignAbility;
        EventManager.AbilityChangeEvent -= UpdateUI;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        clicked();
    }
}
