using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialGrid : MonoBehaviour
{
    public UINumber uinumber;
    public Image image;
    public int id;
    public int quantity;

    void Start()
    {
        
    }

    void Update()
    {
        uinumber.number = quantity;
        image.sprite = GameManager.Instance.settings.items[id].Sprite;
    }
}
