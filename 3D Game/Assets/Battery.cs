using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battery : MonoBehaviour
{
    [SerializeField] List<MovePointsDoTween> currentMovementPoints;
    [SerializeField] List<MovePointsDoTween> bonusMovementPoints;

    [SerializeField] MovePointsDoTween bonusMovementPointPrefab;

    RectTransform rectTransform;

    [SerializeField] float movepointHeight = 10f;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        EventManager.BonusMovementPointGainEvent += GetBonusMovementPoints;
        EventManager.BonusMovementPointLossEvent += RemoveBonusMovementPoint;
        bonusMovementPoints = new List<MovePointsDoTween>();
    }

    private void OnDestroy()
    {
        EventManager.BonusMovementPointGainEvent -= GetBonusMovementPoints;
        EventManager.BonusMovementPointLossEvent -= RemoveBonusMovementPoint;
    }

    public void GetBonusMovementPoints(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            MovePointsDoTween newBonusPoint = Instantiate(bonusMovementPointPrefab);
            newBonusPoint.transform.SetParent(this.transform);
            Debug.Log($"my prev Height:{rectTransform.rect.height}");
            rectTransform.sizeDelta += new Vector2(0, movepointHeight);
            Debug.Log($"my new Height:{rectTransform.rect.height}");
            bonusMovementPoints.Add(newBonusPoint);
        }
    }

    public void RemoveBonusMovementPoint(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (bonusMovementPoints.Count <= 0)
            {
                break;
            }
            MovePointsDoTween mybonus = bonusMovementPoints[0];
            bonusMovementPoints.RemoveAt(0);
            mybonus.transform.SetParent(GetComponentInParent<ToolTipContent>().transform);
            Destroy(mybonus.gameObject);
            rectTransform.sizeDelta -= new Vector2(0, movepointHeight);
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }
    }
}
