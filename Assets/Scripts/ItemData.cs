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
    public CraftMaterial[] Materials;
}

[System.Serializable]
public struct CraftMaterial
{
    public int id;
    public int quantity;
}
