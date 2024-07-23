using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Linq;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] Button EndTurnButton;
    [SerializeField] Button AbilitiesInventoryButton;
    [SerializeField] Image negativeFillBar;

    [SerializeField] public TMP_Text ressourceAText;
    [SerializeField] public TMP_Text ressourceBText;
    [SerializeField] public TMP_Text ressourceCText;
    [SerializeField] public TMP_Text ressourceDText;
    
    [SerializeField] public Image ressourceAImage;
    [SerializeField] public Image ressourceBImage;
    [SerializeField] public Image ressourceCImage;
    [SerializeField] public Image ressourceDImage;



    [SerializeField] GameObject AbilitiesInventory;

    [SerializeField] List<AbilityCastButton> abilityButtons;

    [SerializeField] Sprite emptyAbilitySlotSprite;

    [SerializeField] GameObject PlayerButtonParent;
    [SerializeField] PlayerButton playerButtonPrefab;
    [SerializeField] public List<PlayerButton> playerButtons;

    [SerializeField] public RessourceHighlight ressourceHighlight;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            List<PlayerButton> playerButtons = GetComponentsInChildren<PlayerButton>().ToList<PlayerButton>();

            for (int i = 0; i < PlayerManager.Instance.Players.Count; i++)
            {
                playerButtons[i].Setup(PlayerManager.Instance.Players[i]);
            }
            EndTurnButton.onClick.AddListener(EventManager.OnEndTurn);
        }
        catch { }

        //EndTurnButton.onClick.AddListener(GridManager.Instance.TriggerPhase);
        //AbilitiesInventoryButton.onClick.AddListener(ExpandAbilityInventory);
    }




    /*public void EndTurn()
    {

        EventManager.OnEndTurn();
    }*/
    private void OnEnable()
    {
        try
        {
            //List<Ability> abilityInv = PlayerManager.Instance.abilitInventory;

            //for (int i = 0; i < abilityButtons.Count; i++)
            //{
            //    if (i < abilityInv.Count)
            //    {
            //        abilityButtons[i].GetComponent<Image>().sprite = abilityInv[i].AbilityUISprite;
            //    }
            //    else
            //    {
            //        abilityButtons[i].GetComponent<Button>().image.sprite = emptyAbilitySlotSprite;
            //    }
            //}
            UpdateAbilityInventory(PlayerManager.Instance.selectedPlayer);
        }
        catch
        {
        }


    }

    private void OnDestroy()
    {
    }

    // Update is called once per frame
    void Update()
    {




        List<GridTile> negativeTiles = GridManager.Instance.GetTilesWithState(GridManager.Instance.gS_Negative);
        if (negativeTiles.Count > 0)
        {
            negativeTiles.AddRange(GridManager.Instance.GetTilesWithState(GridManager.Instance.gS_Enemy));
            negativeTiles.AddRange(GridManager.Instance.GetTilesWithState(GridManager.Instance.gS_Boss));
            if (negativeTiles.Count > 0)
            {
                negativeFillBar.fillAmount = (float)negativeTiles.Count / (((float)GridManager.Instance.Grid.Count * 2) / 3);
                if (negativeFillBar.fillAmount >= 1)
                {
                    GameOver();
                }
            }

        }
        else
        {
            negativeFillBar.fillAmount = 0;
        }

        ressourceAText.text = PlayerManager.Instance.RessourceAInventory.ToString();
        ressourceBText.text = PlayerManager.Instance.RessourceBInventory.ToString();
        ressourceCText.text = PlayerManager.Instance.RessourceCInventory.ToString();
        ressourceDText.text = PlayerManager.Instance.RessourceDInventory.ToString();
    }

    //public void ExpandAbilityInventory()
    //{
    //    AbilitiesInventoryButton.onClick.RemoveListener(ExpandAbilityInventory);

    //    AbilitiesInventory.GetComponent<RectTransform>().DOAnchorPosY(280, 1);
    //    AbilitiesInventoryButton.onClick.AddListener(HideAbilityInventory);

    //}

    //public void HideAbilityInventory()
    //{
    //    AbilitiesInventoryButton.onClick.RemoveListener(HideAbilityInventory);
    //    AbilitiesInventory.GetComponent<RectTransform>().DOMoveY(50, 1);
    //    AbilitiesInventoryButton.onClick.AddListener(ExpandAbilityInventory);
    //}

    public void GameOver()
    {
        SceneManager.LoadScene("GameOverScene");
    }

    public void EnableCanvas()
    {
        RectTransform[] children = GetComponentsInChildren<RectTransform>();
        foreach (RectTransform i in children)
        {
            i.gameObject.SetActive(true);
        }
    }

    public void DisableCanvas()
    {
        RectTransform[] children = GetComponentsInChildren<RectTransform>();
        foreach (RectTransform i in children)
        {
            i.gameObject.SetActive(false);
        }
    }

    public void UpdateAbilityInventory(Player player)
    {
        //EventManager.OnSelectPlayer(player);

        //PlayerManager.Instance.PlayerSelect(PlayerManager.Instance.Players.IndexOf(player));
        //for (int i = 0;i < abilityButtons.Count;i++)
        //{
        //    if(player.AbilityInventory.Count > i)
        //    {
        //        abilityButtons[i].AssignAbility(player);
        //        //abilityButtons[i].GetComponent<Button>().image.sprite = player.AbilityInventory[i].AbilityUISprite;
        //    }
        //    else
        //    {
        //        abilityButtons[i].GetComponent<Button>().image.sprite = emptyAbilitySlotSprite;
        //    }
        //}

    }

}
