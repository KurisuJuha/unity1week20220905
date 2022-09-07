using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public Tile[,] tiles;
    public Vector2Int pos;

    public Chunk(Vector2Int pos)
    {
        tiles = new Tile[10, 10];
        this.pos = pos;

        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                Vector2 p = new Vector2(this.pos.x * 10 + x, this.pos.y * 10 + y);
                tiles[x, y] = new Tile(p);
            }
        }
    }
}
