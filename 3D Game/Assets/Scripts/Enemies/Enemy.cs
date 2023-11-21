using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] public EnemySO enemySO;

    [SerializeField] public int currentHealth;
    [SerializeField] public int maxHealth;

    [SerializeField] public Ressource ressource;

    [SerializeField] Material AMAterial;
    [SerializeField] Material BMAterial;
    [SerializeField] Material CMAterial;
    [SerializeField] Material DMAterial;

    [SerializeField] GameObject HealthPointsLayout;
    [SerializeField] Image HealthpointPrefab;
    [SerializeField] List<Image> healthpoints;


    [SerializeField] GameObject Particle_EnemyDeath;


    private void Awake()
    {

    }

    public void Setup(EnemySO enemySO, GridTile tile)
    {
        this.enemySO = enemySO;

        //currentHealth = enemySO.myCurrentHealth;
        maxHealth = enemySO.mymaxHealth;
        ressource = tile.ressource;

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = enemySO.myMesh;
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = enemySO.myMaterial;

        transform.DOPunchScale(Vector3.one * Random.Range(0.5f, 1), 1f);

        MeshRenderer mr = GetComponent<MeshRenderer>();
        switch (ressource)
        {
            case Ressource.ressourceA:
                mr.material = AMAterial;
                break;
            case Ressource.ressourceB:
                mr.material = BMAterial;
                break;
            case Ressource.ressourceC:
                mr.material = CMAterial;
                break;
            case Ressource.resscoureD:
                mr.material = DMAterial;
                break;
        }

        for (int i = 0; i < currentHealth; i++)
        {
            Image hpp = Instantiate(HealthpointPrefab);
            healthpoints.Add(hpp);
            hpp.transform.SetParent(HealthPointsLayout.transform);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            PlayerManager.Instance.SkillPoints++;
            GetComponentInParent<GridTile>().ChangeCurrentState(GridManager.Instance.gS_Negative);
            PlayerManager.Instance.SkillPoints++;
            Instantiate(Particle_EnemyDeath, transform.position, Quaternion.Euler(-90, 0, 0));
            Destroy(gameObject);
        }
        else
        {
            for (int i = 0; i < damage; i++)
            {
                Image hp = healthpoints[healthpoints.Count - 1];
                healthpoints.RemoveAt(healthpoints.Count - 1);
                Destroy(hp.gameObject);
            }
        }
    }

}
