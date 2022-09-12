using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public PlayerAnimator animator;
    public float speed;
    private static int _HandIndex;
    public static int HandIndex
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

            _HandIndex = i;
        }
    }
    public float HandInterval;
    public float HandElapsed;
    public bool flip;
    public CameraShake shake;
    public GameObject CraftBenchObj;

    public static Vector2 PlayerPos;

    void Start()
    {
        #region Debug
        Inventory.data[0].id = 4;
        Inventory.quantity[0] = 10;

        Chunk chunk = MapData.GetChunk(new Vector2Int(0, 0));
        for (int i = 0; i < 7; i++)
        {
            MapData.SetTile(new Vector2(i, 0), i);
            MapData.SetTile(new Vector2(i, 1), 1);
        }

        MapData.ChangeDimension(Dimension.UnderGround);
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                MapData.SetTile(new Vector2(x, y), 7);
            }
        }

        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                MapData.SetTile(new Vector2(x, y) + new Vector2(3, 3), 0);
            }
        }

        MapData.SetTile(new Vector2(15, 15), 7);
        MapData.SetTile(new Vector2(15, 16), 7);
        MapData.SetTile(new Vector2(16, 15), 7);
        MapData.SetTile(new Vector2(15, 14), 7);
        MapData.SetTile(new Vector2(14, 15), 7);

        #endregion

        HandElapsed = HandInterval;
        DropItemManager.Instance.NewDropItem(new Item() { id = 1 }, 2, transform.position);
        StartCoroutine(WalkAudio());
    }

    void Update()
    {
        PlayerPos = transform.position;
        Vector2 velo = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * speed;

        // プレイヤーの周囲3x3を読み込む
        Vector2Int playerChunk = Utility.GetChunkPos(transform.position);
        Vector2Int loadSize = new Vector2Int(3, 3);
        for (int y = 0; y < loadSize.y; y++)
        {
            for (int x = 0; x < loadSize.x; x++)
            {
                MapData.GetChunk(playerChunk - new Vector2Int(x - loadSize.x / 2, loadSize.y - 3 / 2));
            }
        }

        // 一定以上のスピードで動いていたらアニメーションする
        if (velo.sqrMagnitude > 0.4f) animator.state = PlayerAnimationState.Walk;
        else animator.state = PlayerAnimationState.Idle;

        // 設置
        if (Input.GetKey(KeyCode.K)) Set();
        // 破壊
        if (Input.GetKey(KeyCode.J)) Attack();
        // ドロップ
        if (Input.GetKeyDown(KeyCode.Q)) Drop();
        // HandIndex操作
        SetHandIndex();

        // 左右反転
        if (velo.x < 0) flip = true;
        if (velo.x > 0) flip = false;
        transform.localScale = new Vector3(flip ? -1 : 1, 1, 1);

        rb2d.velocity = velo;

        // 経過時間関係
        HandElapsed += Time.deltaTime;
        if (HandElapsed > HandInterval) HandElapsed = HandInterval;

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (MapData.GetDimension() == Dimension.UnderGround) MapData.ChangeDimension(Dimension.Ground);
            else MapData.ChangeDimension(Dimension.UnderGround);
        }
    }

    /// <summary>
    /// HandIndexを数字キーとかUとかIで操作する
    /// </summary>
    public void SetHandIndex()
    {
        // 数字キー
        if (Input.GetKeyDown(KeyCode.Alpha1)) HandIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) HandIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) HandIndex = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4)) HandIndex = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5)) HandIndex = 4;
        if (Input.GetKeyDown(KeyCode.Alpha6)) HandIndex = 5;
        if (Input.GetKeyDown(KeyCode.Alpha7)) HandIndex = 6;
        if (Input.GetKeyDown(KeyCode.Alpha8)) HandIndex = 7;
        if (Input.GetKeyDown(KeyCode.Alpha9)) HandIndex = 8;

        // HとL
        if (Input.GetKeyDown(KeyCode.U)) HandIndex--;
        if (Input.GetKeyDown(KeyCode.I)) HandIndex++;
    }

    /// <summary>
    /// 設置とか
    /// </summary>
    public void Set()
    {
        if (HandElapsed >= HandInterval)
        {
            // 設置
            if (
                // 数が1以上
                Inventory.quantity[HandIndex] > 0
                // 持っているアイテムが置けるかどうか
                && Utility.GetItemData(Inventory.data[HandIndex].id).CanPlace
                // 置く場所がempty
                && MapData.GetChunk(PlayerCursor.chunkPos).tiles[PlayerCursor.tilePos.x, PlayerCursor.tilePos.y].id == 0)
            {
                MapData.SetTile(PlayerCursor.Pos, Inventory.PeekItem(HandIndex));
                Inventory.quantity[HandIndex]--;

                // もしアイテム数が0ならidも削除する
                if (Inventory.quantity[HandIndex] == 0) Inventory.data[HandIndex] = new Item() { id = 0 };

                HandElapsed = 0;
                AudioManager.Play(AudioType.Attack);
                shake.Shake();

                return;
            }

            // 使用
            switch (MapData.GetChunk(PlayerCursor.chunkPos).tiles[PlayerCursor.tilePos.x, PlayerCursor.tilePos.y].id)
            {
                // クラフトベンチ
                case 3:
                    CraftBenchObj.SetActive(true);
                    CraftBench.mode = CraftBenchMode.CraftBench;
                    StartCoroutine(usetile(3, () =>
                    {
                        CraftBenchObj.SetActive(false);
                    }));
                    break;
                // かまど
                case 4:
                    CraftBenchObj.SetActive(true);
                    CraftBench.mode = CraftBenchMode.Furnace;
                    StartCoroutine(usetile(4, () =>
                    {
                        CraftBenchObj.SetActive(false);
                    }));
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// 破壊とか
    /// </summary>
    public void Attack()
    {
        if (HandElapsed >= HandInterval)
        {
            int power = Utility.GetItemData(Inventory.data[HandIndex].id).AttackPower;
            MapData.GetChunk(PlayerCursor.chunkPos).tiles[PlayerCursor.tilePos.x, PlayerCursor.tilePos.y].addDamage(power);

            HandElapsed = 0;
            AudioManager.Play(AudioType.Attack);
            shake.Shake();
        }
    }

    /// <summary>
    /// ドロップとか
    /// </summary>
    public void Drop()
    {
        if (// 数が1以上
            Inventory.quantity[HandIndex] > 0
            // Emptyアイテムじゃない
            && Inventory.data[HandIndex].id != 0)
        {
            DropItemManager.Instance.NewDropItem(Inventory.data[HandIndex], 1, transform.position);
            Inventory.quantity[HandIndex]--;

            // もしアイテム数が0ならidも削除する
            if (Inventory.quantity[HandIndex] == 0) Inventory.data[HandIndex] = new Item() { id = 0 };

            AudioManager.Play(AudioType.Failure);
        }
    }

    IEnumerator usetile(int id, System.Action finish)
    {
        var pos = PlayerCursor.Pos;
        while (true)
        {
            if (MapData.GetChunk(PlayerCursor.chunkPos).tiles[PlayerCursor.tilePos.x, PlayerCursor.tilePos.y].id != id) break;
            if (pos != PlayerCursor.Pos) break;
            yield return null;
        }

        finish();
    }

    IEnumerator WalkAudio()
    {
        while (true)
        {
            if (rb2d.velocity.magnitude > 0.4f)
            {
                AudioManager.Play(AudioType.Walk);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
}
