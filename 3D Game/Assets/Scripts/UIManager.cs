using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [SerializeField] Button EndTurnButton;
    [SerializeField] Button AbilitiesInventoryButton;
    [SerializeField] Image negativeFillBar;

    [SerializeField] TMP_Text ressourceAText;
    [SerializeField] TMP_Text ressourceBText;
    [SerializeField] TMP_Text ressourceCText;
    [SerializeField] TMP_Text ressourceDText;

    [SerializeField] GameObject AbilitiesInventory;

    [SerializeField] List<AbilityCastButton> abilityButtons;

    [SerializeField] Sprite emptyAbilitySlotSprite;

    [SerializeField] GameObject PlayerButtonParent;
    [SerializeField] PlayerButton playerButtonPrefab;
    [SerializeField] public List<PlayerButton> playerButtons;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Player player in PlayerManager.Instance.Players)
        {
            PlayerButton playerButton = Instantiate(playerButtonPrefab, PlayerButtonParent.transform);
            playerButton.Setup(player);
        }
        EndTurnButton.onClick.AddListener(EventManager.OnEndTurn);
        EndTurnButton.onClick.AddListener(GridManager.Instance.TriggerPhase);
        EventManager.OnSelectPlayerEvent += UpdateAbilityInventory;
        //AbilitiesInventoryButton.onClick.AddListener(ExpandAbilityInventory);
    }

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
        EventManager.OnSelectPlayerEvent -= UpdateAbilityInventory;
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
        for (int i = 0;i < abilityButtons.Count;i++)
        {
            if(player.AbilityInventory.Count > i)
            {
                //abilityButtons[i].GetComponent<Button>().image.sprite = player.AbilityInventory[i].AbilityUISprite;
            }
            else
            {
                abilityButtons[i].GetComponent<Button>().image.sprite = emptyAbilitySlotSprite;
            }
        }
        
    }

}
