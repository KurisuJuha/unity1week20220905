using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DropData
{
    public int id;
    public int min;
    public int max;
    public int threshold;

    public (bool drop ,int id, int quantity) Drop()
    {
        (bool drop, int id, int quantity) ret;

        int a = Random.Range(min, Mathf.CeilToInt(max * SkillManager.Instance.card_luck) + 1);

        ret.drop = a > threshold;
        ret.id = id;
        ret.quantity = a - threshold;

        return ret;
    }
}

public static class DropDataEx
{
    public static void Drop(this DropData[] data)
    {
        List<(int id, int quantity)> drops = new List<(int id, int quantity)>();

        // 実際にドロップするアイテムのリストを作る
        for (int i = 0; i < data.Length; i++)
        {
            var d = data[i].Drop();

            if (d.drop)
            {
                drops.Add((d.id, d.quantity));
            }
        }

        // ドロップする前にプレイヤーのインベントリに直接入れられるものは入れて、入れられなかったもので新しいリストを作成する
        List<(int id, int quantity)> drops2 = new List<(int id, int quantity)>();
        foreach (var drop in drops)
        {
            var d = Inventory.AddItem(new Item() { id = drop.id }, drop.quantity);

            if (!d.Item1)
            {
                drops2.Add((drop.id, d.Item2));
            }
        }

        // ドロップする
        foreach (var drop in drops2)
        {
            DropItemManager.Instance.NewDropItem(new Item() { id = drop.id }, drop.quantity, Player.PlayerPos);
        }
    }
}
