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
}
