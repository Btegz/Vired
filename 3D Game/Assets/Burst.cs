using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class Burst : MonoBehaviour
{

    float currentOuterBurstAlpha;
    public float rate;
    public float secondsToWait;
    public List<Material> enemyMassChildren;
    public ParticleSystem BubbleBurstParticle;
    private Material EnemyMass; 
    void OnEnable()
    {
        enemyMassChildren = new List<Material>();
        foreach (SkinnedMeshRenderer rend in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            enemyMassChildren.Add(rend.material);
        }

  

     
       

    }


    public IEnumerator BubblyBurst()
    {
        
        transform.DOScale(0.5f, 2f);

        while (true)
        {
            foreach (Material component in enemyMassChildren)
            {
                component.SetFloat("_Bubble", component.GetFloat("_Bubble") + rate);

            }
            if(enemyMassChildren[0].GetFloat("_Bubble") >20)
            { 
                break;
            }
            yield return new WaitForSeconds(secondsToWait);
        }
        try
        {
            Instantiate(BubbleBurstParticle, gameObject.transform.position, Quaternion.identity);
              foreach (Material component in enemyMassChildren) 
           component.SetFloat("_Bubble", -20);

            Destroy(gameObject);

        }

        catch
        {
            Debug.Log("I JUST COULDNT HANDLE DESTORYING MY EnemyMass IM SORRY");

        }
    }


    public void Bursting()
    {
        StartCoroutine(BubblyBurst());

    }

    public void OnDestroy()
    {
      
    }
}
