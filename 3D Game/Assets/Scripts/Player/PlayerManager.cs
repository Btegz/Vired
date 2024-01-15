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


    [Header("The Boring Stuff")]
    public Camera cam;
    public AbilityLoadout abilityLoadout; 
    [SerializeField] InputActionReference cancelAbilityInputActionReference;
    public AbilityObjScript abilityObj;
    [HideInInspector] public List<Ability> abilitInventory;
    [HideInInspector] public bool abilityActivated = false;
    [HideInInspector] private bool abilityUsable = true;
    [HideInInspector] public bool AbilityLoadoutActive;

    [Header("Player")]
    [SerializeField] public List<GameObject> MovePoints;
    [SerializeField] public List<Player> Players;
    public List<Sprite> PlayerSprites;
    [SerializeField] int movementPointsPerTurn;
    [HideInInspector]public int movementAction = 4;
    public int SkillPoints;
    [HideInInspector] public int extraMovement;
    [HideInInspector] public Player selectedPlayer;
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
    [SerializeField] AudioMixerGroup soundEffect;

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
                        AudioManager.Instance.PlaySoundAtLocation(noMovementSound, soundEffect);
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
                                AudioManager.Instance.PlaySoundAtLocation(noMovementSound, soundEffect);
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

        PlayerManager.Instance.target.currentGridState.PlayerEnters(PlayerManager.Instance.target);

        selectedPlayer.transform.DOMove(target.transform.position, .25f);

        

        selectedPlayer.CoordinatePosition = moveTo;

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
        AbilityPreview.ShowMesh(chosenAbility, selectedPoint, playerPos);
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
        // sets the "abilityAcitvated" bool to true, so player cant move anymore after choosing a Ability
        if (abilityActivated == false && InventoryCheck(ability, selectedPlayer))
        {
            move = false;
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
        move = true;
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


    public void AbilityCasted()
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
        abilityActivated = false;

        yield return new WaitForEndOfFrame();
    }

    public void CancelAbilityChoice(InputAction.CallbackContext actionCallBackContext)
    {
        abilityActivated = false;
        EventManager.OnAbilityCastEvent -= AbilityCasted;
        cancelAbilityInputActionReference.action.performed -= CancelAbilityChoice;
    }

    public void CancelAbilityChoice()
    {
        abilityActivated = false;
        EventManager.OnAbilityCastEvent -= AbilityCasted;
        cancelAbilityInputActionReference.action.performed -= CancelAbilityChoice;
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
        AudioManager.Instance.PlaySoundAtLocation(switchPlayer, soundEffect);
        PlayerManager.Instance.selectedPlayer = player/*PlayerManager.Instance.Players[(int)keyPressed]*/;
        // EventManager.OnSelectPlayer(selectedPlayer);


       // CameraRotation.Instance.Playercam.LookAt = PlayerManager.Instance.selectedPlayer.transform;
        //CameraRotation.Instance.Playercam.Follow = PlayerManager.Instance.selectedPlayer.transform;
        // CameraRotation.Instance.SwitchToPlayer();



    }
    public void Audio(Player player)
    {
        AudioManager.Instance.PlaySoundAtLocation(movementSound, soundEffect);
    }

   
}