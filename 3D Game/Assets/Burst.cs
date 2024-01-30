using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burst : MonoBehaviour
{
    Material BubbleBurstInner;
    Material BubbleBurstOutside;
    float currentBurstAlpha;
    public float rate; 
    public float secondsToWait;
    void Start()
    {





    }

    void Update()
    {
        
    }

    public IEnumerator BubblyBurst(Material BubbleBurst, float current)
    {
        while (currentBurstAlpha < 20)
        {
            BubbleBurst.SetFloat("_Bubble", currentBurstAlpha + rate);
            yield return new WaitForSeconds(secondsToWait);
            currentBurstAlpha = BubbleBurst.GetFloat("_Bubble");
        }

    }


    public void Bursting(Material BubbleBurst, float current)
    {
        StartCoroutine(BubblyBurst(BubbleBurst, current));
    }
}
