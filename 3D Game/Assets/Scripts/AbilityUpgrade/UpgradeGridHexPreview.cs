using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UpgradeGridHexPreview : UpgradeGridHex
{
    public UpgradeGridHex HexToPlace;

    public UpgradeHexGrid upgradeHexGrid;

    public Vector2Int coordinateToPlace;

    Vector3 previousPosition;

    private void Start()
    {
        previousPosition = Vector3.zero;
    }

    public void Place()
    {
        if (upgradeHexGrid.Grid.ContainsKey(coordinateToPlace))
        {
            upgradeHexGrid.UpgradeAbility(coordinateToPlace, HexToPlace.effect);
        }
        Destroy(gameObject);
    }

    private void Update()
    {
        Vector2 mousePos = Pointer.current.position.ReadValue();

        Vector2 SceenCenter = mousePos - (Vector2)upgradeHexGrid.transform.position;

        Vector2Int GridCoord = HexGridUtil.PixelToHexCoord2D(SceenCenter, 50);

        if (upgradeHexGrid.Grid.ContainsKey(GridCoord))
        {
            UpgradeGridHex target = upgradeHexGrid.Grid[GridCoord];
            if (determinePlaceablitily(target, GridCoord))
            {
                transform.position = upgradeHexGrid.Grid[GridCoord].transform.position;
                coordinateToPlace = GridCoord;
                previousPosition = transform.position;
            }
            else
            {
                transform.position = previousPosition;
            }

        }
        else
        {
            transform.position = mousePos;
        }
    }

    public void Initialize(UpgradeHexGrid hexGrid, UpgradeGridHex toPlace)
    {
        this.Fill(toPlace.image.sprite);

        upgradeHexGrid = hexGrid;
        HexToPlace = toPlace;
    }

    public bool determinePlaceablitily(UpgradeGridHex target, Vector2Int GridCoord)
    {
        if (GridCoord != Vector2Int.zero)
        {
            if (HexToPlace.effect == Effect.Positive && target.effect == Effect.Neutral)
            {
                return true;
            }

            if ((int)HexToPlace.effect >= 3)
            {
                switch (target.effect)
                {
                    case Effect.Positive: return false;
                    case Effect.Negative500: return false;
                    default: return true;
                }
            }
            return false;
        }
        return false;
    }
}
