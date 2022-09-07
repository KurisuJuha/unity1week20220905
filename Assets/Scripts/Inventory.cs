using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Inventory
{
    public static Item[] data = new Item[9];
    public static int[] am = new int[9];

    public static Item GetItem(int index)
    {
        while (index < 0)
        {
            index += 9;
        }
        index %= 9;

        return data[index];
    }
}
