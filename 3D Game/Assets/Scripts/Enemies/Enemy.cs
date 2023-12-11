using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    //[SerializeField] public EnemySO enemySO;

    [SerializeField] public int currentHealth;
    [SerializeField] public int maxHealth;

    [SerializeField] public Ressource ressource;

    [SerializeField] public Material AMAterial;
    [SerializeField] public Material BMAterial;
    [SerializeField] public Material CMAterial;
    [SerializeField] public Material DMAterial;

    [SerializeField] GameObject HealthPointsLayout;
    [SerializeField] Image HealthpointPrefab;
    [SerializeField] List<Image> healthpoints;


    [SerializeField] GameObject Particle_EnemyDeath;
    public int SkillPointReward;

    public MeshRenderer mr;
    [SerializeField] public List<Spreadbehaviours> spreadbehaviours;
    public Vector2Int axialLocation;    
    [SerializeField] public AudioData audioData;
    public AudioData death;
    public AudioData spawn;
    [HideInInspector] public bool FirstAndLast = true;

    private void Awake()
    {
        AudioManager.Instance.PlaySoundAtLocation(spawn);
    }

    private void Start()
    {
        EventManager.OnEndTurnEvent += Spread;
    }

    private void OnDestroy()
    {
        EventManager.OnEndTurnEvent -= Spread;
    }

    public void Setup(/*EnemySO enemySO, */GridTile tile)
    {
        //this.enemySO = enemySO;

        //currentHealth = enemySO.myCurrentHealth;
        //maxHealth = enemySO.mymaxHealth;
        ressource = tile.ressource;
        axialLocation = tile.AxialCoordinate;

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        //meshFilter.mesh = enemySO.myMesh;
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        //meshRenderer.material = enemySO.myMaterial;

        transform.DOPunchScale(Vector3.one * Random.Range(0.5f, 1), 1f);

        mr = GetComponent<MeshRenderer>();
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
            //if (gameObject.TryGetComponent<Boss>(out Boss boss))
            //{
            //    boss.BossDeath(boss.location[0]);
            //    if (FirstAndLast)
            //    {
            //        Destroy(gameObject);
            //    }

            //    return;
            //}
            Death();
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

    public virtual void Death()
    {
        PlayerManager.Instance.SkillPoints += SkillPointReward;
        GetComponentInParent<GridTile>().ChangeCurrentState(GridManager.Instance.gS_Neutral);
        Instantiate(Particle_EnemyDeath, transform.position, Quaternion.Euler(-90, 0, 0));
        Destroy(gameObject);
    }

    public virtual void Spread()
    {
        foreach(Spreadbehaviours sb in spreadbehaviours)
        {
            if(sb.TargetTile(HexGridUtil.AxialToCubeCoord(axialLocation), out Vector3Int target, FindClosestPlayer().CoordinatePosition))
            {
                GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(target)].ChangeCurrentState(GridManager.Instance.gS_Negative); 
                break;
            }
        }
    }

    public Player FindClosestPlayer()
    {
        Player closestPlayer = PlayerManager.Instance.Players[0];
        int Distance = HexGridUtil.CubeDistance(HexGridUtil.AxialToCubeCoord(axialLocation),HexGridUtil.AxialToCubeCoord(closestPlayer.CoordinatePosition));
        for(int i = 1; i < PlayerManager.Instance.Players.Count; i++)
        {
            Player player = PlayerManager.Instance.Players[i];  
            int newDistance = HexGridUtil.CubeDistance(HexGridUtil.AxialToCubeCoord(axialLocation), HexGridUtil.AxialToCubeCoord(player.CoordinatePosition));
            if (newDistance < Distance)
            {
                closestPlayer = player;
                Distance = newDistance;
            }
        }
        return closestPlayer;
    }
}
