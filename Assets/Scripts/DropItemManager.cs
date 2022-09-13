using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemManager : SingletonMonoBehaviour<DropItemManager>
{
    public List<DropItem>[] dropItems = new List<DropItem>[2];
    public GameObject DropItemPrefab;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void NewDropItem(Item item, int quantity, Vector2 pos)
    {
        if (dropItems[(int)MapData.GetDimension()] == null) dropItems[(int)MapData.GetDimension()] = new List<DropItem>();

        // 近くに同じidのアイテムがある場合はそっちに数を足して新しいdropitemは生成しない
        foreach (var drop in dropItems[(int)MapData.GetDimension()])
        {
            if (drop.item.id == item.id
                && (drop.transform.position - (Vector3)pos).magnitude < 2f)
            {
                drop.quantity += quantity;
                drop.transform.position = (drop.transform.position + (Vector3)pos) / 2f;
                return;
            }
        }

        DropItem dropItem = GameObject.Instantiate(DropItemPrefab, pos, Quaternion.identity).GetComponent<DropItem>();
        dropItem.item = item;
        dropItem.quantity = quantity;

        dropItems[(int)MapData.GetDimension()].Add(dropItem);
    }

    public void DestroyDropItem(DropItem dropItem)
    {
        if (dropItems[(int)MapData.GetDimension()] == null) dropItems[(int)MapData.GetDimension()] = new List<DropItem>();

        dropItems[(int)MapData.GetDimension()].Remove(dropItem);
        Destroy(dropItem.gameObject);
    }

    public void SetActive(bool b)
    {
        if (dropItems[(int)MapData.GetDimension()] == null) dropItems[(int)MapData.GetDimension()] = new List<DropItem>();

        foreach (var item in dropItems[(int)MapData.GetDimension()])
        {
            item.gameObject.SetActive(b);
        }
    }
}
