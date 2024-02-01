using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Audio; 

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public float moveTime;

    [Header("The Boring Stuff")]
    public Camera cam;
    public AbilityLoadout abilityLoadout; 
    [SerializeField] InputActionReference cancelAbilityInputActionReference;
    public AbilityObjScript abilityObj;
    [HideInInspector] public List<Ability> abilitInventory;
    /*[HideInInspector]*/ public bool abilityActivated = false;
    [HideInInspector] private bool abilityUsable = true;
    [HideInInspector] public bool AbilityLoadoutActive;

    [Header("Player")]
    [SerializeField] public List<GameObject> MovePoints;
    [SerializeField] public List<Player> Players;
    public List<Sprite> PlayerSprites;
    [SerializeField] int movementPointsPerTurn;
    [HideInInspector]public int movementAction = 4;
    [HideInInspector] public int totalSteps;
    public int SkillPoints;
    [HideInInspector] public int extraMovement;
public Player selectedPlayer;
    [HideInInspector] public GridTile target;
    [HideInInspector] public bool move = true;
    [HideInInspector] public Vector3 mouse_pos;
    [HideInInspector] public Vector2Int clickedTile;
    [HideInInspector] public Vector2Int PlayerSpawnPoint;
    [HideInInspector] public Vector2Int collisionPoint;
    [HideInInspector] public Vector2Int playerPosition;


    [Header("Resources")]
    public int RessourceAInventory;
    public int RessourceBInventory;
    public int RessourceCInventory;
    public int RessourceDInventory;
    public RessourceGainEffect ressourceGainEffect;

    [Header("Audio")]
    [SerializeField] AudioData movementSound;
    [SerializeField] AudioData noMovementSound;
    [SerializeField] AudioData switchPlayer;
    [SerializeField] AudioData selectedAbility;
    [SerializeField] AudioData AbilityCanceled;
    [SerializeField] AudioSource hovern;
    [SerializeField] AudioSource enemyHovern;
    [SerializeField] AudioMixerGroup soundEffect;

     public bool abilityLoadoutTutorial = false;
  

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            AbilityLoadoutActive = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {

        
            foreach (Player p in Players)
            {
                p.transform.position = GridManager.Instance.Grid[p.SpawnPoint].transform.position;
                p.CoordinatePosition = GridManager.Instance.Grid[p.SpawnPoint].AxialCoordinate;
            }

            selectedPlayer = Players[0];
        PlayerPrefs.SetFloat("HoverVolume", hovern.volume);




        EventManager.OnEndTurnEvent += resetMovementPoints;
        EventManager.OnAbilityButtonEvent += AbilityClicked;
        EventManager.OnSelectPlayerEvent += PlayerSelect;
        EventManager.OnMoveEvent += startMoveCoroutine;
        EventManager.OnMoveEvent += Audio;
        //playerPosition = PlayerSpawnPoint;
        //selectedPlayer.transform.position = GridManager.Instance.Grid[PlayerSpawnPoint].transform.position;


        //Audio(selectedPlayer);


    }

    public void startMoveCoroutine(Player player)
    {
        StartCoroutine(Move(clickedTile));
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        EventManager.OnEndTurnEvent -= resetMovementPoints;
        EventManager.OnAbilityButtonEvent -= AbilityClicked; 
        EventManager.OnEndTurnEvent -= resetMovementPoints;
        EventManager.OnAbilityButtonEvent -= AbilityClicked;
        EventManager.OnSelectPlayerEvent -= PlayerSelect;
        EventManager.OnMoveEvent -= startMoveCoroutine;
        EventManager.OnMoveEvent -= Audio;
    }

    private void Update()
    {

        AudioManager.Instance.PlayMusic(hovern);
        AudioManager.Instance.PlayMusic(enemyHovern);
        if (!AbilityLoadoutActive)
        {
            // takes mouse positition
            mouse_pos = Mouse.current.position.ReadValue();
            // Searches for the Nieghbors of playerposition
            List<Vector3Int> neighbors = HexGridUtil.CubeNeighbors(HexGridUtil.AxialToCubeCoord(selectedPlayer.CoordinatePosition));

            foreach (Player player in Players)
            {
                if (player.AbilityInventory.Count > 0)
                {
                    // Lose Condition: surrounded by enemies/ no ressources
                    for (int i = 0; i < player.AbilityInventory.Count; i++)
                    {

                        abilityUsable = true;
                        //check ob Ability bezahlbar ist
                        //if (InventoryCheck(i,player))
                        //{
                        //    abilityUsable = true;
                        //    break;
                        //}
                        //else
                        //{
                        //    abilityUsable = false;
                        //}
                    }
                }
            }



            // wenn Ability nicht bezahlbar ist check Nachbarn
            if (abilityUsable == false)
            {
                bool lost = true;
                foreach (Vector3Int neighbor in neighbors)
                {
                    try
                    {
                        //  checks player neighbors for neutral/ positive grids
                        if (GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(neighbor)].currentGridState ==
                            GridManager.Instance.gS_Positive ||
                            GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(neighbor)].currentGridState ==
                            GridManager.Instance.gS_Neutral)
                        {
                            lost = false; break;
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }
                if (lost)
                {
                    SceneManager.LoadScene("GameOverScene");
                    //Debug.Log("Dead");
                }
            }




            // enters if Left Mouse Button was clicked
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                // checks whether movement points are available or if a Ability is activated
                if (MouseCursorPosition(out clickedTile))
                {
                    if (movementAction == 0 && extraMovement == 0)
                    {
                        selectedPlayer.gameObject.transform.GetChild(0).transform.DOComplete();
                        selectedPlayer.gameObject.transform.GetChild(0).transform.DOPunchRotation(new Vector3(10f, 2f), 1f);
                        if (!noMovementSound.audioPlaying)
                        {
                            AudioManager.Instance.PlaySoundAtLocation(noMovementSound, soundEffect, null, true);
                        }
                    }
                    // saves the Grid Tile Location that was clicked


                    // enters if a tile was clicked
                    if (movementAction > 0 || abilityActivated || ((movementAction == 0) && (extraMovement > 0)))
                    {

                        // enters if Players Neighbors contains the clicked Tile
                        if (neighbors.Contains(HexGridUtil.AxialToCubeCoord(clickedTile)) && !abilityActivated && move == true)
                        {
                            if (GridManager.Instance.Grid[clickedTile].currentGridState ==
                                GridManager.Instance.gS_Positive ||
                                GridManager.Instance.Grid[clickedTile].currentGridState ==
                                GridManager.Instance.gS_Neutral ||
                                GridManager.Instance.Grid[clickedTile].currentGridState ==
                                GridManager.Instance.gS_PofI)

                                EventManager.OnMove(selectedPlayer);

                            else
                            {
                                selectedPlayer.gameObject.transform.GetChild(0).transform.DOComplete();
                                selectedPlayer.gameObject.transform.GetChild(0).transform.DOPunchRotation(new Vector3(10f, 2f), .5f);
                                if (!noMovementSound.audioPlaying)
                                {
                                    AudioManager.Instance.PlaySoundAtLocation(noMovementSound, soundEffect, null, true);
                                }
                            }

                        }
                    }
                }




            }
        }

    }


    /// <summary>
    /// Used to determin the GridTile the Mouse is on right now
    /// </summary>
    /// <param name="clickedTile">Grid Tile Coordinate</param>
    /// <returns>true if Mouse Position is on a GridTile</returns>
    private bool MouseCursorPosition(out Vector2Int clickedTile)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        bool overUI = false;
        foreach (RaycastResult reaycastResult in results)
        {
            if (reaycastResult.gameObject.TryGetComponent<RectTransform>(out RectTransform res))
            {
                overUI = true;
                break;
            }
        }
        if (!overUI)
        {
            Ray ray = cam.ScreenPointToRay(mouse_pos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                GridTile tile;

                if (hit.collider.TryGetComponent<GridTile>(out tile))
                {
                    clickedTile = tile.AxialCoordinate;
                    return true;
                }

            }
        }
        clickedTile = Vector2Int.zero;
        return false;

    }





    /// <summary>
    /// Coroutine to Move Player to a Coordinate
    /// </summary>
    /// <param name="moveTo">Coordinate to move player to</param>
    /// <returns></returns>
    IEnumerator Move(Vector2Int moveTo)
    {
        selectedPlayer.CoordinatePosition = moveTo;
        hovern.volume = Mathf.Lerp(hovern.volume, hovern.volume * 1.5f, 0.5f);
            Debug.Log(hovern.volume);

        move = false;

        target = GridManager.Instance.Grid[moveTo];
        if (((movementAction == 0) && (extraMovement > 0)))
        {
            EventManager.OnBonusMovementPointLoss(1);
            extraMovement--;
        }
        else if (movementAction > 0)
        {
            movementAction--; 
            MovePoints[movementAction].GetComponent<MovePointsDoTween>().Away();
        }
        
        selectedPlayer.transform.DOMove(target.transform.position, selectedPlayer.GetComponentInChildren<PlayerVisuals>().moveDuration);
        
        //PlayerManager.Instance.target.currentGridState.PlayerEnters(PlayerManager.Instance.target));

        
        
        
        SaveManager.Instance.totalMovement++;

        yield return new WaitForSeconds(0.35f);
        hovern.volume = PlayerPrefs.GetFloat("HoverVolume");
        move = true;
        yield return null;
    }


    /// <summary>
    /// Used to reset Movementpoints of the Player
    /// </summary>
    public void resetMovementPoints()
    {
        movementAction = movementPointsPerTurn;

        foreach (GameObject mp in MovePoints)
        {
            mp.GetComponent<MovePointsDoTween>().SpriteReset();
        }
        //MovePoints[0].GetComponent<MovePointsDoTween>().SpriteReset();
        //MovePoints[1].GetComponent<MovePointsDoTween>().SpriteReset();
        //MovePoints[2].GetComponent<MovePointsDoTween>().SpriteReset();
        //MovePoints[3].GetComponent<MovePointsDoTween>().SpriteReset();
    }

    public void ChooseAbilityWithIndex(Ability ability, Vector3Int selectedPoint, Vector3Int playerPos)
    {
        Debug.Log("I CHOSE AN ABILITY");
        Ability chosenAbility = ability;
        AbilityObjScript AbilityPreview = Instantiate(abilityObj);
        AbilityPreview.ShowMesh(chosenAbility, selectedPoint, playerPos, selectedPlayer);
    }

    public void AbilityClicked(Ability ability, AbilityButton button)
    {
        if (button.currentState == ButtonState.inMainScene)
        {
           
            
            AbilityClicked(ability);
            
        }
    }

    /// <summary>
    /// Called in OnClick of an AbilityButton.
    /// determins whether player has enough Ressources for the Ability
    /// </summary>
    /// <param name="index">index of the Ability Clicked</param>
    public void AbilityClicked(Ability ability)
    {

        Debug.Log(ability);
        // sets the "abilityAcitvated" bool to true, so player cant move anymore after choosing a Ability
        if (abilityActivated == false && InventoryCheck(ability, selectedPlayer))
        {
            move = false;
           // AudioManager.Instance.PlaySoundAtLocation(selectedAbility, soundEffect, null);

            cancelAbilityInputActionReference.action.performed += CancelAbilityChoice;
            abilityActivated = true;
            EventManager.OnAbilityCastEvent += AbilityCasted;
            //StartCoroutine(ChooseAbilityLocation(ability));
            ChooseAbilityWithIndex(ability, HexGridUtil.CubeAdd(HexGridUtil.AxialToCubeCoord(selectedPlayer.CoordinatePosition), HexGridUtil.cubeDirectionVectors[0]),
                            HexGridUtil.AxialToCubeCoord(selectedPlayer.CoordinatePosition));

            //SHOW THE ABILITY INDICATOR


            //Vector3 indicatorPosition = new Vector3(0, 0, 0);
            //Quaternion indicatorRotation = Quaternion.Euler(0, 30, 0);
            //indicatorPrefabClone = Instantiate(indicatorPrefab, selectedPlayer.transform.position, indicatorRotation);
        }
    }

    public bool InventoryCheck(Ability ability, Player player)
    {
        try
        {
            //saves the cost of the chosen Ability
            Ressource resCost = ability.MyCostRessource;
            //switches over the different ressources and checks whether player has anough ressources of fitting Type
            //the function returns if Player does not have enough Ressources for the Ability
            switch (resCost)
            {
                case Ressource.ressourceA:
                    if (ability.MyCostAmount > RessourceAInventory)
                    {
                        return false;
                    }

                    break;


                case Ressource.ressourceB:
                    if (ability.MyCostAmount > RessourceBInventory)
                    {
                        return false;
                    }

                    break;

                case Ressource.ressourceC:
                    if (ability.MyCostAmount > RessourceCInventory)
                    {
                        return false;
                    }

                    break;

                case Ressource.ressourceD:
                    if (ability.MyCostAmount > RessourceDInventory)
                    {
                        return false;
                    }

                    break;
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Coroutine to Wait for Player to choose the Abilities Location after it was chosen
    /// </summary>
    /// <param name="AbilityIndex"></param>
    /// <returns></returns>
    //public IEnumerator ChooseAbilityLocation(Ability ability)
    //{
    //    cancelAbilityInputActionReference.action.performed += CancelAbilityChoice;

    //    // collects player Neighbors as viable tiles
    //    List<Vector3Int> neighbors = HexGridUtil.CubeNeighbors(HexGridUtil.AxialToCubeCoord(selectedPlayer.CoordinatePosition));

    //    //selectedPoint = HexGridUtil.AxialToCubeCoord(selectedPlayer.coordinatePosition);

    //    Vector2Int clickedTile;

    //    while (abilityActivated)
    //    {
    //        if (Mouse.current.rightButton.wasPressedThisFrame)
    //        {
    //            CancelAbilityChoice();
    //            Destroy(indicatorPrefabClone);
    //        }

    //        // enters if Left Mouse Button was clicked
    //        if (Mouse.current.leftButton.wasPressedThisFrame)
    //        {
    //            if (MouseCursorPosition(out clickedTile))
    //            {
    //                if (neighbors.Contains(HexGridUtil.AxialToCubeCoord(clickedTile)))
    //                {
    //                    //ChooseAbilityWithIndex(ability, HexGridUtil.AxialToCubeCoord(clickedTile),HexGridUtil.AxialToCubeCoord(selectedPlayer.CoordinatePosition));
    //                    break;
    //                }

    //                yield return null;
    //            }
    //        }

    //        yield return null;
    //    }

    //    yield return null;
    //}


    public void AbilityCasted(Player player)
    {
        StartCoroutine(AbilityCastCoroutine());
        //EventManager.OnAbilityCastEvent -= AbilityCasted;
        //cancelAbilityInputActionReference.action.performed -= CancelAbilityChoice; 
        //abilityActivated = false;
        //Destroy(indicatorPrefabClone);
        
    }

    public IEnumerator AbilityCastCoroutine()
    {
        EventManager.OnAbilityCastEvent -= AbilityCasted;
        cancelAbilityInputActionReference.action.performed -= CancelAbilityChoice;
        yield return new WaitForEndOfFrame(); 
        move = true;
        abilityActivated = false;
        yield return null;
    }

    public void CancelAbilityChoice(InputAction.CallbackContext actionCallBackContext)
    {
        abilityActivated = false;
        move = true;
        EventManager.OnAbilityCastEvent -= AbilityCasted;
        cancelAbilityInputActionReference.action.performed -= CancelAbilityChoice;
        AudioManager.Instance.PlaySoundAtLocation(AbilityCanceled, soundEffect, null, true);
    }

    public void CancelAbilityChoice()
    {
        abilityActivated = false;
        move = true;
        EventManager.OnAbilityCastEvent -= AbilityCasted;
        cancelAbilityInputActionReference.action.performed -= CancelAbilityChoice;
        AudioManager.Instance.PlaySoundAtLocation(AbilityCanceled, soundEffect, null, true);

    }

    public List<Vector2Int> PlayerPositions()
    {
        List<Vector2Int> positions = new List<Vector2Int>();
        foreach (Player p in Players)
        {
            positions.Add(p.CoordinatePosition);
        }
        return positions;
    }

    public void PlayerSelect(Player player)
    {
        
        CameraRotation.Instance.MainCam = true;
       
        PlayerManager.Instance.selectedPlayer = player/*PlayerManager.Instance.Players[(int)keyPressed]*/;
        // EventManager.OnSelectPlayer(selectedPlayer);


        // CameraRotation.Instance.Playercam.LookAt = PlayerManager.Instance.selectedPlayer.transform;
        //CameraRotation.Instance.Playercam.Follow = PlayerManager.Instance.selectedPlayer.transform;
        // CameraRotation.Instance.SwitchToPlayer();

        if (player == Players[0])
        {
            AudioManager.Instance.PlaySoundAtLocation(switchPlayer, soundEffect, "Player1", true);

        }

        else if (player == Players[1])
        {
            AudioManager.Instance.PlaySoundAtLocation(switchPlayer, soundEffect, "Player2", true);
        }

        else if (player == Players[2])
        {

            AudioManager.Instance.PlaySoundAtLocation(switchPlayer, soundEffect, "Player3", true);
        }

        else return;


    }
    public void Audio(Player player)
    {
        if(!movementSound.audioPlaying)
        {
        AudioManager.Instance.PlaySoundAtLocation(movementSound, soundEffect, null, true);

        }
    }




}