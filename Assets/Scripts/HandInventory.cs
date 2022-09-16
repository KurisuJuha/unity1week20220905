using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandInventory : SingletonMonoBehaviour<HandInventory>
{
    public UINumber uinumber;
    public Image image;
    public RectTransform rectTransform;

    void Start()
    {

    }

    void Update()
    {
        uinumber.number = Inventory.handInventoryQuantity;
        image.sprite = GameManager.Instance.settings.items[Inventory.handInventory.id].Sprite;
        rectTransform.position = Input.mousePosition;
    }

    public static void SetActive(bool active)
    {
        Instance.gameObject.SetActive(active);
    }
}
