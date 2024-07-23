using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeGridHex : MonoBehaviour
{
    public Image image;

    [SerializeField] public Vector2Int coordinate;

    [SerializeField] TMP_Text text;

    public Effect effect;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Fill(Sprite sprite)
    {
        //imageSprite = sprite;
        image.sprite = sprite;
    }

    public void Fill(Sprite sprite,string text)
    {
        //imageSprite = sprite;
        image.sprite = sprite;
        this.text.text = text;
    }

    private void OnDestroy()
    {
      //  Debug.Log("I dont know why but im beeing destroyed");
    }
}
