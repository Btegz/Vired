using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerVisuals : MonoBehaviour
{

    public void Update()
    {
        Movement();
    }
    public void Movement()
    {
        try
        {
            if (PlayerManager.Instance.moving == true)
            {
                ParticleSystem landingCloud = GetComponentInChildren<ParticleSystem>();
                transform.DOJump(PlayerManager.Instance.target.transform.position, 2, 1, .25f)
                 .OnComplete(() => PlayerManager.Instance.target.currentGridState.PlayerEnters(PlayerManager.Instance.target));
                transform.DOPunchScale(Vector3.one * .1f, .25f).OnComplete(landingCloud.Play);

            }
            else if(PlayerManager.Instance.movementAction == 0 && Mouse.current.leftButton.wasPressedThisFrame && !PlayerManager.Instance.abilityActivated && PlayerManager.Instance.extraMovement == 0)
            {
               transform.DOPunchRotation(new Vector3(10f, 2f), 1f);
            }
           
            PlayerManager.Instance.moving = false;
        }
        catch
        {

        }

    }
}
