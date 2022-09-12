using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Tile
{
    public int id;
    public int damage { get; private set; }
    public Guid guid;
    public Vector2 pos;

    public Tile(Vector2 pos)
    {
        id = 0;
        damage = 0;
        guid = Guid.NewGuid();
        this.pos = pos;
    }

    public void addDamage() => addDamage(1);

    public void addDamage(int power)
    {
        if (GameManager.Instance.settings.tiles[id].canTakeDamage)
        {
            damage += power;

            // 初回ダメージ
            if (damage == power)
            {
                TickManager.AddJob(Recovery(guid));
            }


            // ダメージが耐久値以上なら壊す
            if (damage >= GameManager.Instance.settings.tiles[id].dulability) Destroy();
            if (damage < 0) damage = 0;
        }
    }

    /// <summary>
    /// Emptyにする
    /// </summary>
    public void Destroy()
    {
        GameManager.Instance.settings.tiles[id].dropItems.Drop();
        this = new Tile(pos);

        MapData.UpdateTile(Utility.GetChunkPos(pos), Utility.GetTilePos(pos));
    }

    /// <summary>
    /// 回復と表示
    /// </summary>
    public IEnumerator Recovery(Guid guid)
    {
        Vector2Int chunkpos = Utility.GetChunkPos(pos);
        Vector2Int tilepos = Utility.GetTilePos(pos); 
        Tile tile = MapData.GetChunk(chunkpos).tiles[tilepos.x, tilepos.y]; // この辺定義しなくても同じ構造体内だからいけそうだけど、structだから余計なコピーとか発生してそうでなんかこわいからやめておく
        TileData data = GameManager.Instance.settings.tiles[tile.id];
        int dulability = data.dulability;
        Gauge gauge = GameObject.Instantiate(GameManager.Instance.GaugePrefab).GetComponent<Gauge>();
        gauge.transform.position = (Vector2)(chunkpos * 10 + tilepos) + new Vector2(0.5f, 1f);

        float r = 0;

        while (true)
        {
            // 現在のタイルが生成された時のタイルと違う場合他の処理をする前にbreakしておく
            if (!tile.guid.Equals(guid)) break;

            // 回復値をためておく
            r += GameManager.Instance.settings.recoveryPerTick;

            // ある程度ためておいた回復値をタイルのダメージから引く
            if (r >= 1)
            {
                MapData.GetChunk(chunkpos).tiles[tilepos.x, tilepos.y].addDamage(-1);
                r -= 1;
            }

            tile = MapData.GetChunk(chunkpos).tiles[tilepos.x, tilepos.y];
            gauge.Range = 1 - tile.damage / (float)dulability;

            // ダメージが0なら表示する必要がないためbreakする
            if (tile.damage == 0) break;

            yield return null;
        }

        GameObject.Destroy(gauge.gameObject);
    }
}
