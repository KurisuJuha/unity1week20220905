using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public PlayerAnimator animator;
    public float speed;
    private int _HandIndex;
    public int HandIndex 
    {
        get { return _HandIndex; }
        set 
        { 
            int i = value;
            while (i < 0)
            {
                i += 9;
            }
            i %= 9;
        }
    }

    void Start()
    {
        Inventory.data[0] = new Item() { id = 1 };
        Inventory.am[0] = 3;
    }

    void Update()
    {
        Vector2 velo = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * speed;

        if (velo.sqrMagnitude > 0.4f) animator.state = PlayerAnimationState.Walk;
        else animator.state = PlayerAnimationState.Idle;

        if (Input.GetKeyDown(KeyCode.J)) Set();
        if (Input.GetKeyDown(KeyCode.K)) Atack();

        rb2d.velocity = velo;
    }

    public void Set()
    {
        if (
            // 数が1以上
            Inventory.am[HandIndex] > 0
            // 持っているアイテムが置けるかどうか
            && Utility.GetItemData(Inventory.data[HandIndex].id).CanPlace
            // 置く場所がempty
            && MapData.GetChunk(PlayerCursor.chunkPos).tiles[PlayerCursor.tilePos.x, PlayerCursor.tilePos.y].id == 0)
        {
            MapData.SetTile(PlayerCursor.Pos, Inventory.GetItem(HandIndex));
            Inventory.am[HandIndex]--;

            // もしアイテム数が0ならidも削除する
            if (Inventory.am[HandIndex] == 0) Inventory.data[HandIndex] = new Item() { id = 0 };
        }

    }

    public void Atack()
    {
        MapData.GetChunk(PlayerCursor.chunkPos).tiles[PlayerCursor.tilePos.x, PlayerCursor.tilePos.y].addDamage();
    }
}
