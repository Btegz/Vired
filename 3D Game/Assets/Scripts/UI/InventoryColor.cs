using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryColor : MonoBehaviour
{

    [SerializeField] List<Sprite> backgroundsInv;

    private Image image;


    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        EventManager.OnSelectPlayerEvent += SwitchSprite;
    }

    private void SwitchSprite(Player player)
    {
        int inti = PlayerManager.Instance.Players.IndexOf(player);
        image.sprite = backgroundsInv[inti];
    }

    private void OnDestroy()
    {
        EventManager.OnSelectPlayerEvent -= SwitchSprite;
    }
}
