using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Inventory
{
    public static Item[] data = new Item[9];
    public static int[] quantity = new int[9];

    public static bool GetItem(int id, int quantity, bool take = false)
    {
        int q = 0;

        for (int i = 0; i < data.Length; i++)
        {
            if (data[i].id == id) q += Inventory.quantity[i];

        }

        if (q < quantity) return false;

        if (take)
        {
            SubItem(id, q);
        }

        return true;
    }

    public static void SubItem(int id, int q)
    {
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i].id == id)
            {
                if (quantity[i] <= q)
                {
                    q -= quantity[i];
                    quantity[i] = 0;
                    data[i] = new Item() { id = 0 };
                }
                else
                {
                    quantity[i] -= q;
                    q = 0;
                }
            }
        }
    }

    public static Item PeekItem(int index)
    {
        while (index < 0)
        {
            index += 9;
        }
        index %= 9;

        return data[index];
    }

    public static bool AddItem(Item item)
    {
        for (int i = 0; i < 9; i++)
        {
            // 同じidのアイテムがあった場合
            if (data[i].id == item.id
                && quantity[i] < 99)
            {
                quantity[i]++;
                return true;
            }
        }

        for (int i = 0; i < 9; i++)
        {
            // Emptyアイテムだった場合
            if (data[i].id == 0)
            {
                data[i] = item;
                quantity[i]++;
                return true;
            }
        }

        return false;
    }

    public static (bool, int) AddItem(Item item, int quantity)
    {
        bool retbool = true;
        int retint = 0;

        for (int i = 0; i < quantity; i++)
        {
            if (!AddItem(item))
            {
                retbool = false;
                retint++;
            }
        }

        return (retbool, retint);
    }
}
