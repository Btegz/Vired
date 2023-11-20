using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public List<Boss> BossList;

    public void Start()
    {
        Instantiate(BossList[0]);

    }
}