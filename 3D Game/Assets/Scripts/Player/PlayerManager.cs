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

    private void Awake()
    {
        if(Instance == null)
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
            if (movementAction > 0)
            {

                MouseCursorPosition();
                HexGridUtil.CubeNeighbors(playerPosition);
                List<Vector3Int> neighbors = HexGridUtil.CubeNeighbors(playerPosition);

                if (neighbors.Contains(selectedPoint))
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


        //    Vector3 normCursor = Vector3.Normalize(Cursor);




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

}
