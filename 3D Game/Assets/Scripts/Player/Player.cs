using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;

public class Player : MonoBehaviour
{
    public List<Ability> AbilityInventory;

    public Vector2Int CoordinatePosition;

    public Vector2Int SpawnPoint;

    [SerializeField] CinemachineVirtualCamera PlayerCam;



    public void PlayerSelect()
    {
        CameraRotation.Instance.MainCam = true;
        switch(CameraRotation.Instance.keyPressed)
        {
            case 1:
                PlayerManager.Instance.selectedPlayer  = PlayerManager.Instance.Players[0];
     
                break;

            case 2:
                PlayerManager.Instance.selectedPlayer = PlayerManager.Instance.Players[1];
                break;

            case 3:
                PlayerManager.Instance.selectedPlayer = PlayerManager.Instance.Players[2];
                break;
        }
        PlayerCam.LookAt = PlayerManager.Instance.selectedPlayer.transform; 
        PlayerCam.Follow = PlayerManager.Instance.selectedPlayer.transform;  
        CameraRotation.Instance.SwitchToPlayer();
        
    }
}


