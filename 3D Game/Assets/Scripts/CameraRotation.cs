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
    public Player player;

    [SerializeField] InputActionReference rotatoAction;
    [SerializeField] InputActionReference zoomAction;
    [SerializeField] InputActionReference movementAction;
    [SerializeField] InputActionReference switchAction;
    [SerializeField] InputActionReference playerswitchAction;
    [SerializeField] InputActionReference topDownAction;
    [SerializeField] InputAction playerAction;
    [SerializeField] Camera cam;
    [SerializeField] public CinemachineVirtualCamera Playercam;
    [SerializeField] CinemachineVirtualCamera Worldcam;
    [SerializeField] CinemachineVirtualCamera TopDownCam;

    [SerializeField] float rotationSpeed = 500;
    [SerializeField] float maxMovementSpeed = 50;
    [SerializeField] float minMovementSpeed = 5f;
    [SerializeField] public float MaxZoom;
    [SerializeField] public float MinZoom;
    [SerializeField] float MouseZoom;
    [SerializeField] float MouseZoomStep;
    [SerializeField] float MouseScrollStep;
    [SerializeField] float MouseScrollDistance;
    
    private float mouseScrollY;
    private float startMovementSpeed;

    private Coroutine rotationCoroutine;
    private Coroutine movementCoroutine;

    private Vector3 endPosition;
    private Vector3 camPosition;
    private Vector3 startingPosition;

    
    [HideInInspector][SerializeField] public bool MainCam = true;
    private Vector3 WorldcamStart;

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
        playerswitchAction.action.performed += _ => PlayerManager.Instance.PlayerSelect(PlayerManager.Instance.Players[(int)_.ReadValue<float>()]);

        topDownAction.action.Enable();
        topDownAction.action.performed += _ => SwitchToTopDown();
    }

    void Start()
    {
        WorldcamStart = transform.position;
        startingPosition = cam.transform.localPosition;
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
        endPosition = new Vector3(0, 1, 2);
        camPosition = cam.transform.localPosition;

        /// Zoom +
        /// Lerp von der Kamera Position durch die End Positon mit dem Scroll Input 
        if (mouseScrollY > 0)
        {
            MouseScrollDistance += MouseScrollStep;
            MouseScrollDistance = Mathf.Clamp(MouseScrollDistance, 0, 1);

            cam.transform.localPosition = Vector3.Lerp(camPosition, endPosition, MouseScrollDistance);
            maxMovementSpeed = Mathf.Lerp(maxMovementSpeed, minMovementSpeed, MouseScrollDistance);



            if (Worldcam.Priority == 2 && MainCam == true)
            {
                Worldcam.transform.LookAt(cam.transform.position);
                if (Worldcam.m_Lens.FieldOfView > MaxZoom)
               Worldcam.m_Lens.FieldOfView -= MouseScrollDistance;
            }

            else if (Playercam.Priority == 2)
            {
                Playercam.transform.LookAt(Playercam.transform.position);
                Playercam.m_Lens.FieldOfView -= MouseScrollDistance;
            }

            else
            {
                TopDownCam.transform.LookAt(TopDownCam.transform.position);
                TopDownCam.m_Lens.FieldOfView -= MouseScrollDistance;
            }
        }

        /// Zoom -
        /// Lerp von der anfangs Kamera Position durch die aktuelle KameraPosition mit dem Scroll Input 
        if (mouseScrollY < 0)
        {
            MouseScrollDistance += MouseScrollStep;
            MouseScrollDistance = Mathf.Clamp(MouseScrollDistance, 0, 1);
            cam.transform.localPosition = Vector3.Lerp(camPosition, startingPosition, MouseScrollDistance);
            maxMovementSpeed = Mathf.Lerp(startMovementSpeed, maxMovementSpeed, MouseScrollDistance);


            if (Worldcam.Priority == 2 && MainCam == true)
            {
                Worldcam.transform.LookAt(cam.transform.position);
                if (Worldcam.m_Lens.FieldOfView < MinZoom)
                Worldcam.m_Lens.FieldOfView += MouseScrollDistance;
            }

            else if (Playercam.Priority == 2)
            {
                Playercam.transform.LookAt(Playercam.transform.position);
                Playercam.m_Lens.FieldOfView += MouseScrollDistance;       

            }

            else
            {
                TopDownCam.transform.LookAt(TopDownCam.transform.position);
                TopDownCam.m_Lens.FieldOfView += MouseScrollDistance;
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
        if (Playercam.Priority == 2)
        {
            CameraRotation.Instance.Playercam.LookAt = null;
            CameraRotation.Instance.Playercam.Follow = null;
            while (true)
            {
                Vector2 currentMousePosition = Pointer.current.position.ReadValue();
                transform.RotateAround(PlayerManager.Instance.selectedPlayer.transform.position, Vector3.up, ((currentMousePosition - previousMousePosition).x / Screen.width) * rotationSpeed);
                previousMousePosition = currentMousePosition;
                yield return null;

            }
        }
        else
        {
            while (true)
            {
                Vector2 currentMousePosition = Pointer.current.position.ReadValue();
                transform.Rotate(Vector3.up, ((currentMousePosition - previousMousePosition).x / Screen.width) * rotationSpeed);
                previousMousePosition = currentMousePosition;
                yield return null;

            }
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
            if (Playercam.Priority == 2)
            {
                CameraRotation.Instance.Playercam.LookAt = null;
                CameraRotation.Instance.Playercam.Follow = null;
            }
        

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
    /// <summary>
    /// Priority Switch der einzelnen Kameras 
    /// Kamera mit der höchsten Priority wird angezeigt 
    /// </summary>
    public void SwitchtoMain()
    {
        Worldcam.Priority = 2;
        TopDownCam.Priority = 1;
        Playercam.Priority = 0;
        MainCam = true;
        transform.position = WorldcamStart;

    }

    public void SwitchToPlayer()
    {
        Worldcam.Priority = 0;
        TopDownCam.Priority = 1;
        Playercam.Priority = 2;

        MainCam = false;
    }

    public void SwitchToTopDown()
    {
        Worldcam.Priority = 0;
        Playercam.Priority = 1;
        TopDownCam.Priority = 2;
        transform.position = WorldcamStart;


    }
}