using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class HealthPontsBillBoard : MonoBehaviour
{
    public GameObject mainCamera;


    [SerializeField] private BillboardType billboardType;

    [Header("Lock Rotation")]
    [SerializeField] private bool lockX;
    [SerializeField] private bool lockY;
    [SerializeField] private bool lockZ;

    private Vector3 originalRotation;

    public enum BillboardType { LookAtCamera, CameraForward };

    private void Awake()
    {
        originalRotation = transform.rotation.eulerAngles;
    }

    void LateUpdate()
    {
        switch (billboardType)
        {
            case BillboardType.LookAtCamera:
                
                if(CameraRotation.Instance.TopDownCam.Priority != 2)
                transform.LookAt(Camera.main.transform.position, Vector3.up);
                


                else
                    transform.rotation = Camera.main.transform.rotation;
                break;
                    
                
            case BillboardType.CameraForward:
                if (CameraRotation.Instance.TopDownCam.Priority != 2)
                    transform.forward = Camera.main.transform.forward;


                else
                    transform.forward = mainCamera.transform.GetChild(3).transform.forward; 
                    break;

            default:
                break;
        }
        // Modify the rotation in Euler space to lock certain dimensions.
        Vector3 rotation = transform.rotation.eulerAngles;
        if (lockX) { rotation.x = originalRotation.x; }
        if (lockY) { rotation.y = originalRotation.y; }
        if (lockZ) { rotation.z = originalRotation.z; }
        transform.rotation = Quaternion.Euler(rotation);

    }
}
