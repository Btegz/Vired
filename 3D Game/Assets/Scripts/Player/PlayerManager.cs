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

/// <summary>
/// Singleton for the Playermanager
/// This class manages player behaviour, selection and movement, through determination of mouse position and the use of abilities  
/// </summary>
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    

    [Header("Ability")]
    [SerializeField] public InputActionReference cancelAbilityInputActionReference;
    public Camera cam;
    public AbilityLoadout abilityLoadout;
    public AbilityObjScript abilityObj;
    public bool abilityActivated = false;
    public bool abilityLoadoutTutorial = false;
    private bool abilityUsable = true;
    [HideInInspector] public List<Ability> abilitInventory;
    [HideInInspector] public bool AbilityLoadoutActive;

    [Header("Player")]
    [SerializeField] public List<GameObject> MovePoints;
    [SerializeField] public List<Player> Players;
    [SerializeField] int movementPointsPerTurn;
    public List<Sprite> PlayerSprites;
    public int SkillPoints;
    public Player selectedPlayer;
    public float moveTime;
    [HideInInspector] public int extraMovement;
    [HideInInspector] public int movementAction = 4;
    [HideInInspector] public int totalSteps;
    [SerializeField] public GridTile target;
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

    /// <summary>
    /// create singleton
    /// </summary>
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


    /// <summary>
    /// First player gets selected, Methods are being added to events
    /// </summary>
    void Start()
    {
        foreach (Player p in Players)
        {
            p.transform.position = GridManager.Instance.Grid[p.SpawnPoint].transform.position;
            p.CoordinatePosition = GridManager.Instance.Grid[p.SpawnPoint].AxialCoordinate;
        }
        selectedPlayer = Players[0];
        PlayerPrefs.SetFloat("HoverVolume", hovern.volume);
        AddMethods();
    }




    /// <summary>
    /// Checks all conditions for player movement, results in movement or refusal of it 
    /// </summary>

    private void Update()
    {
        AudioManager.Instance.PlayMusic(hovern);
        AudioManager.Instance.PlayMusic(enemyHovern);
        if (!AbilityLoadoutActive)
        {
            // takes mouse positition
            mouse_pos = Mouse.current.position.ReadValue();

            // Searches for the Neighbors of playerposition
            List<Vector3Int> neighbors = HexGridUtil.CubeNeighbors(HexGridUtil.AxialToCubeCoord(selectedPlayer.CoordinatePosition));
            foreach (Player player in Players)
            {
                if (player.AbilityInventory.Count > 0)
                {
                    // Lose Condition: surrounded by enemies/ no ressources
                    for (int i = 0; i < player.AbilityInventory.Count; i++)
                    {
                        abilityUsable = true;
                    }
                }
            }
            
            // enters if Left Mouse Button was clicked
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                // checks whether movement points are available or if a Ability is activated
                if (MouseCursorPosition(out clickedTile))
                {
                    //No movements point left will result in refusal of movement
                    if (movementAction == 0 && extraMovement == 0)
                    {
                        selectedPlayer.gameObject.transform.GetChild(0).transform.DOComplete();
                        selectedPlayer.gameObject.transform.GetChild(0).transform.DOPunchRotation(new Vector3(10f, 2f), 1f);
                        if (!noMovementSound.audioPlaying)
                        {
                            AudioManager.Instance.PlaySoundAtLocation(noMovementSound, soundEffect, null, true);
                        }
                    }

                    // enters if a tile was clicked; Checks if movement points are available or an ability is active)
                    if (movementAction > 0 || abilityActivated || ((movementAction == 0) && (extraMovement > 0)))
                    {
                        // checks if Players Neighbors contains the clicked Tile
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
    }

    /// <summary>
    /// Selects and shows chosen ability
    /// </summary>
    /// <param name="ability">current ability</param>
    /// <param name="selectedPoint">spawn point/ coords</param>
    /// <param name="playerPos">selected player coords</param>
    public void ChooseAbilityWithIndex(Ability ability, Vector3Int selectedPoint, Vector3Int playerPos)
    {
        Ability chosenAbility = ability;
        AbilityObjScript AbilityPreview = Instantiate(abilityObj);
        AbilityPreview.ShowMesh(chosenAbility, selectedPoint, playerPos, selectedPlayer);
    }

    /// <summary>
    /// Checks if we are currently in the crafting, main or loadout scene
    /// </summary>
    /// <param name="ability">current ability</param>
    /// <param name="button">button of specific ability</param>
    public void AbilityClicked(Ability ability, AbilityButton button)
    {
        if (button.currentState == ButtonState.inMainScene)
        {
            AbilityClicked(ability);
        }
    }

    /// <summary>
    /// Called in OnClick of an AbilityButton
    /// checks if an ability is already active and player has enough resources
    /// </summary>
    /// <param name="ability">current ability</param>
    public void AbilityClicked(Ability ability)
    {
        if (abilityActivated == false && InventoryCheck(ability, selectedPlayer))
        {
            move = false;
            cancelAbilityInputActionReference.action.performed += CancelAbilityChoice;
            abilityActivated = true;
            EventManager.OnAbilityCastEvent += AbilityCasted;
            ChooseAbilityWithIndex(ability, HexGridUtil.CubeAdd(HexGridUtil.AxialToCubeCoord(selectedPlayer.CoordinatePosition), HexGridUtil.cubeDirectionVectors[0]),
                            HexGridUtil.AxialToCubeCoord(selectedPlayer.CoordinatePosition));
        }
    }

    /// <summary>
    /// Checks if the player has enough resources to pay for the chosen ability 
    /// </summary>
    /// <param name="ability">current ability</param>
    /// <param name="player">currently selected player</param>
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
    /// Starts the coroutine to cast the selected ability
    /// </summary>
    /// <param name="player">currently selected player</param>
    public void AbilityCasted(Player player)
    {
        StartCoroutine(AbilityCastCoroutine());
    }

    /// <summary>
    /// Sets new conditions after the ability was used
    /// </summary>
    public IEnumerator AbilityCastCoroutine()
    {
        EventManager.OnAbilityCastEvent -= AbilityCasted;
        cancelAbilityInputActionReference.action.performed -= CancelAbilityChoice;
        yield return new WaitForEndOfFrame();
        move = true;
        abilityActivated = false;
        yield return null;
    }

    /// <summary>
    /// Sets new conditions after the ability was canceled
    /// </summary>
    public void CancelAbilityChoice(InputAction.CallbackContext actionCallBackContext)
    {
        abilityActivated = false;
        move = true;
        EventManager.OnAbilityCastEvent -= AbilityCasted;
        cancelAbilityInputActionReference.action.performed -= CancelAbilityChoice;
        AudioManager.Instance.PlaySoundAtLocation(AbilityCanceled, soundEffect, null, true);
    }

    /// <summary>
    /// Sets new conditions after the ability was canceled
    /// </summary>
    public void CancelAbilityChoice()
    {
        abilityActivated = false;
        move = true;
        EventManager.OnAbilityCastEvent -= AbilityCasted;
        cancelAbilityInputActionReference.action.performed -= CancelAbilityChoice;
        AudioManager.Instance.PlaySoundAtLocation(AbilityCanceled, soundEffect, null, true);
    }


    /// <summary>
    /// Defines player position
    /// </summary>
    public List<Vector2Int> PlayerPositions()
    {
        List<Vector2Int> positions = new List<Vector2Int>();
        foreach (Player p in Players)
        {
            positions.Add(p.CoordinatePosition);
        }
        return positions;
    }

    /// <summary>
    /// Defines currently selected player
    /// </summary>
    public void PlayerSelect(Player player)
    {
        CameraRotation.Instance.MainCam = true;
        PlayerManager.Instance.selectedPlayer = player;
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

    /// <summary>
    /// Plays audio
    /// </summary>
    /// <param name="player">currently selected player</param>
    public void Audio(Player player)
    {
        if (!movementSound.audioPlaying)
        {
            AudioManager.Instance.PlaySoundAtLocation(movementSound, soundEffect, null, true);
        }
    }

    /// <summary>
    /// Starts movement coroutine
    /// </summary>
    /// <param name="player">currently selected player</param>
    public void startMoveCoroutine(Player player)
    {
        StartCoroutine(Move(clickedTile));
    }

    /// <summary>
    /// Add methods to events
    /// </summary>
    public void AddMethods()
    {
        EventManager.OnEndTurnEvent += resetMovementPoints;
        EventManager.OnAbilityButtonEvent += AbilityClicked;
        EventManager.OnSelectPlayerEvent += PlayerSelect;
        EventManager.OnMoveEvent += startMoveCoroutine;
        EventManager.OnMoveEvent += Audio;
    }

    /// <summary>
    /// Removes methods from events
    /// </summary>
    public void RemoveMethods()
    {
        EventManager.OnEndTurnEvent -= resetMovementPoints;
        EventManager.OnAbilityButtonEvent -= AbilityClicked;
        EventManager.OnEndTurnEvent -= resetMovementPoints;
        EventManager.OnAbilityButtonEvent -= AbilityClicked;
        EventManager.OnSelectPlayerEvent -= PlayerSelect;
        EventManager.OnMoveEvent -= startMoveCoroutine;
        EventManager.OnMoveEvent -= Audio;
    }

    /// <summary>
    /// Removes methods from events
    /// </summary>
    private void OnDestroy()
    {
        StopAllCoroutines();
        RemoveMethods();
    }
}