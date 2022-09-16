using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftBench : MonoBehaviour
{
    public CraftGrid[] craftGrids;
    public GameObject LeftArrow;
    public GameObject RightArrow;
    public GameObject SelectCraftGrid;
    public CameraShake shake;

    public List<int> items;
    public int page;
    public int select;

    public static CraftBenchMode mode;

    public bool left;
    public bool right;

    void Start()
    {

    }

    void Update()
    {
        int length = craftGrids.Length;

        items = new List<int>();
        for (int i = 0; i < GameManager.Instance.settings.items.Length; i++)
        {
            switch (mode)
            {
                case CraftBenchMode.CraftBench:
                    if (Utility.GetItemData(i).Can_CraftBench) items.Add(i);
                    break;
                case CraftBenchMode.Furnace:
                    if (Utility.GetItemData(i).Can_Furnace) items.Add(i);
                    break;
                case CraftBenchMode.HandCraft:
                    if (Utility.GetItemData(i).Can_HandCraft) items.Add(i);
                    break;
                default:
                    break;
            }
        }

        for (int i = 0; i < length; i++)
        {
            int index = i + page * length;
            if (index < items.Count)
            {
                craftGrids[i].gameObject.SetActive(true);
                craftGrids[i].id = items[index];
            }
            else
            {
                craftGrids[i].gameObject.SetActive(false);
            }
        }
        left = page > 0;
        right = page < (items.Count - 1) / length;

        if (select < 0) select = 0;
        if (select >= items.Count) select = items.Count - 1;
        page = select / craftGrids.Length;

        SelectCraftGrid.transform.position = craftGrids[select % length].transform.position;
    }

    public void LateUpdate()
    {
        LeftArrow.SetActive(left);
        RightArrow.SetActive(right);
    }

    public void TryCraft()
    {
        int id = items[select];
        CraftMaterial[] craftmaterials = Utility.GetItemData(id).Materials;
        Item[] playerinventoryitems = Inventory.data;
        int[] playerinventoryquantity = Inventory.quantity;

        // 作れるかどうか判定
        bool cancraft = true;
        for (int i = 0; i < craftmaterials.Length; i++)
        {
            CraftMaterial craftMaterial = craftmaterials[i];
            cancraft = cancraft && Inventory.GetItem(craftMaterial.id, craftMaterial.quantity);
        }

        // 作れるなら素材をインベントリから引く
        if (cancraft)
        {
            for (int i = 0; i < craftmaterials.Length; i++)
            {
                CraftMaterial craftMaterial = craftmaterials[i];
                Inventory.SubItem(craftMaterial.id, craftMaterial.quantity);
            }

            // インベントリに出来たものを足す
            Item item = new Item() { id = id };
            if (!Inventory.AddItem(item))
            {

                DropItemManager.Instance.NewDropItem(item, 1, Player.PlayerPos);
            }

            // 画面を揺らす
            shake.Shake();

            AudioManager.Play(AudioType.Success);
            return;
        }

        AudioManager.Play(AudioType.Failure);
    }

    public void AddPageIndex() => select += craftGrids.Length;

    public void SubPageIndex() => select -= craftGrids.Length;

    public void SetSelectIndex(int index)
    {
        int p = select / craftGrids.Length;
        select = p * craftGrids.Length + index;
    }
}

public enum CraftBenchMode
{
    CraftBench,
    Furnace,
    HandCraft,
}