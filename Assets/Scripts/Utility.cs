using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{
    public static Vector2Int GetChunkPos(Vector2 pos)
    {
        Vector2Int ret = new Vector2Int();

        ret.x = Mathf.FloorToInt(pos.x / 10f);
        ret.y = Mathf.FloorToInt(pos.y / 10f);

        return ret;
    }

    public static Vector2Int GetTilePos(Vector2 pos)
    {
        Vector2Int ret = new Vector2Int();
        Vector2Int chunkpos = GetChunkPos(pos);

        ret.x = Mathf.FloorToInt(pos.x) - chunkpos.x * 10;
        ret.y = Mathf.FloorToInt(pos.y) - chunkpos.y * 10;

        return ret;
    }

    public static ItemData GetItemData(int id)
    {
        return GameManager.Instance.settings.items[id];
    }
}