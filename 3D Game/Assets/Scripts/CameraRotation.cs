using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private InputActionReference rotatoAction;
    [SerializeField] InputActionReference zoomAction;

    [SerializeField] float speed = 500;
    [SerializeField] Camera cam;
    
    private Coroutine rotationCoroutine;
    private Coroutine zoomCoroutine;

    private Vector3 camPosition;
    private Vector3 startingPosition;
    

    public float mouseScrollY;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = gameObject.transform.GetChild(0).transform.localPosition;
    }
    void Awake()
    {
        rotatoAction.action.Enable();
        rotatoAction.action.started += StartRotato;
        rotatoAction.action.canceled += StopRotato;

        zoomAction.action.Enable();
        zoomAction.action.performed += x => mouseScrollY = x.ReadValue<float>();
    }

    void StopRotato(InputAction.CallbackContext obj)
    {
        StopCoroutine(rotationCoroutine);
    }
    void StartRotato(InputAction.CallbackContext obj)
    {
        rotationCoroutine = StartCoroutine(Rotato());
    }



    private void Update()
    {
    
        camPosition = gameObject.transform.GetChild(0).transform.localPosition;

        if (mouseScrollY > 0 )
        {  
            gameObject.transform.GetChild(0).transform.localPosition =
                Vector3.Lerp(camPosition,Vector3.zero, mouseScrollY * Time.deltaTime );
        }
        
        if (mouseScrollY < 0)
        {
            Debug.Log(startingPosition);
            Debug.Log((mouseScrollY * Time.deltaTime));
            gameObject.transform.GetChild(0).transform.localPosition =
                Vector3.Lerp(startingPosition,camPosition, ((mouseScrollY * Time.deltaTime )+1));
        }
    }
    IEnumerator Rotato()
    {
        Vector2 previousMousePosition = Pointer.current.position.ReadValue();
        
        while (true)
        {
            Vector2 currentMousePosition = Pointer.current.position.ReadValue();
            transform.Rotate(Vector3.up, (currentMousePosition - previousMousePosition).x / Screen.width * speed);

            previousMousePosition = currentMousePosition;
            yield return null;
        }
        //Movement x und z Position des Parents 
    }

    IEnumerator Movement()
    {
        yield return null;
    }
}
