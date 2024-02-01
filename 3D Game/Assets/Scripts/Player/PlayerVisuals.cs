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

    [SerializeField] float shootDuraiton;
    [SerializeField] public float moveDuration;

    [SerializeField] List<ParticleSystem> hoverEffect;
    [SerializeField] List<ParticleSystem> SuckEffect;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Start()
    {
        EventManager.OnMoveEvent += Movement;
        EventManager.OnSelectPlayerEvent += PlayerSelection;
        //EventManager.OnAbilityCastEvent += AbilityCast;

    }

    private void OnDestroy()
    {
        EventManager.OnMoveEvent -= Movement;
        EventManager.OnSelectPlayerEvent -= PlayerSelection;
    }

    public void AbilityCast(Player player)
    {
        if(player == playwer)
        {
            StartCoroutine(AnimationCoroutine(shootDuraiton, "IsShooting"));
        }
    }


    public void Movement(Player player)
    {
        try
        {
            if (player == playwer)
            {
                //ParticleSystem landingCloud = GetComponentInChildren<ParticleSystem>();
                //transform.DOLocalJump(Vector3.up * 0.45f, 2, 1, .25f);
                //transform.DOPunchScale(Vector3.one * .1f, .25f).OnComplete(landingCloud.Play);
                StartCoroutine(MovementCoroutine(player));
            }

        }
        catch
        {

        }

    }

    private void StopParticles(List<ParticleSystem> parts)
    {
        foreach(ParticleSystem part in parts)
        {
            part.Stop();
        }
    }

    private void PlayParticles(List<ParticleSystem> parts)
    {
        foreach (ParticleSystem part in parts)
        {
            part.gameObject.SetActive(true);
            part.Play();
        }
    }


    public IEnumerator MovementCoroutine(Player player)
    {
        StartCoroutine(AnimationCoroutine(moveDuration, "IsMoving"));

        if(GridManager.Instance.Grid[player.CoordinatePosition].currentGridState.StateValue() == 1)
        {
            StopParticles(hoverEffect);
        }
        yield return new WaitForSeconds(moveDuration);
        if (GridManager.Instance.Grid[player.CoordinatePosition].currentGridState.StateValue() == 1)
        {
            PlayParticles(SuckEffect);
        }
        GridManager.Instance.Grid[player.CoordinatePosition].currentGridState.PlayerEnters(GridManager.Instance.Grid[player.CoordinatePosition]);
        yield return new WaitForSeconds(0.35f);
        if (GridManager.Instance.Grid[player.CoordinatePosition].currentGridState.StateValue() == 1)
        {
            PlayParticles(hoverEffect);
        }
    }

    public IEnumerator AnimationCoroutine(float duration, string animation)
    {
        animator.SetBool(animation,true);
        yield return new WaitForSeconds(duration);
        animator.SetBool(animation,false);
        yield return null;
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
            MeshRenderer[] mr = GetComponentsInChildren<MeshRenderer>();
            foreach(MeshRenderer mr2 in mr)
            {
                mr2.materials = PlayerVisuals;
            }
            
        }

        else
        {
            Material[] PlayerVisual =
            {
                    PlayerMaterial
            };

            MeshRenderer[] mr = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer mr2 in mr)
            {
                mr2.materials = PlayerVisual;
            }
        }
            

        
    }
}
