using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityObjScript : MonoBehaviour
{
    public Ability ability;

    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    [SerializeField] Material positiveMaterial;
    [SerializeField] Material negativeMaterial;
    [SerializeField] Material movementMaterial;

    [SerializeField] InputActionAsset inputAction;
    [SerializeField] InputActionReference rotationInputAction;


    public void ShowMesh(Ability ability)
    {
        this.ability = ability;
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
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
    }

    public void SetPositionToGridCoord(Vector2Int coord)
    {
        transform.position = GridManager.Instance.Grid[coord].transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        inputAction.Enable();
        rotationInputAction.action.performed += rotateClockwise;
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (rotationInputAction.action.triggered)
        //{
        //    float rotation = rotationInputAction.action.ReadValue<float>();
        //    if (rotation > 0)
        //    {
        //    }
        //    if (rotation < 0)
        //    {
        //        rotateCounterClockwise();
        //    }
        //}
    }

    public void rotateClockwise(InputAction.CallbackContext action)
    {
        float rotation = action.ReadValue<float>();
        if (rotation > 0)
        {
            Debug.Log("I SHould rotate Clockwise");
        }
        if (rotation < 0)
        {
            rotateCounterClockwise();
        }
    }

    public void rotateCounterClockwise()
    {
        Debug.Log("I SHould rotate Counterclockwise");
    }



}
