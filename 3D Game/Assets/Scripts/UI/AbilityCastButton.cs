using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityCastButton : AbilityButton, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image AbilityButtonImage;
    private Color color;
    public AbilityCastButton MainButton;
    public Image AbilityPreview;
    public AudioData AbilitySelect;
    public AudioMixerGroup soundEffect;
    [SerializeField] Image ressourceIconImage;

    List<RessourceHighlight> livingHighlights;


    public void OnEnable()
    {
        color = Color.white;
        AssignAbility();
      
    }


    public void AssignAbility(Player player)
    {
        if (player.AbilityInventory.Count > index)
        {
            ability = player.AbilityInventory[index];

            color.a = 1f;
            AbilityButtonImage.GetComponent<Image>().color = color;
            ResourceButton.GetComponent<Image>().enabled = true;
            ResourceButton.GetComponent<Image>().color = color;



            MakeAbilityToGrid(ability);
            //CorrectResource();
            AbilityButtonImage.sprite = Background;
        }
        else
        {
            ability = null;
            color.a = 0.2f;
            AbilityButtonImage.GetComponent<Image>().color = color;
            ResourceButton.GetComponent<Image>().enabled = false;
            AbilityButtonImage.sprite = Background;






            ResetButton();
        }
    }

    public void AssignAbility()
    {
        try
        {
            ability = PlayerManager.Instance.selectedPlayer.AbilityInventory[index];
            color.a = 1f;
            AbilityButtonImage.GetComponent<Image>().color = color;
            ResourceButton.GetComponent<Image>().color = color;
            AbilityButtonImage.sprite = Background;




        }
        catch
        {
            ability = null;
            color.a = 0.2f;
            AbilityButtonImage.GetComponent<Image>().color = color;
            ResourceButton.GetComponent<Image>().color = color;
            AbilityButtonImage.sprite = Background;

            UpgradeGridHex[] children = GetComponentsInChildren<UpgradeGridHex>();

            foreach (UpgradeGridHex rt in children)
            {
                Destroy(rt.gameObject);
            }
            ResetButton();
            return;
        }
        MakeAbilityToGrid(ability);
        AbilityButtonImage.sprite = Background;
        //CorrectResource();
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
        color = AbilityButtonImage.GetComponent<Image>().color;
        livingHighlights = new List<RessourceHighlight>();
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
            //InfoTextPopUp newthing = Instantiate(infoTextPopUp, transform.position + Vector3.up * 100, Quaternion.identity, UIManager.Instance.transform);
            //newthing.Text = "Not enough Ressource";
            //RectTransform thisRectT = GetComponent<RectTransform>();
            //thisRectT.DOComplete();
            //thisRectT.DOPunchRotation(Vector3.back * 30, .25f).SetEase(Ease.OutExpo);
            Debug.Log("null1");
            return;
        }

        if(this != MainButton)
        {
           
            Ability previousMainAbility = MainButton.ability;
            Ability previousMyAbility = ability;
            PlayerManager.Instance.selectedPlayer.AbilityInventory[0] = previousMyAbility;
            PlayerManager.Instance.selectedPlayer.AbilityInventory[index] = previousMainAbility;
            AssignAbility();
            MainButton.AssignAbility();
            Debug.Log("null2");
        }

        EventManager.OnAbilityButtonClicked(MainButton.ability, this);

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
       AudioManager.Instance.PlaySoundAtLocation(AbilitySelect, soundEffect, null, true);
        
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
        GameObject currentRessourceTextHighlight = UIManager.Instance.ressourceAImage.gameObject;
        //switch (ability.MyCostRessource)
        //{
        //    case Ressource.ressourceA:
        //        currentRessourceTextHighlight = UIManager.Instance.ressourceAImage.gameObject;
        //        break;
        //    case Ressource.ressourceB:
        //        currentRessourceTextHighlight = UIManager.Instance.ressourceBImage.gameObject;
        //        break;
        //    case Ressource.ressourceC:
        //        currentRessourceTextHighlight = UIManager.Instance.ressourceCImage.gameObject;
        //        break;
        //    case Ressource.ressourceD:
        //        currentRessourceTextHighlight = UIManager.Instance.ressourceDImage.gameObject;
        //        break;
        //}
        foreach (RessourceHighlight rsh in livingHighlights)
        {
            Debug.Log("Ich sollte eigentlich sachen zerstören hmmm");
            Destroy(rsh.gameObject);
        }
        livingHighlights.Clear();
        AbilityPreview.gameObject.SetActive(false);
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
                RessourceHighlight.transform.position = UIManager.Instance.ressourceAImage.transform.position;
                RessourceHighlight.transform.SetParent(UIManager.Instance.ressourceAImage.transform);
                break;
            case Ressource.ressourceB:
                RessourceHighlight.transform.position = UIManager.Instance.ressourceBImage.transform.position;
                RessourceHighlight.transform.SetParent(UIManager.Instance.ressourceBImage.transform);
                break;
            case Ressource.ressourceC:
                RessourceHighlight.transform.position = UIManager.Instance.ressourceCImage.transform.position;
                RessourceHighlight.transform.SetParent(UIManager.Instance.ressourceCImage.transform);
                break;
            case Ressource.ressourceD:
                RessourceHighlight.transform.position = UIManager.Instance.ressourceDImage.transform.position;
                RessourceHighlight.transform.SetParent(UIManager.Instance.ressourceDImage.transform);
                break;
        }
        livingHighlights.Add(RessourceHighlight);

        AbilityPreview.gameObject.SetActive(true);
        

        
    }
}
