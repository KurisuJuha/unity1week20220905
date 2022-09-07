using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Tile
{
    public int id;
    public int damage;
    public Vector2 pos;

    public Tile(Vector2 pos)
    {
        id = 0;
        damage = 0;
        this.pos = pos;
    }

    public void addDamage()
    {
        if (GameManager.Instance.settings.tiles[id].canTakeDamage)
        {
            damage++;

            // ダメージが耐久値以上なら壊す
            if (damage >= GameManager.Instance.settings.tiles[id].dulability) Destroy();
        }
    }

    public void Destroy()
    {
        this = new Tile(pos);

        MapData.UpdateTile(Utility.GetChunkPos(pos), Utility.GetTilePos(pos));
    }
}