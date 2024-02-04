using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

[System.Serializable]
public class EnemyMassLayer
{
    [SerializeField] public GameObject MassPrefab;
    [SerializeField] public Vector2Int AmountFromTo;
    [SerializeField] public Vector2 YRotationFromTo;
    [SerializeField] public Vector2 XScaleFromTo;
    [SerializeField] public Vector2 ZScaleFromTo;
    [SerializeField] public Vector2 YOffsetFromTo;
    [SerializeField][Range(0, 6)] public int MassNeighborThreshold;
}


public class RessourceVisuals : MonoBehaviour
{
    [Header("TileRessourceParticleSystems")]
    [SerializeField] List<ParticleSystem> PondRessourceParticleSystems;
    [SerializeField] List<ParticleSystem> CocoonRessourceParticleSystems;
    [SerializeField] List<ParticleSystem> SpikesRessourceParticleSystems;
    [SerializeField] List<ParticleSystem> MossRessourceParticleSystems;
    [Header("-------------------------------------")]
    [SerializeField] List<ParticleSystem> PondCleanParticleSystems;
    [SerializeField] List<ParticleSystem> CocoonCleanParticleSystems;
    [SerializeField] List<ParticleSystem> SpikesCleanParticleSystems;
    [SerializeField] List<ParticleSystem> MossCleanParticleSystems;
    [Header("-------------------------------------")]
    [SerializeField] List<ParticleSystem> EnemyMassParticleSystems;
    [Header("-------------------------------------")]

    [Header("Aesthetic MeshesGeneration")]
    [SerializeField] List<TerrainFeature> BäumeFeatures;
    [SerializeField] Vector2 BäumeNoiseValueFromTo;
    [SerializeField] Vector2 CenterLerpBäumeFromTo;
    [SerializeField] Vector2Int BäumeAmountFromTo;
    [SerializeField] float propabilityBäume;


    [SerializeField] List<TerrainFeature> SpikeFeatures;
    [SerializeField] Vector2 SpikeNoiseValueFromTo;
    [SerializeField] Vector2 CenterLerpSpikeFromTo;
    [SerializeField] Vector2Int SpikeAmountFromTo;
    [SerializeField] float propabilitySpike;

    [SerializeField] List<TerrainFeature> RockFeatures;
    [SerializeField] Vector2 RockNoiseValueFromTo;
    [SerializeField] Vector2 CenterLerpRockFromTo;
    [SerializeField] Vector2Int RockAmountFromTo;
    [SerializeField] float propabilityRock;


    [Header("Klopse")]
    [SerializeField] List<TerrainFeature> KlopseA;
    [SerializeField] Vector2Int howManyKlopseAFromTo;
    //[SerializeField] float randomRadius;
    [SerializeField] Vector2 randomXOffsetAFromTo;
    [SerializeField] Vector2 randomZOffsetAFromTo;
    [SerializeField] Vector2 randomRotationAFromTo;
    [SerializeField] Vector2 randomYOffsetAFromTo;
    [SerializeField] Vector2 randomACenterLerpFrmoTo;
    [Header("-------------------------------------")]
    [SerializeField] List<TerrainFeature> KlopseB;
    [SerializeField] Vector2Int howManyKlopseBFromTo;
    [SerializeField] Vector2 randomXOffsetBFromTo;
    [SerializeField] Vector2 randomZOffsetBFromTo;
    [SerializeField] Vector2 randomRotationBFromTo;
    [SerializeField] Vector2 randomYOffsetBFromTo;
    [SerializeField] Vector2 randomBCenterLerpFrmoTo;
    [Header("-------------------------------------")]
    [SerializeField] List<TerrainFeature> KlopseC;
    [SerializeField] Vector2Int howManyKlopseCFromTo;
    [SerializeField] Vector2 randomXOffsetCFromTo;
    [SerializeField] Vector2 randomZOffsetCFromTo;
    [SerializeField] Vector2 randomRotationCFromTo;
    [SerializeField] Vector2 randomYOffsetCFromTo;
    [SerializeField] Vector2 randomCCenterLerpFrmoTo;
    [Header("-------------------------------------")]
    [SerializeField] List<TerrainFeature> KlopseD;
    [SerializeField] Vector2Int howManyKlopseDFromTo;
    [SerializeField] Vector2 randomXOffsetDFromTo;
    [SerializeField] Vector2 randomZOffsetDFromTo;
    [SerializeField] Vector2 randomRotationDFromTo;
    [SerializeField] Vector2 randomYOffsetDFromTo;
    [SerializeField] Vector2 randomDCenterLerpFrmoTo;
    [Header("-------------------------------------")]

