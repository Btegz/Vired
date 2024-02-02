using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] AbilityLoadout abilityLoadout;
    [SerializeField] GameObject PlayerArea1;
    [SerializeField] GameObject PlayerArea2;
    [SerializeField] GameObject PlayerButton;
    [SerializeField] GameObject PlayerButton1;
    [SerializeField] GameObject PlayerButton2;
    [SerializeField] GameObject PlayerButton3;
    [SerializeField] GameObject Settings;
    [SerializeField] GameObject Info;
    [SerializeField] GameObject WorldButton;
    [SerializeField] GameObject TopdownButton;
    [SerializeField] GameObject NextTurn;
    [SerializeField] GameObject AbilityButtons;
    [SerializeField] GameObject ResourceObject;
    [SerializeField] GameObject Wrench;
    [SerializeField] GameObject Chunk;
    [SerializeField] GameObject Battery;
    [SerializeField] GameObject GridTilePreview;
    [SerializeField] GameObject AbilityUpgrade;
    public UpgradeHexGrid Upgrade;


    [Header("TutorialTexts")]
    [SerializeField] GameObject DroneImage;
    [SerializeField] GameObject BottomText;
    [SerializeField] GameObject BottomBox;

    [SerializeField] GameObject NextButtonBlock;
    [SerializeField] GameObject AbilityBlock;
    [SerializeField] public GameObject ConfirmBlock;

    public GameObject PlayerButtonHighlight;
    public GameObject PlayerButtonText;
    public GameObject PlayerButtonTextText;

    public GameObject chooseAbilityText;
    public GameObject confirmText;
    public GridManager gridManager;


    public CinemachineTrack dolly;
    public List<GameObject> HighlightObject;
    public List<GameObject> ResourceHighlight;
    public bool tutorial;
   // public Button ButtonHighlight;
    public IEnumerator enabled;
    public bool enabledIsRunning;
    public GameObject InventoryHighlight;
    List<Vector2Int> neighbors;
    List<GameObject> Neighbor;

    public CinemachinePathBase path;


    private Vector3 DroneStart;
    public static TutorialManager Instance;
    public List<Vector2Int> Preview;
    public List<Vector2Int> EnemyTut;
    public bool enemySpawnt = false;

    public PlayableDirector PlayerButtonTrack;
    public PlayableDirector UpgradeTrack;
    public PlayableDirector EnemyTrack;
    public PlayableDirector TutorialEndTrack;
    public GameObject BlackScreen;
    public GameObject UpgradeGrid;
    public Sprite damageHex; 

    private GridTile neighborTile;
    public Ability ability;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        
    }




    void OnEnable()
    {
        //BottomText.GetComponent<TextMeshProUGUI>().text = chooseAbilityText;

        tutorial = true;
        PlayerArea1.SetActive(false);
        PlayerArea2.SetActive(false);
        abilityLoadout.GetComponentInChildren<HorizontalLayoutGroup>().childAlignment = TextAnchor.MiddleCenter;
        


        /* AbilityButtons.SetActive(false);
         ResourceObject.SetActive(false);
         Wrench.SetActive(false);
         NextTurn.SetActive(false);
         */
        BottomBox.SetActive(true);
        BottomText.SetActive(true);
        DroneImage.SetActive(true);

        enabled = Enabling(HighlightObject);
        StartCoroutine(enabled);
        tutorial = false;


    }

    private void Start()
    {
        PlayerManager.Instance.selectedPlayer = PlayerManager.Instance.Players[0];

    }
    private void Update()
    {
        abilityLoadout.amountToChoose = 1;

        CameraRotation.Instance.AbilityUpgradeCam.Follow = PlayerManager.Instance.Players[0].transform;


        while (abilityLoadout.amountToChoose > 1)
        {
            abilityLoadout.amountToChoose = 1;
            break;
        }

        if (CameraRotation.Instance.AbilityLoadoutCam.Priority == 3)
        {
            PlayerManager.Instance.abilityLoadoutTutorial = true;
        }

        else
        {
            PlayerManager.Instance.abilityLoadoutTutorial = false;
        }

        //follow Objekt 
        // 

        if (neighborTile!= null && neighborTile.GetComponentInChildren<Enemy>() == null && enemySpawnt == true)
        { 
            TutorialEndTrack.Play();
            enemySpawnt = false;
            NextButtonBlock.SetActive(false);

        }

        if (PlayerManager.Instance.RessourceAInventory < 2 || PlayerManager.Instance.RessourceBInventory <2  || PlayerManager.Instance.RessourceCInventory <2 || PlayerManager.Instance.RessourceDInventory <2 )
        {
            PlayerManager.Instance.RessourceAInventory = 4;
            PlayerManager.Instance.RessourceBInventory = 4;
            PlayerManager.Instance.RessourceCInventory = 4;
            PlayerManager.Instance.RessourceDInventory = 4;
        }

        
        

            

    }

    public IEnumerator Enabling(List<GameObject> EnablingObjects)
    {
        while (true)
        {


            enabledIsRunning = true;
            for (int i = 0; i < EnablingObjects.Count; i++)
            {
                EnablingObjects[i].SetActive(true);
                yield return new WaitForSeconds(2f);
                EnablingObjects[i].SetActive(false);
                Debug.Log("enabled");
            }

            yield return null;
        }
    }



    public void Deactivate()
    {

        BottomText.SetActive(false);
        BottomBox.SetActive(false);

        confirmText.SetActive(false);
        PlayerButtonTrack.Play();
        StartCoroutine(StartGame());



        //PlsMove();

        /*   CameraRotation.Instance.AbilityLoadoutCam.GetComponent<CinemachineDollyCart>().m_Path = path;
            CameraRotation.Instance.AbilityLoadoutCam.GetComponent<CinemachineDollyCart>().m_Speed = 15;
           Chunk.GetComponent<CinemachineDollyCart>().m_Speed = 15;


               CameraRotation.Instance.AbilityLoadoutCam.transform.rotation = new Quaternion(0, 0, 0, 0);

           if (CameraRotation.Instance.AbilityLoadoutCam.GetComponent<CinemachineDollyCart>().m_Position >6)
           {
               Debug.Log("hi");
               CameraRotation.Instance.AbilityLoadoutCam.GetComponent<CinemachineDollyCart>().m_Path = null;

           }

           if (Chunk.GetComponent<CinemachineDollyCart>().m_Position >= 248)
           {
               Chunk.SetActive(false);
           }


           if (CameraRotation.Instance.AbilityLoadoutCam.GetComponent<CinemachineTrackedDolly>().m_PathPosition == 8)
           {
               CameraRotation.Instance.AbilityLoadoutCam.GetComponent<CinemachineTrackedDolly>().m_Path = null; 
          */




    }

  
    

    public IEnumerator StartGame()
    {
        enabledIsRunning = true;
        PlayerManager.Instance.abilityActivated = true;
        CameraRotation.Instance.dontMove = true;
        yield return new WaitForSeconds(0.5f);

        PlayerButton1.GetComponent<Image>().DOFade(1, 1f);

        // PlayerButton1.GetComponent<RectTransform>().DOScale(1.2f, 1f).OnComplete(() => PlayerButton1.GetComponent<RectTransform>().DOScale(1f, 1f));

        PlayerButton1.GetComponentInChildren<TextMeshProUGUI>().DOFade(1, 1f);

        PlayerButtonHighlight.GetComponent<RectTransform>().DOScale(1.3f, 1f).OnComplete(() => PlayerButtonHighlight.GetComponent<RectTransform>().DOScale(1f, 1f));

        PlayerButtonHighlight.GetComponent<Image>().DOFade(1f, 1f);


        yield return new WaitForSeconds(1f);



        yield return null;
    }

    public void PlsMove()
    {
        if (CameraRotation.Instance.Worldcam.Priority == 3)
        {

            neighbors = HexGridUtil.AxialNeighbors(PlayerManager.Instance.Players[0].CoordinatePosition);

            foreach (Vector2Int neighbor in neighbors)
            {
                Preview.Add(neighbor);

                foreach (KeyValuePair<Vector2Int, GridTile> kvp in GridManager.Instance.Grid)
                {
                    if (kvp.Key == neighbor)
                    {
                        tutorial = false;
                        Instantiate(GridTilePreview, kvp.Value.transform);
                    }
                }
            }

            /*  if (PlayerManager.Instance.move)
              {
                  for (int i = 0; i < Preview.Count; i++)
                  {
                      Destroy(Preview[i]);
                  }

              }*/
        }
    }

    public void NOMove()
    {
        foreach (TileHighlight tileHighlightInstance in gridManager.GetComponentsInChildren<TileHighlight>())
        {
            Debug.Log("Destroy");
            Destroy(tileHighlightInstance.gameObject);
        }
        PlayerManager.Instance.movementAction = 1;
        tutorial = true;
        PlayerManager.Instance.abilityActivated = false;


    }
    public IEnumerator Flicker(GameObject Object)
    {
        while (true)
        {
            Object.GetComponent<Image>().DOComplete();
            Object.GetComponent<Image>().DOFade(Random.RandomRange(0.3f, 0.6f), 0.2f).OnComplete(() => Object.GetComponent<Image>().DOFade(0.9f, 0.4f));

            Object.GetComponent<TextMeshProUGUI>().DOComplete();
            Object.GetComponentInChildren<TextMeshProUGUI>().DOFade(Random.RandomRange(0.3f, 0.6f), 0.2f).OnComplete(() => Object.GetComponentInChildren<TextMeshProUGUI>().DOFade(0.9f, 0.4f));
            yield return new WaitForSeconds(0.5f);
        }

    }



    public void EnemyTutorial()
    {
        AbilityUpgrade.SetActive(false);
        PlayerManager.Instance.abilityActivated = false;
        CameraRotation.Instance.dontMove = true;

        neighbors = HexGridUtil.AxialNeighbors(PlayerManager.Instance.Players[0].CoordinatePosition);

        foreach (Vector2Int neighbor in neighbors)
        {


            foreach (KeyValuePair<Vector2Int, GridTile> kvp in GridManager.Instance.Grid)
            {
                if (kvp.Key == neighbor)
                {
                    EnemyTut.Add(neighbor);
                    neighborTile = kvp.Value;
                }
            }
        }

        Enemy enemy = Instantiate(GridManager.Instance.StartEnemyPrefabs[Random.Range(0, GridManager.Instance.StartEnemyPrefabs.Count)], HexGridUtil.AxialToCubeCoord(EnemyTut[Random.RandomRange(0, EnemyTut.Count)]), Quaternion.identity);
        enemy.Setup(neighborTile);
        enemySpawnt = true;
        AbilityBlock.SetActive(false);
       

    }


    public void TutorialEnd()
    {
        if( TutorialManager.Instance != null)
        {
            BlackScreen.SetActive(true);
            
            BlackScreen.GetComponent<Image>().DOFade(1f, 1f).OnComplete(() => SceneManager.LoadScene("MainScene"));
            TutorialManager.Instance.tutorial = true;
                   
            }
           
        


    }

}
