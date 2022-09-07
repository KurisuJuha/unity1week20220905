using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class MapData
{
    public static Dictionary<Vector2Int, Chunk> data = new Dictionary<Vector2Int, Chunk>();

    /// <summary>
    /// 指定されたチャンクにデータがある場合それを返し、
    /// 無い場合はチャンクを生成してそれを返す。
    /// </summary>
    public static Chunk GetChunk(Vector2Int chunkPos)
    {
        if (!data.TryGetValue(chunkPos, out Chunk chunk))
        {
            chunk = new Chunk(chunkPos);
            data.Add(chunkPos, chunk);
        }

        return chunk;
    }

    /// <summary>
    /// 指定されたチャンクの描画を更新する
    /// </summary>
    public static void UpdateChunk(Vector2Int chunkPos)
    {
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                UpdateTile(chunkPos, new Vector2Int(x, y));
            }
        }
    }

    /// <summary>
    /// 指定されたタイルの描画を更新する
    /// </summary>
    public static void UpdateTile(Vector2Int chunkPos, Vector2Int tilePos)
    {
        Chunk chunk = GetChunk(chunkPos);
        int tileid = chunk.tiles[tilePos.x, tilePos.y].id;
        TileBase tile = GameManager.Instance.settings.tiles[tileid].Tile;
        Tilemap tilemap = GameManager.Instance.tilemap;
        tilemap.SetTile((Vector3Int)(chunkPos * 10 + tilePos), tile);
    }

    /// <summary>
    /// 指定された位置に指定されたitemを設置する
    /// </summary>
    public static void SetTile(Vector2 pos, Item item)
    {
        Chunk chunk = GetChunk(Utility.GetChunkPos(pos));
        Vector2Int tilepos = Utility.GetTilePos(pos);

        chunk.tiles[tilepos.x, tilepos.y] = item.ToTile(pos);
        UpdateTile(Utility.GetChunkPos(pos), Utility.GetTilePos(pos));
    }

    /// <summary>
    /// 指定された位置のタイルを破壊する
    /// </summary>
    public static void Destroy(Vector2 pos)
    {
        Chunk chunk = GetChunk(Utility.GetChunkPos(pos));
        Vector2Int tilepos = Utility.GetTilePos(pos);

        chunk.tiles[tilepos.x, tilepos.y].Destroy();
        UpdateTile(Utility.GetChunkPos(pos), Utility.GetTilePos(pos));
    }
}