    [Header("Enemy Mass")]
    [SerializeField] List<EnemyMassLayer> EnemyMassLayers;
    List<int> enemyMassLayersActive;

    public int negativeNeighbors;

    Dictionary<Direction, TerrainFeature> CurrentKlopse;
    List<GameObject> CurrentEnemyMasses;

    [SerializeField] public Burst EnemyMassBurst;
    [SerializeField] Material OuterBurst;
    [SerializeField] float currentOuter;

    List<TerrainFeature> myKlopse;
    Material myKlopseMat;
    GridTile myTile;

    [Header(">>>-------DONT TOUCH---------<<<")]
    public List<ParticleSystem> myRessourceParticles;
    public List<ParticleSystem> myCleanParticles;
    public List<ParticleSystem> myEnemyParticles;



    [SerializeField] float TweenDuration;

    // Start is called before the first frame update
    void Awake()
    {
        myTile = GetComponent<GridTile>();
        CurrentEnemyMasses = new List<GameObject>();
        enemyMassLayersActive = new List<int>();
    }

    public void Setup(GridTile myTile)
    {
        this.myTile = myTile;
        switch (myTile.ressource)
        {
            case Ressource.ressourceA:
                myRessourceParticles = PondRessourceParticleSystems;
                myCleanParticles = PondCleanParticleSystems;
                myEnemyParticles = EnemyMassParticleSystems;

                myKlopse = KlopseA;

                break;
            case Ressource.ressourceB:
                myRessourceParticles = CocoonRessourceParticleSystems;
                myCleanParticles = CocoonCleanParticleSystems;
                myEnemyParticles = EnemyMassParticleSystems;


                myKlopse = KlopseB;

                break;
            case Ressource.ressourceC:
                myRessourceParticles = SpikesRessourceParticleSystems;
                myCleanParticles = SpikesCleanParticleSystems;
                myEnemyParticles = EnemyMassParticleSystems;
                myKlopse = KlopseC;

                break;
            case Ressource.ressourceD:
                myRessourceParticles = MossRessourceParticleSystems;
                myCleanParticles = MossCleanParticleSystems;
                myEnemyParticles = EnemyMassParticleSystems;
                myKlopse = KlopseD;

                break;
        }

        float randomBaumNumber = Random.Range(0f, 1f);
        float randomSpikeNumber = Random.Range(0f, 1f);
        float randomRockNumber = Random.Range(0f, 1f);

        if (myTile.tileInfo.noiseValue >= BäumeNoiseValueFromTo.x && myTile.tileInfo.noiseValue <= BäumeNoiseValueFromTo.y && randomBaumNumber >= propabilityBäume)
        {
            int randomBaumAmount = Random.Range(BäumeAmountFromTo.x, BäumeAmountFromTo.y);
            for (int i = 0; i < randomBaumAmount; i++)
            {
                Direction randomDirection;
                randomDirection = (Direction)Random.Range(0, 7);
                //else
                //{
                //    randomDirection = Direction.C;
                //}
                TerrainFeature newBaum = Instantiate(BäumeFeatures[Random.Range(0, BäumeFeatures.Count)], transform);
                Vector3 goalPosition = transform.position;
                goalPosition += myTile.Points[randomDirection];
                goalPosition = Vector3.Lerp(goalPosition, myTile.Points[Direction.C] + transform.position, Random.Range(CenterLerpBäumeFromTo.x, CenterLerpBäumeFromTo.y));
                newBaum.transform.position = goalPosition;
                newBaum.transform.rotation = Quaternion.Euler(Vector3.up * Random.Range(0, 360));
            }
        }

        if (myTile.tileInfo.noiseValue >= SpikeNoiseValueFromTo.x && myTile.tileInfo.noiseValue <= SpikeNoiseValueFromTo.y && randomSpikeNumber >= propabilitySpike && myTile.ressource != Ressource.ressourceD)
        {
            int randomSpikeAmount = Random.Range(SpikeAmountFromTo.x, SpikeAmountFromTo.y);
            for (int i = 0; i < randomSpikeAmount; i++)
            {
                Direction randomDirection;
                randomDirection = (Direction)Random.Range(0, 7);
                //else
                //{
                //randomDirection = Direction.C;
                //}
                TerrainFeature newSpike = Instantiate(SpikeFeatures[Random.Range(0, SpikeFeatures.Count)], transform);
                Vector3 goalPosition = transform.position;
                goalPosition += myTile.Points[randomDirection];
                goalPosition = Vector3.Lerp(goalPosition, myTile.Points[Direction.C] + transform.position, Random.Range(CenterLerpSpikeFromTo.x, CenterLerpSpikeFromTo.y));
                newSpike.transform.position = goalPosition;
                newSpike.transform.rotation = Quaternion.Euler(Vector3.up * Random.Range(0, 360));
            }
        }

        if (myTile.tileInfo.noiseValue >= RockNoiseValueFromTo.x && myTile.tileInfo.noiseValue <= RockNoiseValueFromTo.y && randomSpikeNumber >= propabilityRock && myTile.ressource != Ressource.ressourceB)
        {
            int randomRockAmount = Random.Range(RockAmountFromTo.x, RockAmountFromTo.y);
            for (int i = 0; i < randomRockAmount; i++)
            {
                Direction randomDirection;
                randomDirection = (Direction)Random.Range(0, 7);
                //else
                //{
                //randomDirection = Direction.C;
                //}
                TerrainFeature newRock = Instantiate(RockFeatures[Random.Range(0, RockFeatures.Count)], transform);
                Vector3 goalPosition = transform.position;
                goalPosition += myTile.Points[randomDirection];
                goalPosition = Vector3.Lerp(goalPosition, myTile.Points[Direction.C] + transform.position, Random.Range(CenterLerpSpikeFromTo.x, CenterLerpSpikeFromTo.y));
                newRock.transform.position = goalPosition;
                newRock.transform.rotation = Quaternion.Euler(Vector3.up * Random.Range(0, 360));
            }
        }

    }

