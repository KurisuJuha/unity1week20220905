using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    public Image image;
    public int index;
    public UINumber uiNumber;

    void Update()
    {
        image.sprite = Utility.GetItemData(Inventory.data[index].id).Sprite;
        uiNumber.number = Inventory.am[index];
    }
}
