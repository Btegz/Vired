using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Typewriter : MonoBehaviour
{
    private TextMeshProUGUI txt;
    private string story;
    public float time;
    public GameObject TextBox;


    // Start is called before the first frame update
    void OnEnable()
    {
        txt = GetComponent<TextMeshProUGUI>();
       
        story = txt.text;
        txt.text = "";
        StartCoroutine(TypewriterFunction());

    }


    public IEnumerator TypewriterFunction()
    {
        IEnumerator textBox = TutorialManager.Instance.Flicker(TextBox);
        StartCoroutine(textBox);
        foreach(char c in story)
        {
            txt.text += c;
            yield return new WaitForSeconds(time); 
            while(true)
            {
                TutorialManager.Instance.Flicker(TextBox);
                break;
            }
        }
        StopCoroutine(textBox);

        
        
    }
}
