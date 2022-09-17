using UnityEngine;
using UnityEngine.UI;

public class TameruGrid : MonoBehaviour
{
    public Image image;
    public int id;
    public int quantity;
    public UINumber uiNumber;
    public float alpha;

    void Update()
    {
        image.sprite = Utility.GetItemData(id).Sprite;
        if (quantity == 0) alpha = 0.5f;
        else alpha = 1;
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
        uiNumber.number = quantity;
    }

    public void onRightClick()
    {
        if (Inventory.handInventory.id == id || Inventory.handInventory.id == 0)
        {
            (Item item, int quantity) data = HandInventory.IO(new Item() { id = id }, quantity, true, 999);

            quantity = data.quantity;
            return;
        }
    }

    public void onLeftClick()
    {
        if (Inventory.handInventory.id == id || Inventory.handInventory.id == 0)
        {
            (Item item, int quantity) data = HandInventory.IO(new Item() { id = id }, quantity, false, 999);

            quantity = data.quantity;
            return;
        }
    }
}
