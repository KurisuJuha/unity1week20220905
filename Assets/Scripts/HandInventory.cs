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

    public static (Item item, int quantity) IO(Item item, int quantity, bool right)
    {
        if (right)
        {
            if (Inventory.handInventoryQuantity == 0)
            {
                // 3
                int q = quantity;

                quantity = Mathf.FloorToInt(q / 2f);
                Inventory.handInventoryQuantity = q - Mathf.FloorToInt(q / 2f);
                Inventory.handInventory = item;
            }
            else
            {
                // 2
                if (item.id == Inventory.handInventory.id || item.id == 0)
                {
                    item.id = Inventory.handInventory.id;

                    if (quantity < 99)
                    {
                        quantity++;
                        Inventory.handInventoryQuantity--;
                    }
                }
                else
                {
                    // 4
                    Item _item = item;
                    int q = quantity;

                    item = Inventory.handInventory;
                    quantity = Inventory.handInventoryQuantity;
                    Inventory.handInventory = _item;
                    Inventory.handInventoryQuantity = q;
                }
            }
        }
        else
        {
            if (item.id == Inventory.handInventory.id)
            {
                // 5
                quantity += Inventory.handInventoryQuantity;
                if (quantity > 99)
                {
                    Inventory.handInventoryQuantity = quantity - 99;
                    quantity = 99;
                }
                else Inventory.handInventoryQuantity = 0;
            }
            else
            {
                // 1
                Item _item = item;
                int q = quantity;

                item = Inventory.handInventory;
                quantity = Inventory.handInventoryQuantity;
                Inventory.handInventory = _item;
                Inventory.handInventoryQuantity = q;
            }
        }

        return (item, quantity);
    }
}
