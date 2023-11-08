using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour, IPointerClickHandler
{
    public List<Ability> AbilityInventory;

    public Vector2Int CoordinatePosition;

    public Vector2Int SpawnPoint;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("I AM THE SELECTED ONE " + transform.name);
        PlayerManager.Instance.selectedPlayer = this;
    }
}
