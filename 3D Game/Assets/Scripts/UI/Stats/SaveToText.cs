using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveToText : MonoBehaviour
{

    public static SaveManager Instance;
    public TextMeshProUGUI Movement;

    public TMP_Text PofICollection;

    public TMP_Text Kills;

    public TMP_Text Damage;

    public TMP_Text Spread;

    public TMP_Text Resources;

    public TMP_Text Heals;
    // Start is called before the first frame update
    void Update()
    {
        Movement.text = PlayerPrefs.GetString("Movement");
        PofICollection.text = PlayerPrefs.GetString("PofIs");
        Kills.text = PlayerPrefs.GetString("Kills");
        Damage.text = PlayerPrefs.GetString("Damage");
        Resources.text = PlayerPrefs.GetString("Resources");
        Heals.text = PlayerPrefs.GetString("Heals");
        Spread.text = PlayerPrefs.GetString("Spread");
    }

}
