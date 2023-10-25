using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq.Expressions;
using UnityEngine;
using System.Windows;
using Unity.VisualScripting;
using UnityEngine.InputSystem;


public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public AbilityObjScript abilityObj;

    public List<Ability> abilitInventory;

    private int movementAction = 4;
    private Vector3 mouse_pos;
    public GameObject player;
    public Camera cam;
    public Vector2Int collisionPoint;
    private Vector3 Point;
    public Vector3Int playerPosition;
    private Vector3Int selectedPoint;

    public int RessourceAInventory;
    public int RessourceBInventory;
    public int RessourceCInventory;
    public int RessourceDInventory;

    bool abilityActivated = false;

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
        playerPosition = Vector3Int.zero;
        player.transform.position = playerPosition;
        EventManager.OnEndTurnEvent += resetMovementPoints;

    }

    private void OnDestroy()
    {
        EventManager.OnEndTurnEvent -= resetMovementPoints;

    }

    private void Update()
    {
        mouse_pos = Mouse.current.position.ReadValue();

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (movementAction > 0 || abilityActivated)
            {
                MouseCursorPosition();
                HexGridUtil.CubeNeighbors(playerPosition);
                List<Vector3Int> neighbors = HexGridUtil.CubeNeighbors(playerPosition);

                if (neighbors.Contains(selectedPoint) && !abilityActivated)
                {
                    StartCoroutine(Move());
                }
            }
        }
    }

    private void MouseCursorPosition()
    {
        Ray ray = cam.ScreenPointToRay(mouse_pos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            collisionPoint = hit.collider.GetComponent<GridTile>().AxialCoordinate;
            selectedPoint = HexGridUtil.AxialToCubeCoord(collisionPoint);
        }






    }

    //     return GetComponent<Collider>();
    IEnumerator Move()
    {

        GridTile target = GridManager.Instance.Grid[collisionPoint];
        player.transform.position = target.transform.position;
        target.currentGridState.PlayerEnters(target);
        movementAction--;
        playerPosition = HexGridUtil.AxialToCubeCoord(target.AxialCoordinate);

        yield return null;

    }

    // hexgridutils neighbors - Liste aus Koordinaten 
    // if Liste contains axial Koordinate dann StartCoroutine()
    // minus movement points
    // while/ if 

    public void resetMovementPoints()
    {
        movementAction = 4;
    }

    public void ChooseAbilityWithIndex(int index, Vector3Int selectedPoint, Vector3Int playerPos)
    {
        Ability chosenAbility = abilitInventory[index];
        AbilityObjScript AbilityPreview = Instantiate(abilityObj);
        AbilityPreview.ShowMesh(chosenAbility, selectedPoint,playerPos);
        
    }

    public void AbilityClicked(int index)
    {
        Ressource resCost = abilitInventory[index].costs[0]; 
        switch (resCost)
        {
            case Ressource.ressourceA:
                if (abilitInventory[index].costs.Count >= RessourceAInventory)
                {
                    return;
                }
                break;
                
            case Ressource.ressourceB:
                if (abilitInventory[index].costs.Count >= RessourceBInventory)
                {
                    return;
                }
                break;
                
            case Ressource.ressourceC:
                if (abilitInventory[index].costs.Count >= RessourceCInventory)
                {
                    return;
                }
                break;
                
            case Ressource.resscoureD:
                if (abilitInventory[index].costs.Count >= RessourceDInventory)
                {
                    return;
                }
                break;
        }
        if (abilityActivated == false)
        {
            abilityActivated = true;
            StartCoroutine(ChooseAbilityLocation(index)); 
        }
        
   
    }


    public IEnumerator ChooseAbilityLocation(int AbilityIndex)
    {
        List<Vector3Int> neighbors = HexGridUtil.CubeNeighbors(playerPosition);

        selectedPoint = playerPosition;

        while (true)
        {
            if (neighbors.Contains(selectedPoint))
            {
                ChooseAbilityWithIndex(AbilityIndex, selectedPoint,playerPosition);
                break;
            }
            yield return null;
        }

        yield return null;
    }


    public void AbilityCasted()
    {
        abilityActivated = false;
    }
}
