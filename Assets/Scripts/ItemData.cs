using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public string Name;
    public Sprite Sprite;
    public bool CanPlace;
    public int tileID;
    public int AttackPower;
    public bool Can_CraftBench;
    public bool Can_Furnace;
    public bool Can_HandCraft;
    public CraftMaterial[] Materials;
    public bool Can_AddPoint;
    public int point;
}

[System.Serializable]
public struct CraftMaterial
{
    public int id;
    public int quantity;
}
