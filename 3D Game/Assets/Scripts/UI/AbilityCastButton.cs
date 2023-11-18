using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCastButton : MonoBehaviour
{
    [SerializeField] int index;

    [SerializeField] public Ability ability;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.OnSelectPlayerEvent += AssignAbility;
        GetComponent<Button>().onClick.AddListener(clicked);
        EventManager.OnConfirmButtonEvent += AssignAbility;
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

    }

    public void AssignAbility(Player player)
    {
        try
        {
            ability = player.AbilityInventory[index];
        }
        catch
        {
            ability = null;
        }
    }

    public void AssignAbility()
    {
        try
        {
            ability = PlayerManager.Instance.selectedPlayer.AbilityInventory[index];
        }
        catch
        {
            ability = null;
        }
    }
}
