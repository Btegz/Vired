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
                transform.DOLocalJump(Vector3.zero, 2, 1, .25f);
                transform.DOPunchScale(Vector3.one * .1f, .25f).OnComplete(landingCloud.Play);
            }

        }
        catch
        {
           
        }

    }
}
