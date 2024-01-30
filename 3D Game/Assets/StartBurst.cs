using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBurst : MonoBehaviour
{

    Material BubbleBurst;
    float currentBurstAlpha;

    // Start is called before the first frame update
    void OnEnable()
    {
        BubbleBurst = GetComponent<SkinnedMeshRenderer>().material;
     
        currentBurstAlpha = BubbleBurst.GetFloat("_Bubble");
        GetComponentInParent<Burst>().Bursting(BubbleBurst, currentBurstAlpha);
       
    }

   
}
