using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    public Image image;
    public int index;
    public UINumber uiNumber;
    public GameObject SelectGridObj;

    void Update()
    {
        image.sprite = Utility.GetItemData(Inventory.data[index].id).Sprite;
        uiNumber.number = Inventory.quantity[index];
        SelectGridObj.SetActive(index == Player.HandIndex);
    }

    public void onRightClick()
    {
        (Item item, int quantity) data = HandInventory.IO(Inventory.data[index], Inventory.quantity[index], true);

        Inventory.data[index] = data.item;
        Inventory.quantity[index] = data.quantity;
        return;

        if (Inventory.handInventoryQuantity == 0)
        {
            // 3
            int q = Inventory.quantity[index];

            Inventory.quantity[index] = Mathf.FloorToInt(q / 2f);
            Inventory.handInventoryQuantity = q - Mathf.FloorToInt(q / 2f);
            Inventory.handInventory = Inventory.data[index];
        }
        else
        {
            // 2
            if (Inventory.data[index].id == Inventory.handInventory.id || Inventory.data[index].id == 0)
            {
                Inventory.data[index].id = Inventory.handInventory.id;
                Inventory.quantity[index]++;
                Inventory.handInventoryQuantity--;
            }
            else
            {
                // 4
                Item item = Inventory.data[index];
                int q = Inventory.quantity[index];

                Inventory.data[index] = Inventory.handInventory;
                Inventory.quantity[index] = Inventory.handInventoryQuantity;
                Inventory.handInventory = item;
                Inventory.handInventoryQuantity = q;
            }
        }
    }

    public void onLeftClick()
    {
        (Item item, int quantity) data = HandInventory.IO(Inventory.data[index], Inventory.quantity[index], false);

        Inventory.data[index] = data.item;
        Inventory.quantity[index] = data.quantity;
        return;

        if (Inventory.data[index].id == Inventory.handInventory.id)
        {
            // 5
            Inventory.quantity[index] += Inventory.handInventoryQuantity;
            Inventory.handInventoryQuantity = 0;
        }
        else
        {
            // 1
            Item item = Inventory.data[index];
            int q = Inventory.quantity[index];

            Inventory.data[index] = Inventory.handInventory;
            Inventory.quantity[index] = Inventory.handInventoryQuantity;
            Inventory.handInventory = item;
            Inventory.handInventoryQuantity = q;
        }
    }
}
