using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    [SerializeField] public EnemySO enemySO;

    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;

    [SerializeField] Ressource ressource;

    private void Awake()
    {
        
    }

    public void Setup(EnemySO enemySO)
    {
        this.enemySO = enemySO;

        currentHealth = enemySO.myCurrentHealth;
        maxHealth = enemySO.mymaxHealth;
        ressource = enemySO.myRessource;

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = enemySO.myMesh;
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = enemySO.myMaterial;

        transform.DOPunchScale(Vector3.one*Random.Range(0.5f,1), 1f);
    }

}
