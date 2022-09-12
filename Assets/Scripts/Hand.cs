using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool flip;
    public Player player;
    public float per;
    public AnimationCurve curve;
    public SpriteRenderer sr;
    public int layer;
    public Vector2 startPos;
    public Vector2 endPos;

    [Header("param")]
    public float bias;
    public float size;

    void Start()
    {
        transform.localPosition = startPos;
        sr.sprite = Utility.GetItemData(Inventory.data[Player.HandIndex].id).Sprite;
        transform.localScale = new Vector3(-1, 1, 1);
    }

    void Update()
    {
        per = player.HandElapsed / player.HandInterval;
        transform.localPosition = Vector2.Lerp(startPos, endPos, curve.Evaluate(per));
        flip = transform.localPosition.x < -0.2f;
        transform.localScale = new Vector3(flip ? -1 : 1, 1, 1);
        sr.sprite = Utility.GetItemData(Inventory.data[Player.HandIndex].id).Sprite;
    }
}
