using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StartBurst : MonoBehaviour
{

    Material BubbleBurst;
    float currentBurstAlpha;
    public Burst Bubble;

    // Start is called before the first frame update
    void OnEnable()
    {
        BubbleBurst = GetComponent<SkinnedMeshRenderer>().material;
     
        currentBurstAlpha = BubbleBurst.GetFloat("_Bubble");
       
    }

   /* private void BurstYourBubble()
    {
        Bubble.Bursting(BubbleBurst, currentBurstAlpha);
    }
   */



}
