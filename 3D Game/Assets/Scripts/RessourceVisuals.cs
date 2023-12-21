using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourceVisuals : MonoBehaviour
{
    [SerializeField] List<GameObject> KlopseA;
    [SerializeField] List<GameObject> KlopseB;
    [SerializeField] List<GameObject> KlopseC;
    [SerializeField] List<GameObject> KlopseD;

    [SerializeField] Material KlopseAMat;
    [SerializeField] Material KlopseBMat;
    [SerializeField] Material KlopseCMat;
    [SerializeField] Material KlopseDMat;

    

    List<GameObject> CurrentKlopse;

    List<GameObject> myKlopse;
    Material myKlopseMat;
    GridTile myTile;

    [SerializeField] Vector2Int howManyKlopseFromTo;
    [SerializeField] Vector2 randomRotationFromTo;
    [SerializeField] Vector2 randomYOffsetFromTo;

    [SerializeField] float TweenDuration;

    // Start is called before the first frame update
    void Start()
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

        if(myTile.currentGridState == GridManager.Instance.gS_Positive)
        {
            SpawnKlopse();
        }
    }

    public void SpawnKlopse()
    {
        int amount = Random.Range(howManyKlopseFromTo.x, howManyKlopseFromTo.y + 1);
        for (int i = 0; i < amount; i++)
        {
            GameObject newKlops = Instantiate(myKlopse[Random.Range(0, myKlopse.Count)],transform);
            Vector3 goalPosition = transform.position;
            goalPosition  += new Vector3(Random.Range(-.6f, .6f), 0, Random.Range(-.4f, .4f));
            goalPosition -= new Vector3(0,Random.Range(randomYOffsetFromTo.x, randomYOffsetFromTo.y),0);
            newKlops.transform.rotation = Quaternion.Euler(0,Random.Range(randomRotationFromTo.x,randomRotationFromTo.y), 0);
            newKlops.GetComponent<MeshRenderer>().material = myKlopseMat;
            CurrentKlopse.Add(newKlops);
            newKlops.transform.DOMove(goalPosition, TweenDuration).From(goalPosition + Vector3.down*.5f).OnComplete(()=> newKlops.transform.DOPunchScale(newKlops.transform.localScale*.25f,TweenDuration/2f));
        }
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
}
