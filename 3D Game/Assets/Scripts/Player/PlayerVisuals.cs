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
  
    public void Movement(Player player)
    {
        try
        {
            if (player == playwer)
            {
                ParticleSystem landingCloud = GetComponentInChildren<ParticleSystem>();
                transform.DOLocalJump(/*PlayerManager.Instance.target.transform.position*/Vector3.zero, 2, 1, .25f)
                 .OnComplete(() => PlayerManager.Instance.target.currentGridState.PlayerEnters(PlayerManager.Instance.target));
                transform.DOPunchScale(Vector3.one * .1f, .25f).OnComplete(landingCloud.Play);
            }
            
           /* else if(PlayerManager.Instance.movementAction == 0 && Mouse.current.leftButton.wasPressedThisFrame && !PlayerManager.Instance.abilityActivated && PlayerManager.Instance.extraMovement == 0)
            {
                tween = transform.DOPunchScale(Vector3.one* .1f, .25f) ;
            }

            PlayerManager.Instance.moving = false;*/
        }
        catch
        {

        }

    }
}
