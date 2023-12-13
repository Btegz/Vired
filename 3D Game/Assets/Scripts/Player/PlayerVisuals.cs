using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerVisuals : MonoBehaviour
{

    [SerializeField] Player playwer;
    Tweener tween;
    public Material PlayerMaterial;
    public Material Outline;

    public void Start()
    {
        EventManager.OnMoveEvent += Movement;
        EventManager.OnSelectPlayerEvent += PlayerSelection;

    }

    private void OnDestroy()
    {
        EventManager.OnMoveEvent -= Movement;
        EventManager.OnSelectPlayerEvent -= PlayerSelection;
    }

    public void Movement(Player player)
    {
        try
        {
            if (player == playwer)
            {
                ParticleSystem landingCloud = GetComponentInChildren<ParticleSystem>();
                transform.DOLocalJump(Vector3.up * 0.45f, 2, 1, .25f);
                transform.DOPunchScale(Vector3.one * .1f, .25f).OnComplete(landingCloud.Play);
            }

        }
        catch
        {

        }

    }

    public void PlayerSelection(Player player)
    {

        if (player == playwer)
        {

            Material[] PlayerVisuals =
        {
            PlayerMaterial,
            Outline
        };
            MeshRenderer mr = GetComponentInParent<MeshRenderer>();

            mr.materials = PlayerVisuals;
        }

        else
        {
            Material[] PlayerVisual =
            {
                    PlayerMaterial
            };

            MeshRenderer mr = GetComponentInParent<MeshRenderer>();

            mr.materials = PlayerVisual;
        }
            

        
    }
}