    public void StartRessourceParticles()
    {
        if (myRessourceParticles == null)
        {
            return;
        }
        foreach (ParticleSystem particle in myRessourceParticles)
        {
            particle.Play();
        }
    }

    public void StopRessourceParticles()
    {
        if (myRessourceParticles == null)
        {
            return;
        }
        foreach (ParticleSystem particle in myRessourceParticles)
        {
            particle.Stop();
        }
    }

    public void StartEnemyMassParticles()
    {
        if (myEnemyParticles == null)
        {
            return;
        }
        foreach (ParticleSystem particle in myEnemyParticles)
        {
            particle.Play();
        }
    }

    public void StopEnemyMassParticles()
    {
        if (myRessourceParticles == null)
        {
            return;
        }
        foreach (ParticleSystem particle in myRessourceParticles)
        {
            particle.Stop();
        }
    }

    public void StartCleanParticles()
    {
        if (myCleanParticles == null)
        {
            return;
        }
        foreach (ParticleSystem particle in myCleanParticles)
        {
            particle.Play();
        }
    }

    public void StopCleanParticles()
    {
        if (myCleanParticles == null)
        {
            return;
        }
        foreach (ParticleSystem particle in myCleanParticles)
        {
            particle.Stop();
        }
    }

    private void Start()
    {
        SpawnKlopse();
        switch (myTile.currentGridState.StateValue())
        {
            case <= 0:
                CleanUpKlopse();
                StartEnemyMassParticles();
                break;
            case 1:
                InfestKlopse();
                StartRessourceParticles();
                break;
            case 4:
                CleanUpKlopse();
                StartCleanParticles();
                break;
        }

    }

