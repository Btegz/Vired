using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCastButton : MonoBehaviour
{
    [SerializeField] int index;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(clicked);
    }

    public void clicked()
    {
        EventManager.OnAbilityButtonClicked(index);
    }

    private void OnDestroy()
    {
        GetComponent<Button>().onClick.RemoveListener(clicked);
    }
}
