using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Stats : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject StatsCanvas;
    public TMP_Text round;
    public TMP_Text currentSpread;
    public TMP_Text expectedSpread;
    public TMP_Text spawn;
    public TMP_Text resources;

    private int resourceCount;
    private int negativityCount;
    public void OnPointerEnter(PointerEventData eventData)
    {
        StatsCanvas.SetActive(true);
        GridStats();

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StatsCanvas.SetActive(false);
    }

    public void Update()
    {

        round.GetComponent<TextMeshProUGUI>().text = GridManager.Instance.TurnCounter.ToString();
        resources.GetComponent<TextMeshProUGUI>().text = resourceCount.ToString();
        currentSpread.GetComponent<TextMeshProUGUI>().text = negativityCount.ToString();

    }


    public void GridStats()
    {
        if (PlayerPrefs.HasKey("Resources"))
        {
            resourceCount = PlayerPrefs.GetInt("Resources");
        }

        else
        {
            foreach (KeyValuePair<Vector2Int, GridTile> grid in GridManager.Instance.Grid)
            {
                if (grid.Value.currentGridState == GridManager.Instance.gS_Positive)
                {
                    resourceCount++;
                }
            }
            PlayerPrefs.SetInt("Resources", resourceCount);
        }


        if (PlayerPrefs.HasKey("Contamination"))
        {
            negativityCount = PlayerPrefs.GetInt("Contamination");
        }

        else
        {
            foreach (KeyValuePair<Vector2Int, GridTile> grid in GridManager.Instance.Grid)
            {
                if (grid.Value.currentGridState == GridManager.Instance.gS_Boss || GridManager.Instance.gS_BossNegative || GridManager.Instance.gS_Negative || GridManager.Instance.gS_Enemy)
                {
                    negativityCount++;
                }
            }
        }
        PlayerPrefs.SetInt("Contamination", negativityCount);
    }
}



