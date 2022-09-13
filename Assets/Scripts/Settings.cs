using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Settings : ScriptableObject
{
    public TileData[] tiles;
    public ItemData[] items;
    public Sprite[] numbers;
    public Card[] cards;
    public float recoveryPerTick;
    public float dropItemIgnoreTime;
    public AnimationCurve MaxPointCurve;
}
