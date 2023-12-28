using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityCastButton : AbilityButton, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void AssignAbility(Player player)
    {
        if (player.AbilityInventory.Count > index)
        {
            ability = player.AbilityInventory[index];
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
        RectData();
        currentState = ButtonState.inMainScene;
        EventManager.AbilityUpgradeEvent += ChangeCurrentState;
        EventManager.OnSelectPlayerEvent += AssignAbility;
        //GetComponent<Button>().onClick.AddListener(clicked);
        EventManager.OnConfirmButtonEvent += AssignAbility;
        EventManager.AbilityChangeEvent += UpdateUI;
    }

    public void clicked()
    {
        if (ability == null)
        {
            return;
        }
        if (!PlayerManager.Instance.InventoryCheck(ability, PlayerManager.Instance.selectedPlayer) && currentState == ButtonState.inMainScene)
        {
            InfoTextPopUp newthing = Instantiate(infoTextPopUp, transform.position+Vector3.up*100, Quaternion.identity, UIManager.Instance.transform);
            newthing.Text = "Not enough Ressource";
            RectTransform thisRectT = GetComponent<RectTransform>();
            thisRectT.DOComplete();
            thisRectT.DOPunchRotation(Vector3.back * 30, .25f).SetEase(Ease.OutExpo);
            return;
        }
        EventManager.OnAbilityButtonClicked(ability,this);
        
    }

    private void OnDestroy()
    {
        GetComponent<Button>().onClick.RemoveListener(clicked);
        EventManager.OnSelectPlayerEvent -= AssignAbility;
        EventManager.OnConfirmButtonEvent -= AssignAbility;
        EventManager.AbilityChangeEvent -= UpdateUI; 
        EventManager.AbilityUpgradeEvent -= ChangeCurrentState;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        clicked();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ability == null)
        {
            return;
        }
        if (currentState != ButtonState.inMainScene)
        {
            return;
        }
        GameObject currentRessourceTextHighlight = UIManager.Instance.ressourceAText.gameObject;
        switch (ability.MyCostRessource)
        {
            case Ressource.ressourceA:
                currentRessourceTextHighlight = UIManager.Instance.ressourceAText.gameObject;
                break;
            case Ressource.ressourceB:
                currentRessourceTextHighlight = UIManager.Instance.ressourceBText.gameObject;
                break;
            case Ressource.ressourceC:
                currentRessourceTextHighlight = UIManager.Instance.ressourceCText.gameObject;
                break;
            case Ressource.ressourceD:
                currentRessourceTextHighlight = UIManager.Instance.ressourceDText.gameObject;
                break;
        }
        foreach(RessourceHighlight rsh in currentRessourceTextHighlight.GetComponentsInChildren<RessourceHighlight>())
        {
            Destroy(rsh.gameObject);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ability == null)
        {
            return;
        }
        if (currentState != ButtonState.inMainScene)
        {
            return;
        }
        RessourceHighlight RessourceHighlight = Instantiate(UIManager.Instance.ressourceHighlight);
        switch (ability.MyCostRessource)
        {
            case Ressource.ressourceA:
                RessourceHighlight.transform.position = UIManager.Instance.ressourceAText.transform.position;
                RessourceHighlight.transform.SetParent(UIManager.Instance.ressourceAText.transform);
                break;
            case Ressource.ressourceB:
                RessourceHighlight.transform.position = UIManager.Instance.ressourceBText.transform.position;
                RessourceHighlight.transform.SetParent(UIManager.Instance.ressourceBText.transform);
                break;
            case Ressource.ressourceC:
                RessourceHighlight.transform.position = UIManager.Instance.ressourceCText.transform.position;
                RessourceHighlight.transform.SetParent(UIManager.Instance.ressourceCText.transform);
                break;
            case Ressource.ressourceD:
                RessourceHighlight.transform.position = UIManager.Instance.ressourceDText.transform.position;
                RessourceHighlight.transform.SetParent(UIManager.Instance.ressourceDText.transform);
                break;
        }
    }
}
