using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourceVisuals : MonoBehaviour
{
    [Header("Klopse")]
    [SerializeField] List<GameObject> KlopseA;
    [SerializeField] Material KlopseAMat;
    [SerializeField] Vector2Int howManyKlopseAFromTo;
    [SerializeField] Vector2 randomRotationAFromTo;
    [SerializeField] Vector2 randomYOffsetAFromTo;
    [Header("-------------------------------------")]
    [SerializeField] List<GameObject> KlopseB;
    [SerializeField] Material KlopseBMat;
    [SerializeField] Vector2Int howManyKlopseBFromTo;
    [SerializeField] Vector2 randomRotationBFromTo;
    [SerializeField] Vector2 randomYOffsetBFromTo;
    [Header("-------------------------------------")]
    [SerializeField] List<GameObject> KlopseC;
    [SerializeField] Material KlopseCMat;
    [SerializeField] Vector2Int howManyKlopseCFromTo;
    [SerializeField] Vector2 randomRotationCFromTo;
    [SerializeField] Vector2 randomYOffsetCFromTo;
    [Header("-------------------------------------")]
    [SerializeField] List<GameObject> KlopseD;
    [SerializeField] Material KlopseDMat;
    [SerializeField] Vector2Int howManyKlopseDFromTo;
    [SerializeField] Vector2 randomRotationDFromTo;
    [SerializeField] Vector2 randomYOffsetDFromTo;
    [Header("-------------------------------------")]

    [Header("Enemy Mass")]
    [SerializeField] List<GameObject> EnemyMassKlopse;
    [SerializeField] Vector2Int howManyEnemyMassKlopseFromTo;
    [SerializeField] Vector2 EnemyMassrandomRotationFromTo;
    [SerializeField] Vector2 EnemyMassrandomYOffsetFromTo;

    List<GameObject> CurrentKlopse;

    List<GameObject> myKlopse;
    Material myKlopseMat;
    GridTile myTile;



    [SerializeField] float TweenDuration;

    // Start is called before the first frame update
    void Start()
    {
        myTile = GetComponent<GridTile>();
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
                GameObject newKlops = Instantiate(myKlopse[Random.Range(0, myKlopse.Count)], transform);
                Vector3 goalPosition = transform.position;
                goalPosition += new Vector3(Random.Range(-.5f, .5f), 0, Random.Range(-.4f, .4f));
                goalPosition -= new Vector3(0, Random.Range(getOffset().x, getOffset().y), 0);
                newKlops.transform.rotation = Quaternion.Euler(0, Random.Range(getRotation().x, getRotation().y), 0);
                newKlops.GetComponentInChildren<MeshRenderer>().material = myKlopseMat;
                CurrentKlopse.Add(newKlops);
                newKlops.transform.DOMove(goalPosition, TweenDuration).From(goalPosition + Vector3.down * .5f).OnComplete(() => newKlops.transform.DOPunchScale(newKlops.transform.localScale * .25f, TweenDuration / 2f));
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
                myKlopseMat = KlopseAMat;
                break;
            case Ressource.ressourceB:
                myKlopse = KlopseB;
                myKlopseMat = KlopseBMat;
                break;
            case Ressource.ressourceC:
                myKlopse = KlopseC;
                myKlopseMat = KlopseCMat;
                break;
            case Ressource.ressourceD:
                myKlopse = KlopseD;
                myKlopseMat = KlopseDMat;
                break;
        }
        CurrentKlopse = new List<GameObject>();
    }

    public void DestroyKlopse()
    {
        try
        {

            foreach (GameObject klops in CurrentKlopse)
            {
                Destroy(klops);
            }

        }
        catch
        {
            Debug.Log("I JUST COULDNT HANDLE DESTORYING MY KLOPSE IM SORRY");
        }
    }

    public void SpawnEnemyMass()
    {

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
}
