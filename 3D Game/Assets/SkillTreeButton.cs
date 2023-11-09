using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillTreeButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] AbilitySkillTree abilitySkillTree;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (abilitySkillTree.gameObject.activeSelf)
        {
            abilitySkillTree.gameObject.SetActive(false);
        }

        else
        {
            abilitySkillTree.gameObject.SetActive(true);
        }
    }
}
