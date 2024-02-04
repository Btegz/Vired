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
        //enemyMassChildren = new List<Material>();
        //foreach (MeshRenderer rend in GetComponentsInChildren<MeshRenderer>())
        //{
        //    enemyMassChildren.Add(rend.material);
        //}

  

     
       

    }


    public IEnumerator BubblyBurst()
    {
        


        try
        {
            Sequence burstSequence = DOTween.Sequence();
            burstSequence.Append(transform.DOScale(transform.localScale - Vector3.one * 0.5f, .25f).SetEase(Ease.OutExpo).OnComplete(() => Instantiate(BubbleBurstParticle, gameObject.transform.position, Quaternion.Euler(Vector3.zero))));
            burstSequence.Append(transform.DOScale(transform.localScale + Vector3.one * 0.5f, .5f).SetEase(Ease.OutExpo));
            burstSequence.OnComplete(() => Destroy(gameObject));
            burstSequence.Play();
        }
        catch
        {

        }
        
        
        //OnComplete(() => Destroy(gameObject));


        //while (true)
        //{
        //    foreach (Material component in enemyMassChildren)
        //    {
        //        component.SetFloat("_Bubble", component.GetFloat("_Bubble") + rate);

        //    }
        //    if(enemyMassChildren[0].GetFloat("_Bubble") >20)
        //    { 
        //        break;
        //    }
        //    yield return new WaitForSeconds(secondsToWait);
        //}
        //Destroy(gameObject);
        yield return null;
    }


    public void Bursting()
    {
        StartCoroutine(BubblyBurst());

    }

    public void OnDestroy()
    {
      
    }
}
