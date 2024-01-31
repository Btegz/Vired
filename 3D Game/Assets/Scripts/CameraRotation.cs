using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class CameraRotation : MonoBehaviour
{
    public static CameraRotation Instance;
    public Player player;

    [SerializeField] Transform normalFollow;

    [SerializeField] InputActionReference rotatoAction;
    [SerializeField] InputActionReference zoomAction;
    [SerializeField] InputActionReference movementAction;
    //[SerializeField] InputActionReference switchAction;
    [SerializeField] InputActionReference playerswitchAction;
    //[SerializeField] InputActionReference topDownAction;
    [SerializeField] InputAction playerAction;
    [SerializeField] public Camera cam;
    [SerializeField] public CinemachineVirtualCamera Playercam;
    [SerializeField] public CinemachineVirtualCamera Worldcam;
    [SerializeField] public CinemachineVirtualCamera TopDownCam;
    [SerializeField] public CinemachineVirtualCamera AbilityUpgradeCam;
    [SerializeField] public CinemachineVirtualCamera AbilityLoadoutCam;

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
    [HideInInspector] public bool AbilityUpgrade;



    private float mouseScrollY;
    private float startMovementSpeed;

    private Coroutine rotationCoroutine;
    private Coroutine movementCoroutine;

    private Vector3 endPosition;
    private Vector3 camPosition;
    private Vector3 startingPosition;

    public bool dontMove;


    [HideInInspector][SerializeField] public bool MainCam = true;
    private Vector3 WorldcamStart;

    float maxX = 0;
    float maxZ = 0;
    float minX = float.MaxValue;
    float minZ = float.MaxValue;

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
        zoomAction.action.performed += ChangeMouseScrollY;

        movementAction.action.Enable();
        movementAction.action.started += StartMovement;
        movementAction.action.canceled += StopMovement;

        //switchAction.action.Enable();
        //switchAction.action.performed += SwitchtoMain;


        playerswitchAction.action.Enable();
        playerswitchAction.action.performed += playerSelection;
        //playerswitchAction.action.performed += _ => EventManager.OnSelectPlayer(PlayerManager.Instance.Players[(int)_.ReadValue<float>()]);

        //topDownAction.action.Enable();
        //topDownAction.action.performed += SwitchToTopDown;

        EventManager.OnSelectPlayerEvent += CameraCenterToPlayer;

    }

    private void ChangeMouseScrollY(InputAction.CallbackContext context)
    {
        mouseScrollY = context.ReadValue<float>();
    }

    private void SwitchToTopDown(InputAction.CallbackContext context)
    {
        CameraCenterToPlayer(PlayerManager.Instance.selectedPlayer);
        Worldcam.Priority = 0;
        AbilityUpgradeCam.Priority = 2;
        AbilityLoadoutCam.Priority = 1;
        TopDownCam.Priority = 3;
        //Playercam.Priority = 0;
        MainCam = true;
        transform.position = WorldcamStart;
        if (dropdown.value != 1)
            dropdown.value = 1;
        dropdown.RefreshShownValue();
    }

    private void playerSelection(InputAction.CallbackContext context)
    {
        PlayerManager.Instance.PlayerSelect(PlayerManager.Instance.Players[(int)context.ReadValue<float>()]);
        EventManager.OnSelectPlayer(PlayerManager.Instance.Players[(int)context.ReadValue<float>()]);
    }

    private void SwitchtoMain(InputAction.CallbackContext context)
    {
        SwitchtoMain();
    }

    void Start()
    {


        // Worldcam.Follow = transform; 
        WorldcamStart = transform.position;
        startingPosition = cam.transform.localPosition;
        startMovementSpeed = maxMovementSpeed;
       // dropdown = cameraDropDown.dropdown;

        foreach (KeyValuePair<Vector2Int, GridTile> kvp in GridManager.Instance.Grid)
        {
            Vector3 currentCoord = kvp.Value.transform.position;

            if (currentCoord.x > maxX)
            {
                maxX = currentCoord.x;
            }
            if (currentCoord.z > maxZ)
            {
                maxZ = currentCoord.z;
            }
            if (currentCoord.x < minX)
            {
                minX = currentCoord.x;
            }
            if (currentCoord.z < minZ)
            {
                minZ = currentCoord.z;
            }
        }
        //Debug.Log($"minX: {minX}, maxX: {maxX}, minZ: {minZ}, maxZ: {maxZ}");
        //jetzt ham wa die grääen Distanzen


    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        EventManager.OnSelectPlayerEvent -= CameraCenterToPlayer;

        rotatoAction.action.Disable();
        rotatoAction.action.started -= StartRotato;
        rotatoAction.action.canceled -= StopRotato;

        zoomAction.action.Disable();
        zoomAction.action.performed -= ChangeMouseScrollY;

        movementAction.action.Disable();
        movementAction.action.started -= StartMovement;
        movementAction.action.canceled -= StopMovement;

        //switchAction.action.Disable();
        //switchAction.action.performed -= SwitchtoMain;


        playerswitchAction.action.Disable();
        playerswitchAction.action.performed -= playerSelection;
        //playerswitchAction.action.performed += _ => EventManager.OnSelectPlayer(PlayerManager.Instance.Players[(int)_.ReadValue<float>()]);

        //topDownAction.action.Disable();
        //topDownAction.action.performed -= SwitchToTopDown;
    }

    void StopRotato(InputAction.CallbackContext obj)
    {
        if (AbilityUpgradeCam.Priority == 3 || AbilityLoadoutCam.Priority == 3)
        {
            return;
        }
        try
        {
            if (!dontMove)
                StopCoroutine(rotationCoroutine);

        }
        catch { }
    }
    void StartRotato(InputAction.CallbackContext obj)
    {
        if (AbilityUpgradeCam.Priority == 3 || AbilityLoadoutCam.Priority == 3)
        {
          

                return;
        }
        if (!dontMove)
            rotationCoroutine = StartCoroutine(Rotato());
    }

    void StartMovement(InputAction.CallbackContext obj)
    {
        if(!dontMove)
        movementCoroutine = StartCoroutine(Movement());
    }
    void StopMovement(InputAction.CallbackContext obj)
    {
        if (!dontMove)

            StopCoroutine(movementCoroutine);

    }


    private void Update()
    {
        endPosition = new Vector3(0, 1, 2);
        camPosition = cam.transform.localPosition;

        /// Zoom +
        /// Lerp von der Kamera Position durch die End Positon mit dem Scroll Input 


        if (PlayerManager.Instance.abilityActivated == false && AbilityUpgradeCam.Priority != 3)
        {

            if (mouseScrollY > 0)
            {
                MouseScrollDistance += MouseScrollStep;
                MouseScrollDistance = Mathf.Clamp(MouseScrollDistance, 0, 1);

                cam.transform.localPosition = Vector3.Lerp(camPosition, endPosition, MouseScrollDistance);
                maxMovementSpeed = Mathf.Lerp(maxMovementSpeed, minMovementSpeed, MouseScrollDistance);



                if (Worldcam.Priority == 3/* && MainCam == true*/)
                {
                    //Worldcam.transform.LookAt(cam.transform.position);
                    if (worldcamRecomposer.m_ZoomScale > MaxZoom)
                        worldcamRecomposer.m_ZoomScale -= 0.1f;
                }

                /*  else if (Playercam.Priority == 2)
                  {
                      //Playercam.transform.LookAt(Playercam.transform.position);
                      if (playercamRecomposer.m_ZoomScale > MaxZoom)
                          playercamRecomposer.m_ZoomScale -= 0.1f;
                  }*/

                else if (TopDownCam.Priority == 3)
                {
                    //TopDownCam.transform.LookAt(TopDownCam.transform.position);
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


                if (Worldcam.Priority == 3/* && MainCam == true*/)
                {
                    //Worldcam.transform.LookAt(cam.transform.position);
                    if (worldcamRecomposer.m_ZoomScale < MinZoom)
                        worldcamRecomposer.m_ZoomScale += 0.1f;

                }

                /*    else if (Playercam.Priority == 2)
                    {
                        //Playercam.transform.LookAt(Playercam.transform.position);
                        if (playercamRecomposer.m_ZoomScale < MinZoom)
                            playercamRecomposer.m_ZoomScale += 0.1f;

                    }*/

                else if (TopDownCam.Priority == 3)
                {
                    //TopDownCam.transform.LookAt(TopDownCam.transform.position);
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
        /*if (Playercam.Priority == 2)
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

        }*/

        if (Worldcam.Priority == 3)
        {
            if (Worldcam.Follow != null)
            {
                normalFollow.position = Worldcam.Follow.position;
            }

            Worldcam.LookAt = null;
            //Worldcam.Follow = null;
        }

        else if (TopDownCam.Priority == 3)
        {
            if (Worldcam.Follow != null)
            {
                normalFollow.position = TopDownCam.Follow.position;
            }
            TopDownCam.LookAt = null;
            //TopDownCam.Follow = null;
        }
        
       
        while (true)
        {
            Vector2 currentMousePosition = Pointer.current.position.ReadValue();

            //Wir erstellen uns ein Quad das auf 0 liegt und nach x und z sich ausdehnt(das ist der Boden)

            // wir schieäen einen Ray von Cam durch Screenmittelpunkt

            // wir holen uns den Punkt wo der Ray das Quad trifft und rotieren um diesen Punkt yo

            Plane boden = new Plane(Vector3.up, 0);

            Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

            var pointerWorldRay = Camera.main.ScreenPointToRay(screenCenter);

            boden.Raycast(pointerWorldRay, out float enterDistance);

            var previousPointerWorldPos = pointerWorldRay.GetPoint(enterDistance);

            if (Worldcam.Priority == 3)
            {
                Worldcam.transform.RotateAround(previousPointerWorldPos, Vector3.up, ((currentMousePosition - previousMousePosition).x / Screen.width) * rotationSpeed);
                previousMousePosition = currentMousePosition;
                yield return null;
            }
            else if (TopDownCam.Priority == 3)
            {
                TopDownCam.transform.RotateAround(previousPointerWorldPos, Vector3.up, ((currentMousePosition - previousMousePosition).x / Screen.width) * rotationSpeed);
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

        if (AbilityUpgradeCam.Priority == 3 || AbilityLoadoutCam.Priority == 3)
        {
            yield return null;
        }

        Vector3 previousMousePosition = Pointer.current.position.ReadValue();

        while (true)
        {
            if (Worldcam.Priority == 3)
            {
                if (Worldcam.Follow != null)
                {
                    normalFollow.position = Worldcam.Follow.position;
                }

                Worldcam.Follow = normalFollow;
                //Worldcam.LookAt = null;
                //Worldcam.Follow = transform;

            }

            else if (TopDownCam.Priority == 3)
            {
                if (TopDownCam.Follow != null)
                {
                    normalFollow.position = TopDownCam.Follow.position;
                    //TopDownCam.LookAt = transform;

                }
                TopDownCam.Follow = normalFollow;
            }

            else
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

            // Clamping the Campera Movement yo

            if (normalFollow.position.x + delta.x > maxX || normalFollow.position.x + delta.x < minX || normalFollow.position.z + delta.z > maxZ || normalFollow.position.z + delta.z < minZ)
            {
                yield return null;
            }
            else
            {
                normalFollow.position += delta;

                //float xpos = Mathf.Clamp(normalFollow.position.x, minX, maxX);
                //float zpos = Mathf.Clamp(normalFollow.position.z, minZ, maxZ);
                //normalFollow.position = new Vector3(xpos, normalFollow.position.y, zpos);

                previousMousePosition = currentPointerPos;
                yield return null;
            }
        }
    }
    /// <summary>
    /// Priority Switch der einzelnen Kameras 
    /// Kamera mit der hächsten Priority wird angezeigt 
    /// </summary>
    public void SwitchtoMain()
    {
        CameraCenterToPlayer(PlayerManager.Instance.selectedPlayer);
        Worldcam.Priority = 3;
        AbilityUpgradeCam.Priority = 1;
        TopDownCam.Priority = 0;
        AbilityLoadoutCam.Priority = 2;

        //Playercam.Priority = 0;
        MainCam = true;
        transform.position = WorldcamStart;
        /* if (dropdown.value != 0)
             dropdown.value = 0;*/
        // dropdown.RefreshShownValue();

    }

    public void SwitchToPlayer()
    {
        //CameraCenterToPlayer(PlayerManager.Instance.selectedPlayer);

        //Worldcam.Priority = 0;
        //TopDownCam.Priority = 1;
        //Playercam.Priority = 2;

        //MainCam = false;
        //if (dropdown.value != 2)
        //    dropdown.value = 2;
        //dropdown.RefreshShownValue();
    }

    public void SwitchToTopDown()
    {
        CameraCenterToPlayer(PlayerManager.Instance.selectedPlayer);
        Worldcam.Priority = 0;
        AbilityUpgradeCam.Priority = 1;
        TopDownCam.Priority = 3;
        AbilityLoadoutCam.Priority = 2;
        transform.position = WorldcamStart;
        /*if (dropdown.value != 1)
            dropdown.value = 1;*/


    }


    public void CameraCenterToPlayer(Player player)
    {
        //if (Playercam.Priority == 2)
        //    {
        //        Playercam.LookAt = player.transform;
        //        Playercam.Follow = player.transform;


        //    }

        if (Worldcam.Priority == 3)
        {
            Worldcam.LookAt = player.transform;
            Worldcam.Follow = player.transform;
        }

        else if (TopDownCam.Priority == 3)
        {
            TopDownCam.Follow = player.transform;
        }

        else if (AbilityUpgradeCam.Priority == 3)
        {
            AbilityUpgradeCam.Follow = player.transform;
        }

    }

    public void AbilityLoadOut()
    {

        PlayerPrefs.SetInt("AbilityLoadOut", AbilityUpgradeCam.Priority);
        PlayerPrefs.SetInt("World", Worldcam.Priority);
        PlayerPrefs.SetInt("Topdown", TopDownCam.Priority);


        Worldcam.Priority = 1;
        AbilityUpgradeCam.Priority = 3;
        AbilityLoadoutCam.Priority = 2;
        TopDownCam.Priority = 0;
        transform.position = WorldcamStart;
        CameraCenterToPlayer(PlayerManager.Instance.selectedPlayer);



    }


}