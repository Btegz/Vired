using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;
using TMPro;

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
    [SerializeField] public CinemachineVirtualCamera Worldcam;
    [SerializeField] public CinemachineVirtualCamera TopDownCam;

    [SerializeField] float rotationSpeed = 500;
    [SerializeField] float maxMovementSpeed = 50;
    [SerializeField] float minMovementSpeed = 5f;
    [SerializeField] public float MaxZoom;
    [SerializeField] public float MinZoom;
    [SerializeField] float MouseZoom;
    [SerializeField] float MouseZoomStep;
    [SerializeField] float MouseScrollStep;
    [SerializeField] float MouseScrollDistance;
    TMP_Dropdown dropdown;
    public CameraDropDown cameraDropDown;
    public CinemachineRecomposer worldcamRecomposer;
    public CinemachineRecomposer playercamRecomposer;
    public CinemachineRecomposer topdowncamRecomposer;



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
        playerswitchAction.action.performed += _ => EventManager.OnSelectPlayer(PlayerManager.Instance.Players[(int)_.ReadValue<float>()]);

        topDownAction.action.Enable();
        topDownAction.action.performed += _ => SwitchToTopDown();

        EventManager.OnSelectPlayerEvent += CameraCenterToPlayer;

    }

    void Start()
    {   
        Worldcam.Follow = transform; 
        WorldcamStart = transform.position;
        startingPosition = cam.transform.localPosition;
        startMovementSpeed = maxMovementSpeed;
        dropdown = cameraDropDown.dropdown;
        


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


        if (PlayerManager.Instance.abilityActivated == false)
        {

            if (mouseScrollY > 0)
            {
                MouseScrollDistance += MouseScrollStep;
                MouseScrollDistance = Mathf.Clamp(MouseScrollDistance, 0, 1);

                cam.transform.localPosition = Vector3.Lerp(camPosition, endPosition, MouseScrollDistance);
                maxMovementSpeed = Mathf.Lerp(maxMovementSpeed, minMovementSpeed, MouseScrollDistance);



                if (Worldcam.Priority == 2 && MainCam == true)
                {
                    Worldcam.transform.LookAt(cam.transform.position);
                    if (worldcamRecomposer.m_ZoomScale > MaxZoom)
                        worldcamRecomposer.m_ZoomScale -= 0.1f;
                }

                else if (Playercam.Priority == 2)
                {
                    Playercam.transform.LookAt(Playercam.transform.position);
                    if (playercamRecomposer.m_ZoomScale > MaxZoom)
                        playercamRecomposer.m_ZoomScale -= 0.1f;
                }

                else
                {
                    TopDownCam.transform.LookAt(TopDownCam.transform.position);
                    if (topdowncamRecomposer.m_ZoomScale > MaxZoom)
                        topdowncamRecomposer.m_ZoomScale -= 0.1f;
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
                    if (worldcamRecomposer.m_ZoomScale < MinZoom)
                        worldcamRecomposer.m_ZoomScale += 0.1f;

                }

                else if (Playercam.Priority == 2)
                {
                    Playercam.transform.LookAt(Playercam.transform.position);
                    if (playercamRecomposer.m_ZoomScale < MinZoom)
                        playercamRecomposer.m_ZoomScale += 0.1f;

                }

                else
                {
                    TopDownCam.transform.LookAt(TopDownCam.transform.position);
                    if (topdowncamRecomposer.m_ZoomScale < MinZoom)
                        topdowncamRecomposer.m_ZoomScale += 0.1f;
                }
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
            if (Worldcam.Priority == 2)
            {
                Worldcam.LookAt = null;
                Worldcam.Follow = null;
            }

            else if (TopDownCam.Priority == 2)
            {
                TopDownCam.LookAt = null;
                TopDownCam.Follow = null;
            }
            while (true)
            {
                Vector2 currentMousePosition = Pointer.current.position.ReadValue();

                //Wir erstellen uns ein Quad das auf 0 liegt und nach x und z sich ausdehnt(das ist der Boden)

                // wir schießen einen Ray von Cam durch Screenmittelpunkt

                // wir holen uns den Punkt wo der Ray das Quad trifft und rotieren um diesen Punkt yo

                Plane boden = new Plane(Vector3.up, 0);

                Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

                var pointerWorldRay = Camera.main.ScreenPointToRay(screenCenter);

                boden.Raycast(pointerWorldRay, out float enterDistance);

                var previousPointerWorldPos = pointerWorldRay.GetPoint(enterDistance);


                transform.RotateAround(previousPointerWorldPos, Vector3.up, ((currentMousePosition - previousMousePosition).x / Screen.width) * rotationSpeed);
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
            if (Worldcam.Priority == 2)
            {
                
                Worldcam.LookAt = null;
                Worldcam.Follow = transform;
     
            }

            else if (TopDownCam.Priority == 2)
            {
                TopDownCam.LookAt = null;
                TopDownCam.Follow = transform;
            }

            else if (Playercam.Priority == 2)
            {
                break;
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
        if (dropdown.value != 0)
            dropdown.value = 0;
        dropdown.RefreshShownValue();

    }

    public void SwitchToPlayer()
    {

        /* if (TopDownCam.Priority == 2 || Worldcam.Priority == 2)
         {
             return;
         }

         else 
         { */
        Worldcam.Priority = 0;
        TopDownCam.Priority = 1;
        Playercam.Priority = 2;

        MainCam = false;
        if (dropdown.value != 2)
            dropdown.value = 2;
        dropdown.RefreshShownValue();

        //}

    }

    public void SwitchToTopDown()
    {

        Worldcam.Priority = 0;
        Playercam.Priority = 1;
        TopDownCam.Priority = 2;
        transform.position = WorldcamStart;
        if (dropdown.value != 1)
            dropdown.value = 1;


    }


    public void CameraCenterToPlayer(Player player)
    {
        if (Playercam.Priority == 2)
        {
            Playercam.LookAt = PlayerManager.Instance.selectedPlayer.transform;
            Playercam.Follow = PlayerManager.Instance.selectedPlayer.transform;


        }

        else if (Worldcam.Priority == 2)
        {
            Worldcam.LookAt = PlayerManager.Instance.selectedPlayer.transform;
            Worldcam.Follow = PlayerManager.Instance.selectedPlayer.transform;
        }

        else
        {
            //TopDownCam.LookAt = PlayerManager.Instance.selectedPlayer.transform;
            TopDownCam.Follow = PlayerManager.Instance.selectedPlayer.transform;
        }

    }

}