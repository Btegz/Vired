using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Button EndTurnButton;
    [SerializeField] Image negativeFillBar;

    [SerializeField] TMP_Text ressourceAText;
    [SerializeField] TMP_Text ressourceBText;
    [SerializeField] TMP_Text ressourceCText;
    [SerializeField] TMP_Text ressourceDText;

    // Start is called before the first frame update
    void Start()
    {
        EndTurnButton.onClick.AddListener(EventManager.OnEndTurn);
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
                negativeFillBar.fillAmount = (float)negativeTiles.Count / (float)GridManager.Instance.Grid.Count;
            }
            else
            {
                negativeFillBar.fillAmount = 0;
            }
        }

        ressourceAText.text = PlayerManager.Instance.RessourceAInventory.ToString();
        ressourceBText.text = PlayerManager.Instance.RessourceBInventory.ToString();
        ressourceCText.text = PlayerManager.Instance.RessourceCInventory.ToString();
        ressourceDText.text = PlayerManager.Instance.RessourceDInventory.ToString();

    }
}
