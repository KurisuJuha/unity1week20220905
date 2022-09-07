using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TileData
{
    public string Name;
    public TileBase Tile;
    public bool canTakeDamage;
    public int dulability;
}
