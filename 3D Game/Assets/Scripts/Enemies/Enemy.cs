using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class Enemy : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    MeshRenderer mr; 
    [HideInInspector] public Ressource ressource;
    [HideInInspector] public Vector2Int axialLocation;
    [HideInInspector] public int currentHealth;

    [Header("Visuals")]
    [SerializeField] public Material AMAterial;
    [SerializeField] public Material BMAterial;
    [SerializeField] public Material CMAterial;
    [SerializeField] public Material DMAterial;

    [SerializeField] public Material AOutlineMAterial;
    [SerializeField] public Material BOutlineMAterial;
    [SerializeField] public Material COutlineMAterial;
    [SerializeField] public Material DOutlineMAterial;

    [SerializeField] GameObject HealthPointsLayout;
    [SerializeField] Image HealthpointPrefab;
    List<Image> healthpoints;

    [SerializeField] AnimationCurve SpawnJumpAnimationCurve;
    [SerializeField] GameObject Particle_EnemyDeath;
    //[SerializeField] public EnemySO enemySO;

    [Header("Enemy Stats")]

    
    [SerializeField] public int maxHealth;
    public int SkillPointReward;

   

    [Header("Negative Spread")]
    [SerializeField] public int SpreadAmount;
    [SerializeField] public float everyXTurns;
    [SerializeField] public List<Spreadbehaviours> spreadbehaviours;

    [Header("Audio")]

    public AudioData EnemyDeath;
    public AudioData EnemyDamage;
    public AudioData BossDamage;
    public AudioData spawn;
    public AudioData SpreadBar;
    public AudioData EnemySpawn;
    public AudioData BossSpread;
    public AudioMixerGroup soundEffect;


    Image hp;

    private void Awake()
    {
        AudioManager.Instance.PlaySoundAtLocation(spawn, soundEffect, null);

        if (GetComponent<Boss>() == null)
        {
            AudioManager.Instance.PlaySoundAtLocation(EnemySpawn, soundEffect, null);

        }

        else
        {
            Debug.Log("Hi");
            AudioManager.Instance.PlaySoundAtLocation(BossSpread, soundEffect, null);
        }

    }

    [HideInInspector] public bool FirstAndLast = true;
    private void Start()
    {
        EventManager.OnEndTurnEvent += Spread;
   
    }

    private void OnDestroy()
    {
        EventManager.OnEndTurnEvent -= Spread;
    }

    virtual public void Setup(GridTile tile)
    {
      //  Debug.Log($"I am an enemy and i setup at {tile.AxialCoordinate}");
        currentHealth = maxHealth;
        ressource = tile.ressource;
        axialLocation = tile.AxialCoordinate;
        //transform.position = tile.transform.position;
        //transform.parent = tile.transform;
        transform.SetParent(tile.gameObject.transform);
        mr = GetComponentInChildren<MeshRenderer>();


        //transform.DOScale(transform.localScale, 0.5f).From(Vector3.one * 0.3f);
        //transform.DOPunchScale(Vector3.one * Random.Range(0.5f, 1), 1f);
        ParticleSystem landingCloud = GetComponentInChildren<ParticleSystem>();
        try
        {

            transform.DOComplete();
            transform.DOJump(tile.transform.position, 5, 1, .4f)/*.SetEase(SpawnJumpAnimationCurve)*/.OnComplete(() => transform.DOPunchScale(Vector3.down * 0.7f, .2f)).OnComplete(landingCloud.Play);
        }
        catch
        {

        }



        mr = GetComponentInChildren<MeshRenderer>();
        switch (ressource)
        {
            case Ressource.ressourceA:
                mr.materials = new Material[] { AMAterial, AOutlineMAterial };
                break;
            case Ressource.ressourceB:
                mr.materials = new Material[] { BMAterial, BOutlineMAterial };
                break;
            case Ressource.ressourceC:
                mr.materials = new Material[] { CMAterial, COutlineMAterial };
                break;
            case Ressource.ressourceD:
                mr.materials = new Material[] { DMAterial, DOutlineMAterial };
                break;
        }

        foreach(Image img in GetComponentsInChildren<Image>())
        {
            Destroy(img.gameObject);
        }

        healthpoints = new List<Image>();
        for (int i = 0; i < currentHealth; i++)
        {
            Image hpp = Instantiate(HealthpointPrefab, HealthPointsLayout.transform);
            healthpoints.Add(hpp);
        }

        tile.ChangeCurrentState(GridManager.Instance.gS_Enemy);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            SaveManager.Instance.TotalKills++;
            
            Death();
           
        }
        else
        {
            for (int i = 0; i < damage; i++)
            {
                if (GetComponent<Boss>() != null)
                {
                    AudioManager.Instance.PlaySoundAtLocation(BossDamage, soundEffect, null);
                    hp = healthpoints[healthpoints.Count - 1];
                    healthpoints.RemoveAt(healthpoints.Count - 1);
                    SaveManager.Instance.DamageDealt++;
                }
                else
                    AudioManager.Instance.PlaySoundAtLocation(EnemyDamage, soundEffect, null);
                 hp = healthpoints[healthpoints.Count - 1];
                healthpoints.RemoveAt(healthpoints.Count - 1);
                SaveManager.Instance.DamageDealt++;


                Destroy(hp.gameObject);
            }
        }
    }

    public virtual void Death()
    {
        PlayerManager.Instance.SkillPoints += SkillPointReward;
        GetComponentInParent<GridTile>().ChangeCurrentState(GridManager.Instance.gS_Negative);
        Instantiate(Particle_EnemyDeath, transform.position, Quaternion.Euler(-90, 0, 0));
        AudioManager.Instance.PlaySoundAtLocation(EnemyDeath, soundEffect, null);


        Destroy(gameObject);
    }

    public virtual void Spread()
    {
        if (GridManager.Instance.TurnCounter % everyXTurns != 0)
        {
            return;
        }
        for (int i = 0; i < SpreadAmount; i++)
        {
            foreach (Spreadbehaviours sb in spreadbehaviours)
            {
                if (sb.TargetTile(HexGridUtil.AxialToCubeCoord(axialLocation), out Vector3Int target, FindClosestPlayer().CoordinatePosition))
                {
                    GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(target)].ChangeCurrentState(GridManager.Instance.gS_Negative);
                    SaveManager.Instance.TotalSpread++;
                    AudioManager.Instance.PlaySoundAtLocation(SpreadBar, soundEffect, null);
                    break;
                }
            }
        }
    }

    public List<GridTile> SpreadTiles()
    {
        foreach (Spreadbehaviours sb in spreadbehaviours)
        {
            if (sb.TargetTiles(HexGridUtil.AxialToCubeCoord(axialLocation), out List<Vector3Int> targets, FindClosestPlayer().CoordinatePosition))
            {
                List<GridTile> result = new List<GridTile>();
                foreach (Vector3Int coord in targets)
                {
                    if (GridManager.Instance.Grid.ContainsKey(HexGridUtil.CubeToAxialCoord(coord)))
                    {
                        result.Add(GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(coord)]);
                    }
                }
                return result;

            }
        }
        return new List<GridTile>();
    }

    public Player FindClosestPlayer()
    {
        Player closestPlayer = PlayerManager.Instance.Players[0];
        int Distance = HexGridUtil.CubeDistance(HexGridUtil.AxialToCubeCoord(axialLocation), HexGridUtil.AxialToCubeCoord(closestPlayer.CoordinatePosition));
        for (int i = 1; i < PlayerManager.Instance.Players.Count; i++)
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

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (TileHighlight tileHighlight in GridManager.Instance.gameObject.GetComponentsInChildren<TileHighlight>())
        {
            Destroy(tileHighlight.gameObject);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        List<GridTile> potentialSpreadTiles = SpreadTiles();
        if (potentialSpreadTiles.Count > 0)
        {
            Debug.Log("I should have some tiles to highlight the potential Spread");
            foreach (GridTile tile in potentialSpreadTiles)
            {
                tile.HighlightEnemySpreadPrediction();
            }
        }
    }

    public virtual int AmountSpreadNextTurn()
    {
        if ((GridManager.Instance.TurnCounter+1) % everyXTurns == 0)
        {
            return SpreadAmount;
        }
        return 0;
    }
}
