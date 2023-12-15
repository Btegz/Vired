using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraDropDown : MonoBehaviour
{
    /*[HideInInspector]*/ public TMP_Dropdown dropdown;

    public void HandleInputData(int va1)
    {
        if(va1 == 0)
        {
            CameraRotation.Instance.SwitchtoMain();
        }

        if (va1 == 1)
        {
            CameraRotation.Instance.SwitchToTopDown();
        }

        //if (va1 == 2)
        //{
        //    CameraRotation.Instance.SwitchToPlayer();
        //}
    }

    //public void Awake()
    //{
    //    dropdown = GetComponentInChildren<TMP_Dropdown>();
    //}
}
