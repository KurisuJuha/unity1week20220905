using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Chunk
{
    public Tile[,] tiles;
    public Vector2Int pos;
    public Dimension dimension;

    public Chunk(Vector2Int pos, Dimension dimension, bool redraw = true)
    {
        tiles = new Tile[10, 10];
        this.pos = pos;
        this.dimension = dimension;

        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                Vector2 p = new Vector2(this.pos.x * 10 + x, this.pos.y * 10 + y);

                switch (dimension)
                {
                    case Dimension.Ground:
                        int a = Random.Range(0, 2);
                        int id = 0;

                        if (Random.Range(0, 10) == 0)
                        {
                            switch (a)
                            {
                                case 0:
                                    id = 1;
                                    break;
                                case 1:
                                    id = 5;
                                    break;
                            }
                        }
                        tiles[x, y] = new Tile(p) { id = id };
                        break;
                    case Dimension.UnderGround:
                        int a2 = Random.Range(0, 10);
                        int id2 = 7;

                        if (Random.Range(0, 10) == 0)
                        {
                            switch (a2)
                            {
                                case 0:
                                case 1:
                                case 2:
                                case 3:
                                    id2 = 7;
                                    break;
                                case 4:
                                case 5:
                                case 6:
                                    id2 = 8;
                                    break;
                                case 7:
                                case 8:
                                    id2 = 9;
                                    break;
                                case 9:
                                    id2 = 10;
                                    break;
                            }
                        }
                        tiles[x, y] = new Tile(p) { id = id2 };
                        break;
                }
            }
        }

        if (redraw)
        {
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    int tileid = tiles[x, y].id;
                    TileBase tile = GameManager.Instance.settings.tiles[tileid].Tile;
                    Tilemap tilemap = GameManager.Instance.tilemap;
                    tilemap.SetTile((Vector3Int)(pos * 10 + new Vector2Int(x, y)), tile);
                }
            }
        }
    }
}
