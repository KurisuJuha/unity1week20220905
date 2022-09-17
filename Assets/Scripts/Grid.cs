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
    }

    public void onLeftClick()
    {
        (Item item, int quantity) data = HandInventory.IO(Inventory.data[index], Inventory.quantity[index], false);

        Inventory.data[index] = data.item;
        Inventory.quantity[index] = data.quantity;
        return;
    }
}
