using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilitySkillTree : MonoBehaviour
{
    [SerializeField] List<Ability> AllAbilities;
    [SerializeField] List<SkillTreeAbilityVisual> SkillTreeAbilityVisualsList;
    [SerializeField] SkillTreeAbilityVisual SkillTreeAbilityVisualPrefab;
    [SerializeField] TMP_Text SkillPoints;



    private void Awake()
    {
        AllAbilities = PlayerManager.Instance.AllAbilities;
        SkillTreeAbilityVisualsList = new List<SkillTreeAbilityVisual>();
        GetComponentsInChildren<SkillTreeAbilityVisual>(SkillTreeAbilityVisualsList) ;
    }

    private void PossibleAbilities()
    {
        List<Ability> ChosenAbilities = PlayerManager.Instance.abilitInventory;
        /* foreach( Player player in PlayerManager.Instance.Players)
         {
             ChosenAbilities.AddRange(player.AbilityInventory);
         }*/
        Debug.Log(ChosenAbilities.Count);
        foreach( SkillTreeAbilityVisual ability in SkillTreeAbilityVisualsList)
        {Debug.Log(ability.ability);
            if(ChosenAbilities.Contains(ability.ability))
            {
                
                foreach( Ability nextLevelAbility in ability.ability.NextLevel)
                {
                    if(nextLevelAbility.UpgradeCost <= PlayerManager.Instance.SkillPoints)
                    {
                        ability.AbilityVisual.color = Color.green;
                    }
                }
            }
        }
    }

    private void OnEnable()
    {
        
        SkillPoints.text = PlayerManager.Instance.SkillPoints.ToString();
        PossibleAbilities();
    }

    public void Pay()
    {
        SkillPoints.text = PlayerManager.Instance.SkillPoints.ToString();
    }
}
