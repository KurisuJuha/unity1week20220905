using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public Number number;
    public SpriteRenderer sr;
    public Item item;
    public int quantity;
    public bool ignore = true;
    public float elapsed;

    void Start()
    {
        ignore = true;
    }

    void Update()
    {
        number.number = quantity;
        sr.sprite = GameManager.Instance.settings.items[item.id].Sprite;

        if (ignore) elapsed += Time.deltaTime;
        ignore = elapsed < GameManager.Instance.settings.dropItemIgnoreTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (ignore) return;
        if (collision.transform.tag != "Player") return;


        (bool, int) ret = Inventory.AddItem(item, quantity);

        if (ret.Item1)
        {
            DropItemManager.Instance.DestroyDropItem(this);
            AudioManager.Play(AudioType.Failure);
            return;
        }

        quantity = ret.Item2;
    }
}
