using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private InputActionReference rotatoAction;
    [SerializeField] InputActionReference zoomAction;

    [SerializeField] float speed = 500;

    private Coroutine rotationCoroutine;
    private Coroutine zoomCoroutine;

    public float mouseScrollY;
    // Start is called before the first frame update
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
        Vector3 dir = new Vector3(0, 1, 1);

        if (mouseScrollY > 0)
        {
            gameObject.transform.GetChild(0).transform.localPosition += dir * mouseScrollY * Time.deltaTime;
        }
        if (mouseScrollY < 0)
            gameObject.transform.GetChild(0).transform.localPosition += dir * mouseScrollY * Time.deltaTime;
        //delta der Kamera und des Zentrums und hierlang bewegen
            
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
        // Zoom Camera lokale Z Position 


    }

    IEnumerator Movement()
    {
        yield return null;
    }
}
