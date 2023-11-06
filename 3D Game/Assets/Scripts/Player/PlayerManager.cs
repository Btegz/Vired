using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq.Expressions;
using UnityEngine;
using System.Windows;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public AbilityObjScript abilityObj;

    public List<Ability> abilitInventory;

    [SerializeField] List<GameObject> MovePoints;

    //MovePointsDoTween MovePointsDoTween;


    [SerializeField] int movementPointsPerTurn;
    private int movementAction = 4;
    private Vector3 mouse_pos;
    public GameObject player;
    public Camera cam;
    public Vector2Int PlayerSpawnPoint;
    public Vector2Int collisionPoint;
    public Vector2Int playerPosition;

    public int RessourceAInventory;
    public int RessourceBInventory;
    public int RessourceCInventory;
    public int RessourceDInventory;

    bool abilityActivated = false;
    private bool abilityUsable = true;

    public bool AbilityLoadoutActive;

    public GameObject indicatorPrefab;
    GameObject indicatorPrefabClone;

    //[SerializeField] ParticleSystem AbilityCastParticleSystem;

    [SerializeField] InputActionReference cancelAbilityInputActionReference;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        playerPosition = PlayerSpawnPoint;
        player.transform.position = GridManager.Instance.Grid[PlayerSpawnPoint].transform.position;
        EventManager.OnEndTurnEvent += resetMovementPoints;
        EventManager.OnAbilityButtonEvent += AbilityClicked;

    }

    private void OnDestroy()
    {
        EventManager.OnEndTurnEvent -= resetMovementPoints;
    }

    private void Update()
    {
        if (!AbilityLoadoutActive)
        {

            // takes mouse positition
            mouse_pos = Mouse.current.position.ReadValue();
            // Searches for the Nieghbors of playerposition
            List<Vector3Int> neighbors = HexGridUtil.CubeNeighbors(HexGridUtil.AxialToCubeCoord(playerPosition));

            // Lose Condition: surrounded by enemies/ no ressources 
            for (int i = 0; i < abilitInventory.Count; i++)
            {
                //check ob Ability bezahlbar ist 
                if (InventoryCheck(i))
                {
                    abilityUsable = true;
                    break;
                }

                else
                {
                    abilityUsable = false;
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
                    //SceneManager.LoadScene("GameOverScene");
                }
            }

            if (movementAction == 0 && Mouse.current.leftButton.wasPressedThisFrame)
            {
                Debug.Log("ich schüttle mich");
                player.transform.DOPunchRotation(new Vector3(10f, 2f), 1f);

            }

            // enters if Left Mouse Button was clicked
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                // checks whether movement points are available or if a Ability is activated
                if (movementAction > 0 || abilityActivated)
                {
                    // saves the Grid Tile Location that was clicked
                    Vector2Int clickedTile;

                    // enters if a tile was clicked
                    if (MouseCursorPosition(out clickedTile))
                    {
                        // enters if Players Neighbors contains the clicked Tile
                        if (neighbors.Contains(HexGridUtil.AxialToCubeCoord(clickedTile)) && !abilityActivated)
                        {
                            if (GridManager.Instance.Grid[clickedTile].currentGridState ==
                                GridManager.Instance.gS_Positive ||
                                GridManager.Instance.Grid[clickedTile].currentGridState ==
                                GridManager.Instance.gS_Neutral)

                                StartCoroutine(Move(clickedTile));
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
        Ray ray = cam.ScreenPointToRay(mouse_pos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            clickedTile = hit.collider.GetComponent<GridTile>().AxialCoordinate;
            return true;
        }
        else
        {
            clickedTile = Vector2Int.zero;
            return false;
        }
    }

    /// <summary>
    /// Coroutine to Move Player to a Coordinate
    /// </summary>
    /// <param name="moveTo">Coordinate to move player to</param>
    /// <returns></returns>
    IEnumerator Move(Vector2Int moveTo)
    {
        GridTile target = GridManager.Instance.Grid[moveTo];

        ParticleSystem landingCloud = player.GetComponentInChildren<ParticleSystem>();
        player.transform.DOJump(target.transform.position, 2, 1, .25f)
            .OnComplete(() => target.currentGridState.PlayerEnters(target));
        player.transform.DOPunchScale(Vector3.one * .1f, .25f).OnComplete(landingCloud.Play);
        movementAction--;

        MovePoints[movementAction].GetComponent<MovePointsDoTween>().Away();
        //MovePoints[movementAction].SetActive(false);


        playerPosition = moveTo;

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

    public void ChooseAbilityWithIndex(int index, Vector3Int selectedPoint, Vector3Int playerPos)
    {
        Ability chosenAbility = abilitInventory[index];
        AbilityObjScript AbilityPreview = Instantiate(abilityObj);
        AbilityPreview.ShowMesh(chosenAbility, selectedPoint, playerPos);
    }

    /// <summary>
    /// Called in OnClick of an AbilityButton.
    /// determins whether player has enough Ressources for the Ability
    /// </summary>
    /// <param name="index">index of the Ability Clicked</param>
    public void AbilityClicked(int index)
    {
        // sets the "abilityAcitvated" bool to true, so player cant move anymore after choosing a Ability
        if (abilityActivated == false && InventoryCheck(index))
        {
            abilityActivated = true;
            StartCoroutine(ChooseAbilityLocation(index));


            //SHOW THE ABILITY INDICATOR


            //Vector3 indicatorPosition = new Vector3(0, 0, 0);
            Quaternion indicatorRotation = Quaternion.Euler(0, 30, 0);
            indicatorPrefabClone = Instantiate(indicatorPrefab, player.transform.position, indicatorRotation);
        }
    }

    public bool InventoryCheck(int index)
    {
        //saves the cost of the chosen Ability
        Ressource resCost = abilitInventory[index].costs[0];

        //switches over the different ressources and checks whether player has anough ressources of fitting Type
        //the function returns if Player does not have enough Ressources for the Ability
        switch (resCost)
        {
            case Ressource.ressourceA:
                if (abilitInventory[index].costs.Count > RessourceAInventory)
                {
                    return false;
                }

                break;


            case Ressource.ressourceB:
                if (abilitInventory[index].costs.Count > RessourceBInventory)
                {
                    return false;
                }

                break;

            case Ressource.ressourceC:
                if (abilitInventory[index].costs.Count > RessourceCInventory)
                {
                    return false;
                }

                break;

            case Ressource.resscoureD:
                if (abilitInventory[index].costs.Count > RessourceDInventory)
                {
                    return false;
                }

                break;
        }

        return true;
    }

    /// <summary>
    /// Coroutine to Wait for Player to choose the Abilities Location after it was chosen
    /// </summary>
    /// <param name="AbilityIndex"></param>
    /// <returns></returns>
    public IEnumerator ChooseAbilityLocation(int AbilityIndex)
    {
        cancelAbilityInputActionReference.action.performed += CancelAbilityChoice;

        // collects player Neighbors as viable tiles
        List<Vector3Int> neighbors = HexGridUtil.CubeNeighbors(HexGridUtil.AxialToCubeCoord(playerPosition));

        //selectedPoint = HexGridUtil.AxialToCubeCoord(playerPosition);

        Vector2Int clickedTile;

        while (abilityActivated)
        {
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                CancelAbilityChoice();
                Destroy(indicatorPrefabClone);
            }

            // enters if Left Mouse Button was clicked
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (MouseCursorPosition(out clickedTile))
                {
                    if (neighbors.Contains(HexGridUtil.AxialToCubeCoord(clickedTile)))
                    {
                        ChooseAbilityWithIndex(AbilityIndex, HexGridUtil.AxialToCubeCoord(clickedTile),
                            HexGridUtil.AxialToCubeCoord(playerPosition));
                        break;
                    }

                    yield return null;
                }
            }

            yield return null;
        }

        yield return null;
    }


    public void AbilityCasted()
    {
        cancelAbilityInputActionReference.action.performed -= CancelAbilityChoice;
        abilityActivated = false;
        Destroy(indicatorPrefabClone);
    }

    public void CancelAbilityChoice(InputAction.CallbackContext actionCallBackContext)
    {
        abilityActivated = false;
        cancelAbilityInputActionReference.action.performed -= CancelAbilityChoice;
        Destroy(indicatorPrefabClone);
    }

    public void CancelAbilityChoice()
    {
        abilityActivated = false;
        cancelAbilityInputActionReference.action.performed -= CancelAbilityChoice;
        Destroy(indicatorPrefabClone);
    }
}