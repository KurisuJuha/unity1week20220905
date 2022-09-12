﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Item
{
    public int id;

    public Tile ToTile(Vector2 pos)
    {
        Tile tile = new Tile(pos);

        tile.id = GameManager.Instance.settings.items[id].tileID;

        return tile;
    }
}