    public void SpawnKlopse()
    {
        getMyKlopse();
        if (myKlopse.Count > 0)
        {
            int amount = Random.Range(getHowMany().x, getHowMany().y + 1);

            for (int i = 0; i < amount; i++)
            {
                Direction randomDirection;
                if (amount != 1 && i != 0)
                {
                    randomDirection = (Direction)Random.Range(0, 7);
                    while (CurrentKlopse.ContainsKey(randomDirection))
                    {
                        randomDirection = (Direction)Random.Range(0, 7);
                    }
                }
                else
                {
                    randomDirection = Direction.C;
                }


                TerrainFeature newKlops = Instantiate(myKlopse[Random.Range(0, myKlopse.Count)], transform);
                Vector3 goalPosition = transform.position;
                goalPosition += myTile.Points[randomDirection];
                goalPosition = Vector3.Lerp(goalPosition, myTile.Points[Direction.C] + transform.position, Random.Range(getCenterLerp().x, getCenterLerp().y));
                //goalPosition += new Vector3(Random.Range(getXOffset().x, getXOffset().y), 0, Random.Range(getZOffset().x, getZOffset().y));
                //goalPosition -= new Vector3(0, Random.Range(getOffset().x, getOffset().y), 0);
                newKlops.transform.rotation = Quaternion.Euler(0, Random.Range(getRotation().x, getRotation().y), 0);
                //newKlops.GetComponentInChildren<MeshRenderer>().material = myKlopseMat;
                CurrentKlopse.Add(randomDirection, newKlops);
                newKlops.transform.DOMove(goalPosition, TweenDuration).From(goalPosition + Vector3.down * .5f).OnComplete(() => newKlops.transform.DOPunchScale(newKlops.transform.localScale * .25f, TweenDuration / 2f));
                //newKlops.CleanUp();
            }
        }
    }



    private void getMyKlopse()
    {
        myTile = GetComponent<GridTile>();
        switch (myTile.ressource)
        {
            case Ressource.ressourceA:
                myKlopse = KlopseA;
                break;
            case Ressource.ressourceB:
                myKlopse = KlopseB;
                break;
            case Ressource.ressourceC:
                myKlopse = KlopseC;
                break;
            case Ressource.ressourceD:
                myKlopse = KlopseD;
                break;
        }
        CurrentKlopse = new Dictionary<Direction, TerrainFeature>();
    }

    public void InfestKlopse()
    {
        StopEnemyMassParticles();
        StopCleanParticles();
        foreach (KeyValuePair<Direction, TerrainFeature> klops in CurrentKlopse)
        {
            klops.Value.Infest();
        }
        StartRessourceParticles();
    }

    public void CleanUpKlopse()
    {
        try
        {
            StopRessourceParticles();
            StopEnemyMassParticles();
            foreach (KeyValuePair<Direction, TerrainFeature> klops in CurrentKlopse)
            {
                klops.Value.CleanUp();
            }
            StartCleanParticles();
        }
        catch
        {
            Debug.Log("I JUST COULDNT HANDLE DESTORYING MY KLOPSE IM SORRY");
        }
    }

    public void DestroyEnemyMasses()
    {
        foreach(Burst enemyMass in GetComponentsInChildren<Burst>())
        {
            enemyMass.Bursting(); 
            enemyMassLayersActive.Clear();
            StopEnemyMassParticles();
        }



        //try
        //{
        //    enemyMassLayersActive.Clear();
        //    StopEnemyMassParticles();
        //    foreach (GameObject enemyMass in CurrentEnemyMasses)
        //    {
        //        enemyMass.GetComponent<Burst>().Bursting();
        //       //Destroy(enemyMass);
                
        //    }
        //}
        //catch
        //{
        //    Debug.Log("I JUST COULDNT HANDLE DESTORYING MY EnemyMass IM SORRY");
        //}
    }

