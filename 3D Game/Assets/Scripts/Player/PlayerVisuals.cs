using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerVisuals : MonoBehaviour
{

    [SerializeField] Player playwer;
    Tweener tween;

    public void Start()
    {
        EventManager.OnMoveEvent += Movement;
    }
  

    
    public void Shake()
    {
        if ((GridManager.Instance.Grid[PlayerManager.Instance.clickedTile].currentGridState != GridManager.Instance.gS_Neutral && Mouse.current.leftButton.wasPressedThisFrame) || (GridManager.Instance.Grid[PlayerManager.Instance.clickedTile].currentGridState != GridManager.Instance.gS_Positive && Mouse.current.leftButton.wasPressedThisFrame))
        {

            transform.DOPunchRotation(new Vector3(2f, 10f), .5f);
        }
    }
    public void Movement(Player player)
    {
        try
        {
            if (player == playwer)
            {
                ParticleSystem landingCloud = GetComponentInChildren<ParticleSystem>();
                transform.DOLocalJump(Vector3.zero, 2, 1, .25f);
                transform.DOPunchScale(Vector3.one * .1f, .25f).OnComplete(landingCloud.Play);
            }

        }
        catch
        {
           
        }

    }
}
