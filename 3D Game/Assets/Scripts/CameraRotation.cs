using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private InputActionReference rotatoAction;

    [SerializeField] float speed =500;

    private Coroutine rotationCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        rotatoAction.action.Enable();
        rotatoAction.action.started += StartRotato;
        rotatoAction.action.canceled += StopRotato;

    }
    
    void StopRotato(InputAction.CallbackContext obj)
    {
        StopCoroutine(rotationCoroutine);
    }
    void StartRotato(InputAction.CallbackContext obj)
    {
        rotationCoroutine = StartCoroutine(Rotato());
    }
    IEnumerator Rotato()
    {
        Vector2 previousMousePosition = Pointer.current.position.ReadValue();

        while (true)
        {
            Vector2 currentMousePosition = Pointer.current.position.ReadValue();
            transform.Rotate(Vector3.up, (currentMousePosition-previousMousePosition).x/Screen.width * speed);
            
            previousMousePosition = currentMousePosition;   
            yield return null;
        }
        
        //Movement x und z Position des Parents 
        // Zoom Camera lokale Z Position 
        
     
    }
}
