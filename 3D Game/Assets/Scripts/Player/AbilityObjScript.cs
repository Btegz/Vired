using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class AbilityObjScript : MonoBehaviour
{
    public Ability ability;
    public GridTile gridTile;
    public List<Vector2Int> AbilityShapeLocation;


    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    [SerializeField] Material positiveMaterial;
    [SerializeField] Material negativeMaterial;
    [SerializeField] Material movementMaterial;

    [SerializeField] InputActionAsset inputAction;
    [SerializeField] InputActionReference rotationInputActionReference;
    [SerializeField] InputActionReference castAbiltyInputActionReference;
    [SerializeField] InputActionReference CancelAbilityInputActionReference;


    Camera cam;

    private void OnDestroy()
    {
        castAbiltyInputActionReference.action.performed -= CastAbility;
        CancelAbilityInputActionReference.action.performed -= KillYourSelf;
        rotationInputActionReference.action.performed -= rotateAbility;
    }

    public void ShowMesh(Ability ability, Vector3Int SpawnPoint, Vector3Int playerPos)
    {
        this.ability = ability;
        AbilityShapeLocation = ability.Coordinates;
        List<Vector3Int> CubeAbilityShapeLocation = HexGridUtil.CubeAddRange(HexGridUtil.AxialToCubeCoord(AbilityShapeLocation), playerPos);

        meshFilter = GetComponentInChildren<MeshFilter>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        meshFilter.mesh = ability.previewShape;
        Material[] materials = new Material[ability.Effects.Count];
        for (int i = 0; i < ability.Effects.Count; i++)
        {
            switch (ability.Effects[i])
            {
                case Effect.Positive:
                    materials[i] = positiveMaterial;
                    break;

                case Effect.Movement:
                    materials[i] = movementMaterial;
                    break;
               
                default:
                    materials[i] = negativeMaterial;
                    break;
            }
        }
        meshRenderer.materials = materials;

        Vector3Int selectedDirection = HexGridUtil.CubeSubstract(SpawnPoint, playerPos);

        Vector3Int[] lol = HexGridUtil.cubeDirectionVectors;

        List<Vector3Int> dumm = new List<Vector3Int>();
        foreach (Vector3Int i in lol)
        {
            dumm.Add(i);
        }

        AbilityShapeLocation = HexGridUtil.CubeToAxialCoord(HexGridUtil.CubeAddRange(HexGridUtil.RotateRangeClockwise(playerPos, HexGridUtil.AxialToCubeCoord(AbilityShapeLocation), dumm.IndexOf(selectedDirection)), playerPos));
        CubeAbilityShapeLocation = HexGridUtil.RotateRangeClockwise(playerPos, CubeAbilityShapeLocation, dumm.IndexOf(selectedDirection));

        transform.rotation *= Quaternion.Euler(0, dumm.IndexOf(selectedDirection) * 60, 0);


        AbilityShapeLocation = HexGridUtil.CubeToAxialCoord(CubeAbilityShapeLocation);

        SetPositionToGridCoord(HexGridUtil.CubeToAxialCoord(playerPos));

        castAbiltyInputActionReference.action.performed += CastAbility;
    }

    public void SetPositionToGridCoord(Vector2Int coord)
    {
        transform.position = GridManager.Instance.Grid[coord].transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        inputAction.Enable();
        rotationInputActionReference.action.performed += rotateAbility; 
        CancelAbilityInputActionReference.action.performed += KillYourSelf;

    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 mouse_pos = Mouse.current.position.ReadValue();
        //Debug.Log(mouse_pos); 

        //Ray ray = cam.ScreenPointToRay(mouse_pos);
        //RaycastHit hit;

        //if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        //{
        //    //GridTile tile;
        //    //hit.collider.TryGetComponent<GridTile>(out tile);

        //    Vector2Int gridCoord = hit.collider.GetComponent<GridTile>().AxialCoordinate;
        //    if(gridCoord != null)
        //    {
        //        SetPositionToGridCoord(gridCoord);
        //    }
        //}
    }

    public void rotateAbility(InputAction.CallbackContext action)
    {

        float rotation = action.ReadValue<float>();
        if (rotation > 0)
        {
            switch (ability.rotato)
            {
                case RotationMode.PlayerCenter:
                    rotateClockwisePlayerCenter();
                    break;

                case RotationMode.SelectedPointCenter:
                    rotateClockwise();
                    break;
            }
        }
        if (rotation < 0)
        {
            switch (ability.rotato)
            {
                case RotationMode.PlayerCenter:
                    rotateCounterClockwisePlayerCenter();
                    break;

                case RotationMode.SelectedPointCenter:
                    rotateCounterClockwise();
                    break;
            }

        }
    }

    public void rotateClockwise()
    {
        transform.RotateAround(GridManager.Instance.Grid[AbilityShapeLocation[0]].transform.position, new Vector3(0, 1, 0), 60);
        AbilityShapeLocation = HexGridUtil.CubeToAxialCoord(HexGridUtil.RotateRangeClockwise(HexGridUtil.AxialToCubeCoord(AbilityShapeLocation[0]), HexGridUtil.AxialToCubeCoord(AbilityShapeLocation), 1));

    }

    public void rotateCounterClockwise()
    {
        transform.RotateAround(GridManager.Instance.Grid[AbilityShapeLocation[0]].transform.position, new Vector3(0, -1, 0), 60);
        AbilityShapeLocation = HexGridUtil.CubeToAxialCoord(HexGridUtil.RotateRangeCounterClockwise(HexGridUtil.AxialToCubeCoord(AbilityShapeLocation[0]), HexGridUtil.AxialToCubeCoord(AbilityShapeLocation), 1));

    }

    public void rotateClockwisePlayerCenter()
    {
        transform.rotation *= Quaternion.Euler(0, 60, 0);
        AbilityShapeLocation = HexGridUtil.CubeToAxialCoord(HexGridUtil.RotateRangeClockwise(HexGridUtil.AxialToCubeCoord(PlayerManager.Instance.playerPosition), HexGridUtil.AxialToCubeCoord(AbilityShapeLocation), 1));
    }

    public void rotateCounterClockwisePlayerCenter()
    {
        transform.rotation *= Quaternion.Euler(0, -60, 0);
        AbilityShapeLocation = HexGridUtil.CubeToAxialCoord(HexGridUtil.RotateRangeCounterClockwise(HexGridUtil.AxialToCubeCoord(PlayerManager.Instance.playerPosition), HexGridUtil.AxialToCubeCoord(AbilityShapeLocation), 1));


    }
    public void UsingEffect(Effect effect)
    {
        switch (effect)
        {
            case Effect.Positive:
                switch (gridTile.currentGridState)
                {
                    case GS_positive:
                        gridTile.ChangeCurrentState(GridManager.Instance.gS_Positive);
                        break;
                    case GS_negative:
                        gridTile.ChangeCurrentState(GridManager.Instance.gS_Neutral);
                        break;
                    case GS_BossNegative:
                        gridTile.ChangeCurrentState(GridManager.Instance.gS_Neutral);
                        break;

                    case GS_neutral:
                        gridTile.ChangeCurrentState(GridManager.Instance.gS_Positive);
                        break;

                }
                break;
            case Effect.Negative100:
                if (gridTile.currentGridState.StateValue() < -1)
                {
                    Enemy currentEnemy = gridTile.GetComponentInChildren<Enemy>();
                    if (ability.costs[0] == currentEnemy.ressource)
                    currentEnemy.TakeDamage(2);
                    else
                        currentEnemy.TakeDamage(1);

                }
                break;

            case Effect.Negative200:
                if (gridTile.currentGridState.StateValue() < -1)
                {
                    Enemy currentEnemy = gridTile.GetComponentInChildren<Enemy>();
                    if (ability.costs[0] == currentEnemy.ressource)
                        currentEnemy.TakeDamage(3);

                    else
                        currentEnemy.TakeDamage(2);
                }
                break;

            case Effect.Negative300:
                if (gridTile.currentGridState.StateValue() < -1)
                {
                    Enemy currentEnemy = gridTile.GetComponentInChildren<Enemy>();
                    if (ability.costs[0] == currentEnemy.ressource)
                        currentEnemy.TakeDamage(3);

                    else
                        currentEnemy.TakeDamage(4);
                }
                break;

            case Effect.Negative400:
                if (gridTile.currentGridState.StateValue() < -1)
                {
                    Enemy currentEnemy = gridTile.GetComponentInChildren<Enemy>();
                    if (ability.costs[0] == currentEnemy.ressource)
                    currentEnemy.TakeDamage(5);
                    else
                        currentEnemy.TakeDamage(4);
                }
                break;

            case Effect.Negative500:
                if (gridTile.currentGridState.StateValue() < -1)
                {
                    Enemy currentEnemy = gridTile.GetComponentInChildren<Enemy>();
                    if (ability.costs[0] == currentEnemy.ressource)
                    currentEnemy.TakeDamage(6);

                    else
                        currentEnemy.TakeDamage(5);
                }
                break;

            case Effect.Movement:
                PlayerManager.Instance.playerPosition = gridTile.AxialCoordinate;
                PlayerManager.Instance.player.transform.position = gridTile.transform.position;
                gridTile.ChangeCurrentState(GridManager.Instance.gS_Neutral);
                break;

        }
    }


    public void CastAbility(InputAction.CallbackContext action)
    {
        for (int i = 0; i < AbilityShapeLocation.Count; i++)
        {
            if (GridManager.Instance.Grid.ContainsKey(AbilityShapeLocation[i]))
            {
                gridTile = GridManager.Instance.Grid[AbilityShapeLocation[i]];
                UsingEffect(ability.Effects[i]);
            }
        }
        Payment();
        castAbiltyInputActionReference.action.performed -= CastAbility;
        PlayerManager.Instance.AbilityCasted();
        EventManager.OnAbilityCast();

        Destroy(gameObject);
    }

    public void Payment()
    {
        switch (ability.costs[0])
        {
            case Ressource.ressourceA:
                PlayerManager.Instance.RessourceAInventory -= ability.costs.Count;
                break;

            case Ressource.ressourceB:
                PlayerManager.Instance.RessourceBInventory -= ability.costs.Count;
                break;

            case Ressource.ressourceC:
                PlayerManager.Instance.RessourceCInventory -= ability.costs.Count;
                break;

            case Ressource.resscoureD:
                PlayerManager.Instance.RessourceDInventory -= ability.costs.Count;
                break;
        }
    }

    public void KillYourSelf(InputAction.CallbackContext actionCallBackContext)
    {
        Destroy(gameObject);
    }

}
