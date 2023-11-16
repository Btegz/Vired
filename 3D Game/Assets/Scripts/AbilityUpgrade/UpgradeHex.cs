using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class UpgradeHex : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] UpgradeHexGrid upgradeHexGrid;

    [SerializeField] UpgradeGridHex newHex;

    [SerializeField] UpgradeGridHexPreview gridHexPrefab;

    UpgradeGridHexPreview gridHex;

    public void OnBeginDrag(PointerEventData eventData)
    {
        gridHex = Instantiate(gridHexPrefab, this.transform);
        gridHex.Initialize(upgradeHexGrid, newHex);
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        gridHex.Place();
    }
}
