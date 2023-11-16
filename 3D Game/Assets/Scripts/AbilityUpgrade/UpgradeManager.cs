using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] TMP_Text skillPointsNumber;

    [SerializeField] PlayerButton playerButtonPrefab;
    [SerializeField] HorizontalLayoutGroup PlayerButtonParent;


    // Start is called before the first frame update
    void Start()
    {
        if (PlayerManager.Instance != null)
        {
            foreach (Player player in PlayerManager.Instance.Players)
            {
                PlayerButton playerButton = Instantiate(playerButtonPrefab, PlayerButtonParent.transform);
                playerButton.Setup(player);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