    public void SpawnEnemyMass()
    {
        try
        {
            foreach (EnemyMassLayer layer in EnemyMassLayers)
            {
                UpdateNegativeNeighbors();
                if (layer.MassNeighborThreshold > negativeNeighbors)
                {
                    continue;
                }
                if (!enemyMassLayersActive.Contains(EnemyMassLayers.IndexOf(layer)))
                {
                    enemyMassLayersActive.Add(EnemyMassLayers.IndexOf(layer));
                }
                else
                {
                    continue;
                }
                int amount = Random.Range(layer.AmountFromTo.x, layer.AmountFromTo.y + 1);
                for (int i = 0; i < amount; i++)
                {
                    GameObject newEnemyMass = Instantiate(layer.MassPrefab, transform);
                    Vector3 goalPosition = transform.position;
                    goalPosition += new Vector3(Random.Range(-.5f, .5f), 0, Random.Range(-.4f, .4f));
                    goalPosition += new Vector3(0, Random.Range(layer.YOffsetFromTo.x, layer.YOffsetFromTo.y), 0);
                    newEnemyMass.transform.rotation = Quaternion.Euler(0, Random.Range(layer.YRotationFromTo.x, layer.YRotationFromTo.y), 0);
                    CurrentEnemyMasses.Add(newEnemyMass);

                    float xScale = Random.Range(layer.XScaleFromTo.x, layer.XScaleFromTo.y);
                    float zScale = Random.Range(layer.ZScaleFromTo.x, layer.ZScaleFromTo.y);
                    float yScale = (xScale + zScale) / 2f;

                    newEnemyMass.transform.localScale = new Vector3(xScale, yScale, zScale);
                    newEnemyMass.transform.DOMove(goalPosition, TweenDuration).From(goalPosition + Vector3.down * .5f).OnComplete(() => newEnemyMass.transform.DOPunchScale(newEnemyMass.transform.localScale * .25f, TweenDuration / 2f));

                }
            }
            StopRessourceParticles();
            StopCleanParticles();
            StartEnemyMassParticles();
        }

        catch
        {

        }
    }

    public void UpdateNegativeNeighbors()
    {
        negativeNeighbors = 0;
        foreach (GridTile neighbor in myTile.myNeighbors)
        {
            if (neighbor.currentGridState.StateValue() < 0)
            {
                negativeNeighbors++;
            }
        }
    }

    private Vector2Int getHowMany()
    {
        switch (myTile.ressource)
        {
            case Ressource.ressourceA:
                return howManyKlopseAFromTo;
            case Ressource.ressourceB:
                return howManyKlopseBFromTo;
            case Ressource.ressourceC:
                return howManyKlopseCFromTo;
            default:
                return howManyKlopseDFromTo;
        }
    }
    private Vector2 getCenterLerp()
    {
        switch (myTile.ressource)
        {
            case Ressource.ressourceA:
                return randomACenterLerpFrmoTo;
            case Ressource.ressourceB:
                return randomBCenterLerpFrmoTo;
            case Ressource.ressourceC:
                return randomCCenterLerpFrmoTo;
            default:
                return randomDCenterLerpFrmoTo;
        }
    }

    private Vector2 getOffset()
    {
        if (myTile == null)
        {
            myTile = GetComponent<GridTile>();
        }

        switch (myTile.ressource)
        {
            case Ressource.ressourceA:
                return randomYOffsetAFromTo;
            case Ressource.ressourceB:
                return randomYOffsetBFromTo;
            case Ressource.ressourceC:
                return randomYOffsetCFromTo;
            default:
                return randomYOffsetDFromTo;
        }
    }

    private Vector2 getRotation()
    {
        switch (myTile.ressource)
        {
            case Ressource.ressourceA:
                return randomRotationAFromTo;
            case Ressource.ressourceB:
                return randomRotationBFromTo;
            case Ressource.ressourceC:
                return randomRotationCFromTo;
            default:
                return randomRotationDFromTo;
        }
    }

    private Vector2 getXOffset()
    {
        switch (myTile.ressource)
        {
            case Ressource.ressourceA:
                return randomXOffsetAFromTo;
            case Ressource.ressourceB:
                return randomXOffsetBFromTo;
            case Ressource.ressourceC:
                return randomXOffsetCFromTo;
            default:
                return randomXOffsetAFromTo;
        }
    }
    private Vector2 getZOffset()
    {
        switch (myTile.ressource)
        {
            case Ressource.ressourceA:
                return randomZOffsetAFromTo;
            case Ressource.ressourceB:
                return randomZOffsetBFromTo;
            case Ressource.ressourceC:
                return randomZOffsetCFromTo;
            default:
                return randomZOffsetDFromTo;
        }
    }


}
