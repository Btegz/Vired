using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Audio;

public class Player : MonoBehaviour, IPointerClickHandler

{
    public Canvas AbilityCastCanvas;
    public Button AbilityCastButton;

    public List<Ability> AbilityInventory;

    public Vector2Int CoordinatePosition;

    public Vector2Int SpawnPoint;

    public Sprite pic;

    [SerializeField] CinemachineVirtualCamera PlayerCam;
    public ParticleSystem circle;



   
    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerManager.Instance.selectedPlayer = this;
        EventManager.OnSelectPlayer(this);
    }

    public void OpenAbilityCastCanvas()
    {
        AbilityCastCanvas.enabled = true;
        circle.Play();
        AbilityCastCanvas.transform.DOMove(AbilityCastCanvas.transform.position,0.2f).From(transform.position).OnComplete(()=> AbilityCastCanvas.transform.DOPunchScale(Vector3.one, 0.2f));
    }

    public void CloseAbilityCastCanvas()
    {
        circle.Stop();

        AbilityCastCanvas.enabled = false;
    }

 //   ("`-''-/").___..--''"`-._ 
 //`6_ 6  )   `-.  (     ).`-.__.`) 
 //(_Y_.)'  ._   )  `._ `. ``-..-' 
 //  _..`--'_..-_/  /--'_.'
 // ((((.-''  ((((.'  (((.-' 
 

}


