using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveToText : MonoBehaviour
{

    public static SaveManager Instance;
    public TextMeshProUGUI Movement;

    public TextMeshProUGUI PofICollection;

    public TextMeshProUGUI Kills;

    public TextMeshProUGUI Damage;

    public TextMeshProUGUI Spread;

    public TextMeshProUGUI Resources;

    public TextMeshProUGUI Heals;
    // Start is called before the first frame update
    void Update()
    {

        Debug.Log(PlayerPrefs.GetString("Movement"));
        Movement.text = PlayerPrefs.GetString("Movement");
        PofICollection.text = PlayerPrefs.GetString("PofIs");
        Kills.text = PlayerPrefs.GetString("Kills");
        Damage.text = PlayerPrefs.GetString("Damage");
        Resources.text = PlayerPrefs.GetString("Resources");
        Heals.text = PlayerPrefs.GetString("Heals");
        Spread.text = PlayerPrefs.GetString("Spread");
    }

    
}
