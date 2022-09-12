using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class MapData
{
    private static Dictionary<Vector2Int, Chunk>[] map = new Dictionary<Vector2Int, Chunk>[2];
    private static Dimension CurrentDimension;

    /// <summary>
    /// 指定されたチャンクにデータがある場合それを返し、
    /// 無い場合はチャンクを生成してそれを返す。
    /// </summary>
    public static Chunk GetChunk(Vector2Int chunkPos) => GetChunk(chunkPos, CurrentDimension);

    /// <summary>
    /// 指定されたチャンクにデータがある場合それを返し、
    /// 無い場合はチャンクを生成してそれを返す。
    /// </summary>
    public static Chunk GetChunk(Vector2Int chunkPos, Dimension dimension)
    {
        if (map[(int)dimension] == null) map[(int)dimension] = new Dictionary<Vector2Int, Chunk>();

        if (!map[(int)dimension].TryGetValue(chunkPos, out Chunk chunk))
        {
            chunk = new Chunk(chunkPos);
            map[(int)dimension].Add(chunkPos, chunk);
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
    public static void SetTile(Vector2 pos, Item item) => SetTile(pos, Utility.GetItemData(item.id).tileID);

    /// <summary>
    /// 指定された位置に指定されたidのtileを設置する
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="tileid"></param>
    public static void SetTile(Vector2 pos, int tileid)
    {
        Chunk chunk = GetChunk(Utility.GetChunkPos(pos));
        Vector2Int tilepos = Utility.GetTilePos(pos);

        chunk.tiles[tilepos.x, tilepos.y] = new Tile(pos) { id = tileid };
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

    /// <summary>
    /// ディメンションを変更する
    /// 変更後にすべてのチャンクの描画を更新する
    /// </summary>
    /// <param name="dimension"></param>
    public static void ChangeDimension(Dimension dimension)
    {
        Vector2Int[] keys = map[(int)CurrentDimension].Keys.ToArray();
        // 古いDropItemの描画を消す
        DropItemManager.Instance.SetActive(false);

        CurrentDimension = dimension;

        // 描画を更新
        foreach (var chunkpos in keys)
        {
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    UpdateTile(chunkpos, new Vector2Int(x, y));
                }
            }
        }

        // 新しいDropItemを描画する
        DropItemManager.Instance.SetActive(true);
    }

    /// <summary>
    /// 現在のディメンションを取得する
    /// </summary>
    /// <returns></returns>
    public static Dimension GetDimension()
    {
        return CurrentDimension;
    }
}

public enum Dimension
{
    Ground,
    UnderGround,
}
