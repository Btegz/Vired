using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityObjScript : MonoBehaviour
{
    public Ability ability;

    public List<Vector2Int> AbilityShapeLocation;

    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    [SerializeField] Material positiveMaterial;
    [SerializeField] Material negativeMaterial;
    [SerializeField] Material movementMaterial;

    [SerializeField] InputActionAsset inputAction;
    [SerializeField] InputActionReference rotationInputAction;

    Camera cam;

    public void ShowMesh(Ability ability,Vector3Int SpawnPoint, Vector3Int playerPos)
    {
        this.ability = ability;
        AbilityShapeLocation = ability.Coordinates;
        Debug.Log(AbilityShapeLocation[0]);
        AbilityShapeLocation = HexGridUtil.CubeToAxialCoord(HexGridUtil.CubeAddRange(HexGridUtil.AxialToCubeCoord(AbilityShapeLocation), playerPos));





        meshFilter = GetComponentInChildren<MeshFilter>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        meshFilter.mesh = ability.previewShape;
        Material[] materials = new Material[ability.Effects.Count];
        for(int i = 0; i< ability.Effects.Count; i++)
        {
            switch (ability.Effects[i])
            {
                case Effect.Positive:
                    materials[i] = positiveMaterial;
                    break;
                case Effect.Negative:
                    materials[i] = negativeMaterial;
                    break;
                case Effect.Movement:
                    materials[i] = movementMaterial;
                    break;
            }
        }
        meshRenderer.materials = materials;

        // selected point nachbarfelder in cubeVector

        // neighbor Vector 

        Vector3Int selectedDirection = HexGridUtil.CubeSubstract(SpawnPoint, playerPos);

        Vector3Int[] lol = HexGridUtil.cubeDirectionVectors;

        List<Vector3Int> dumm = new List<Vector3Int>();
        foreach(Vector3Int i in lol)
        {
            dumm.Add(i);
        }

        AbilityShapeLocation = HexGridUtil.CubeToAxialCoord(HexGridUtil.RotateRangeClockwise(playerPos, HexGridUtil.AxialToCubeCoord(AbilityShapeLocation), dumm.IndexOf(selectedDirection)));

        Debug.Log($"PlayerPos: {playerPos}, SpawnPoint: {SpawnPoint}, AbilityShapeLoc[0]:{AbilityShapeLocation[0]}");

        transform.rotation *= Quaternion.Euler(0, dumm.IndexOf(selectedDirection) * 60, 0);




        SetPositionToGridCoord(HexGridUtil.CubeToAxialCoord(playerPos));
        //transform.position =  GridManager.Instance.Grid[HexGridUtil.CubeToAxialCoord(SpawnPoint)].transform.position;
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
        rotationInputAction.action.performed += rotateAbility;

        //just for attemps
        //ShowMesh(ability);
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
            rotateClockwise();
        }
        if (rotation < 0)
        {
            rotateCounterClockwise();
        }
    }

    public void rotateClockwise()
    {
        transform.rotation *= Quaternion.Euler(0, 60, 0);

    }

    public void rotateCounterClockwise()
    {
        transform.rotation *= Quaternion.Euler(0, -60, 0);
    }


    public void CastAbility()
    {
        PlayerManager.Instance.AbilityCasted();
    }
}
