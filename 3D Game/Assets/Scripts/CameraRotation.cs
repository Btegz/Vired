using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CameraRotation : MonoBehaviour
{
    public static CameraRotation Instance;


    [SerializeField] InputActionReference rotatoAction;
    [SerializeField] InputActionReference zoomAction;
    [SerializeField] InputActionReference movementAction;
    [SerializeField] InputActionReference switchAction;
    [SerializeField] InputActionReference playerswitchAction;
    [SerializeField] InputActionReference topDownAction;
    [SerializeField] InputAction playerAction;
    [SerializeField] Camera cam;
    [SerializeField] CinemachineVirtualCamera Playercam;
    [SerializeField] CinemachineVirtualCamera Worldcam;
    [SerializeField] CinemachineVirtualCamera TopDownCam;

    [SerializeField] float rotationSpeed = 500;
    [SerializeField] float maxMovementSpeed = 50;
    [SerializeField] private float minMovementSpeed = 5f;
    [SerializeField] private float speedAdaption = 0.2f;


    [SerializeField] Vector3 endPosition;

    private float mouseScrollY;
    private float startMovementSpeed;

    private Coroutine rotationCoroutine;
    private Coroutine movementCoroutine;

    private Vector3 camPosition;
    private Vector3 startingPosition;
    [SerializeField] float MouseScrollStep;
    [SerializeField] float MouseScrollDistance;
    [SerializeField] public bool MainCam = true;
    public float keyPressed;
    private bool playerSwitchCalled = false;


    public Player player;

    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        rotatoAction.action.Enable();
        rotatoAction.action.started += StartRotato;
        rotatoAction.action.canceled += StopRotato;

        zoomAction.action.Enable();
        zoomAction.action.performed += x => mouseScrollY = x.ReadValue<float>();

        movementAction.action.Enable();
        movementAction.action.started += StartMovement;
        movementAction.action.canceled += StopMovement;

        switchAction.action.Enable();
        switchAction.action.performed += _ => SwitchtoMain();

        playerswitchAction.action.Enable();
        playerswitchAction.action.performed += ctx => keyPressed = ctx.ReadValue<float>();
        playerswitchAction.action.performed += _ => player.PlayerSelect();

        topDownAction.action.Enable();
        topDownAction.action.performed += _ => SwitchToTopDown();






    }

    void Start()
    {
        startingPosition = gameObject.transform.GetChild(0).transform.localPosition;
        startMovementSpeed = maxMovementSpeed;

    }

    void StopRotato(InputAction.CallbackContext obj)
    {
        try
        {
            StopCoroutine(rotationCoroutine);
        }
        catch { }
    }
    void StartRotato(InputAction.CallbackContext obj)
    {
        rotationCoroutine = StartCoroutine(Rotato());
    }

    void StartMovement(InputAction.CallbackContext obj)
    {
        movementCoroutine = StartCoroutine(Movement());
    }
    void StopMovement(InputAction.CallbackContext obj)
    {
        StopCoroutine(movementCoroutine);
    }


    private void Update()
    {

        //Playercam.transform.localPosition = (Vector3)HexGridUtil.AxialToCubeCoord(PlayerManager.Instance.selectedPlayer.CoordinatePosition) - new Vector3(2,2,2);



        endPosition = new Vector3(0, 1, 2);
        camPosition = gameObject.transform.GetChild(0).transform.localPosition;

        /// Zoom +
        /// Lerp von der Kamera Position durch die End Positon mit dem Scroll Input 
        if (mouseScrollY > 0)
        {
            MouseScrollDistance += MouseScrollStep;
            MouseScrollDistance = Mathf.Clamp(MouseScrollDistance, 0, 1);
            gameObject.transform.GetChild(0).transform.localPosition = Vector3.Lerp(camPosition, endPosition, MouseScrollDistance);
            maxMovementSpeed = Mathf.Lerp(maxMovementSpeed, minMovementSpeed, MouseScrollDistance);
            if (MainCam)
            {
                Worldcam.transform.LookAt(gameObject.transform.GetChild(0).transform.position);
                Worldcam.m_Lens.FieldOfView -= MouseScrollDistance;
            }

            else
            {
                Playercam.transform.LookAt(gameObject.transform.GetChild(0).transform.position);
                Playercam.m_Lens.FieldOfView -= MouseScrollDistance;
            }
        }

        /// Zoom -
        /// Lerp von der anfangs Kamera Position durch die aktuelle KameraPosition mit dem Scroll Input 
        if (mouseScrollY < 0)
        {
            MouseScrollDistance += MouseScrollStep;
            MouseScrollDistance = Mathf.Clamp(MouseScrollDistance, 0, 1);
            gameObject.transform.GetChild(0).transform.localPosition = Vector3.Lerp(startingPosition, camPosition, MouseScrollDistance);
            maxMovementSpeed = Mathf.Lerp(startMovementSpeed, maxMovementSpeed, MouseScrollDistance);

            if (MainCam)
            {
                Worldcam.transform.LookAt(gameObject.transform.GetChild(0).transform.position);
                Worldcam.m_Lens.FieldOfView += MouseScrollDistance;
            }

            else
            {
                Playercam.transform.LookAt(gameObject.transform.GetChild(0).transform.position);
                Playercam.m_Lens.FieldOfView += MouseScrollDistance;
            }

        }
    }

    /// <summary>
    /// Rotation der Kamera um das Spielfeld mit der rechten Maustaste
    /// </summary>
    /// <returns></returns>
    IEnumerator Rotato()
    {
        Vector2 previousMousePosition = Pointer.current.position.ReadValue();

        while (true)
        {
            Vector2 currentMousePosition = Pointer.current.position.ReadValue();
            transform.Rotate(Vector3.up, ((currentMousePosition - previousMousePosition).x / Screen.width) * rotationSpeed);
            previousMousePosition = currentMousePosition;
            yield return null;
        }
    }

    /// <summary>
    /// Movement der Kamera auf der x und z Achse mit der mittleren Maustaste
    /// </summary>
    /// <returns></returns>
    IEnumerator Movement()
    {
        Vector3 previousMousePosition = Pointer.current.position.ReadValue();
        while (true)
        {
            //Vector3 currentMousePosition = Pointer.current.position.ReadValue();
            ///*transform.GetChild(0).*/transform./*local*/position += new Vector3((-(currentMousePosition - previousMousePosition).x / Screen.width) * maxMovementSpeed, transform.localPosition.y, ((-(currentMousePosition - previousMousePosition).y / Screen.width) * maxMovementSpeed));
            //previousMousePosition = currentMousePosition;
            //yield return null;

            var pointerWorldRay = cam.ScreenPointToRay(previousMousePosition);
            Plane groundPlane = new Plane(Vector3.up, 1);
            groundPlane.Raycast(pointerWorldRay, out var EnterDistance);
            Vector3 previousPointerWorldPos = pointerWorldRay.GetPoint(EnterDistance);

            Vector2 currentPointerPos = Pointer.current.position.ReadValue();
            pointerWorldRay = cam.ScreenPointToRay(currentPointerPos);
            groundPlane.Raycast(pointerWorldRay, out var enterDistance);
            Vector3 currentPointerWorldPos = pointerWorldRay.GetPoint(enterDistance);

            Vector3 delta = previousPointerWorldPos - currentPointerWorldPos;

            transform.position += delta;

            previousMousePosition = currentPointerPos;
            yield return null;


        }
    }

    public void SwitchtoMain()
    {
        Worldcam.Priority = 2;
        TopDownCam.Priority = 1;
        Playercam.Priority = 0;
        MainCam = true;
    }

    public void SwitchToPlayer()
    {
        Worldcam.Priority = 0;
        TopDownCam.Priority = 1;
        Playercam.Priority = 2;
        keyPressed = 0;
        MainCam = false;
    }

    public void SwitchToTopDown()
    {
        Worldcam.Priority = 0;
        Playercam.Priority = 1;
        TopDownCam.Priority = 2;

    }
}