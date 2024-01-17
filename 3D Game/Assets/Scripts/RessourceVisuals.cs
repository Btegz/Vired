using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourceVisuals : MonoBehaviour
{
    [Header("Klopse")]
    [SerializeField] List<TerrainFeature> KlopseA;
    [SerializeField] Vector2Int howManyKlopseAFromTo;
    [SerializeField] Vector2 randomXOffsetAFromTo;
    [SerializeField] Vector2 randomZOffsetAFromTo;
    [SerializeField] Vector2 randomRotationAFromTo;
    [SerializeField] Vector2 randomYOffsetAFromTo;
    [Header("-------------------------------------")]
    [SerializeField] List<TerrainFeature> KlopseB;
    [SerializeField] Vector2Int howManyKlopseBFromTo;
    [SerializeField] Vector2 randomXOffsetBFromTo;
    [SerializeField] Vector2 randomZOffsetBFromTo;
    [SerializeField] Vector2 randomRotationBFromTo;
    [SerializeField] Vector2 randomYOffsetBFromTo;
    [Header("-------------------------------------")]
    [SerializeField] List<TerrainFeature> KlopseC;
    [SerializeField] Vector2Int howManyKlopseCFromTo;
    [SerializeField] Vector2 randomXOffsetCFromTo;
    [SerializeField] Vector2 randomZOffsetCFromTo;
    [SerializeField] Vector2 randomRotationCFromTo;
    [SerializeField] Vector2 randomYOffsetCFromTo;
    [Header("-------------------------------------")]
    [SerializeField] List<TerrainFeature> KlopseD;
    [SerializeField] Vector2Int howManyKlopseDFromTo;
    [SerializeField] Vector2 randomXOffsetDFromTo;
    [SerializeField] Vector2 randomZOffsetDFromTo;
    [SerializeField] Vector2 randomRotationDFromTo;
    [SerializeField] Vector2 randomYOffsetDFromTo;
    [Header("-------------------------------------")]

    [Header("Enemy Mass")]
    [SerializeField] List<GameObject> EnemyMassKlopse;
    [SerializeField] Vector2Int howManyEnemyMassKlopseFromTo;
    [SerializeField] Vector2 EnemyMassrandomRotationFromTo;
    [SerializeField] Vector2 EnemyMassrandomYOffsetFromTo;

    List<TerrainFeature> CurrentKlopse;
    List<GameObject> CurrentEnemyMasses;

    List<TerrainFeature> myKlopse;
    Material myKlopseMat;
    GridTile myTile;



    [SerializeField] float TweenDuration;

    // Start is called before the first frame update
    void Start()
    {
        myTile = GetComponent<GridTile>();
        CurrentEnemyMasses = new List<GameObject>();
        //switch (myTile.ressource)
        //{
        //    case Ressource.ressourceA:
        //        myKlopse = KlopseA;
        //        myKlopseMat = KlopseAMat;
        //        break;
        //    case Ressource.ressourceB:
        //        myKlopse = KlopseB;
        //        myKlopseMat = KlopseBMat;
        //        break;
        //    case Ressource.ressourceC:
        //        myKlopse = KlopseC;
        //        myKlopseMat = KlopseCMat;
        //        break;
        //    case Ressource.ressourceD:
        //        myKlopse = KlopseD;
        //        myKlopseMat = KlopseDMat;
        //        break;
        //}
        //CurrentKlopse = new List<GameObject>();

        if (myTile.currentGridState == GridManager.Instance.gS_Positive)
        {
            SpawnKlopse();
        }
    }

    public void SpawnKlopse()
    {
        getMyKlopse();
        if (myKlopse.Count > 0)
        {
            int amount = Random.Range(getHowMany().x, getHowMany().y + 1);
            Debug.Log($"Amount of Klopse: {amount}");
            for (int i = 0; i < amount; i++)
            {
                TerrainFeature newKlops = Instantiate(myKlopse[Random.Range(0, myKlopse.Count)], transform);
                Vector3 goalPosition = transform.position;
                goalPosition += new Vector3(Random.Range(getXOffset().x, getXOffset().y), 0, Random.Range(getZOffset().x, getZOffset().y));
                goalPosition -= new Vector3(0, Random.Range(getOffset().x, getOffset().y), 0);
                newKlops.transform.rotation = Quaternion.Euler(0, Random.Range(getRotation().x, getRotation().y), 0);
                //newKlops.GetComponentInChildren<MeshRenderer>().material = myKlopseMat;
                CurrentKlopse.Add(newKlops);
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
        CurrentKlopse = new List<TerrainFeature>();
    }

    public void InfestKlopse()
    {
        foreach(TerrainFeature klops in CurrentKlopse)
        {
            klops.Infest();
        }
    }

    public void CleanUpKlopse()
    {
        try
        {

            foreach (TerrainFeature klops in CurrentKlopse)
            {
                klops.CleanUp();
            }

        }
        catch
        {
            Debug.Log("I JUST COULDNT HANDLE DESTORYING MY KLOPSE IM SORRY");
        }
    }

    public void DestroyEnemyMasses()
    {
        try
        {
            foreach(GameObject enemyMass in CurrentEnemyMasses)
            {
                Destroy(enemyMass);
            }
        }
        catch
        {
            Debug.Log("I JUST COULDNT HANDLE DESTORYING MY EnemyMass IM SORRY");
        }
    }

    public void SpawnEnemyMass()
    {
        int amount = Random.Range(howManyEnemyMassKlopseFromTo.x, howManyEnemyMassKlopseFromTo.y + 1);
        for(int i = 0; i < amount; i++)
        {
            GameObject newEnemyMass = Instantiate(EnemyMassKlopse[Random.Range(0, EnemyMassKlopse.Count)],transform);
            Vector3 goalPosition = transform.position;
            goalPosition += new Vector3(Random.Range(-.5f, .5f), 0, Random.Range(-.4f, .4f));
            goalPosition -= new Vector3(0, Random.Range(getOffset().x, getOffset().y), 0);
            newEnemyMass.transform.rotation = Quaternion.Euler(0, Random.Range(getRotation().x, getRotation().y), 0);
            CurrentEnemyMasses.Add(newEnemyMass);
            newEnemyMass.transform.DOMove(goalPosition, TweenDuration).From(goalPosition + Vector3.down * .5f).OnComplete(() => newEnemyMass.transform.DOPunchScale(newEnemyMass.transform.localScale * .25f, TweenDuration / 2f));

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

    private Vector2 getOffset()
    {
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
