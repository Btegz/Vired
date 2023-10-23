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
    private int moves = 4;
    private Vector3 mouse_pos;
    public GameObject player;
    public Camera cam;
    public Vector2Int collisionPoint;
    private Vector3 Point;
    private Vector3 playerPosition;
    
    

    private void Update()
    {
        
            if (Mouse.current.leftButton.wasPressedThisFrame)

            {
                mouse_pos = Mouse.current.position.ReadValue();
                MouseCursorPosition();
                StartCoroutine(Move());
            }




    }






    private void MouseCursorPosition( )
    {
        Ray ray = cam.ScreenPointToRay(mouse_pos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            collisionPoint = hit.collider.GetComponent<GridTile>().AxialCoordinate;  
        }
     

//    Vector3 normCursor = Vector3.Normalize(Cursor);




    }

//     return GetComponent<Collider>();
     IEnumerator Move()
     {
         
         GridTile target = GridManager.Instance.Grid[collisionPoint];
         player.transform.position = target.transform.position;
         target.currentGridState.PlayerEnters(target);
         yield return null; 

     }

     // hexgridutils neighbors - Liste aus Koordinaten 
     // if Liste contains axial Koordinate dann StartCoroutine()
     // minus movement points
     // while/ if 

}
