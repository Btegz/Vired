using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillPointsScript : MonoBehaviour
{
    TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = PlayerManager.Instance.SkillPoints.ToString();
    }
}
