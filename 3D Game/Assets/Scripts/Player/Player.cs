using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;

public class Player : MonoBehaviour, IPointerClickHandler

{
    public List<Ability> AbilityInventory;

    public Vector2Int CoordinatePosition;

    public Vector2Int SpawnPoint;


    [SerializeField] CinemachineVirtualCamera PlayerCam;


    public void OnPointerClick(PointerEventData eventData)
    {
        
        PlayerManager.Instance.selectedPlayer = this;
        EventManager.OnSelectPlayer(this);
        PlayerCam.LookAt = PlayerManager.Instance.selectedPlayer.transform;
        PlayerCam.Follow = PlayerManager.Instance.selectedPlayer.transform;
        //CameraRotation.Instance.SwitchToPlayer();
    }

 

}


