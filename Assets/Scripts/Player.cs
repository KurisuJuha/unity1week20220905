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
        HandElapsed = HandInterval;
        StartCoroutine(WalkAudio());

        // プレイヤーの周囲3x3をemptyにする
        Vector2Int size = new Vector2Int(3, 3);
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                MapData.SetTile(transform.position + new Vector3(x - size.x / 2, y - size.y / 2), 0);
            }
        }

        // 1,1にtameruboxを設置する
        MapData.SetTile(new Vector2(1, 1), 6);
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
                MapData.GetChunk(playerChunk - new Vector2Int(x - loadSize.x / 2, y - loadSize.y / 2));
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
        HandElapsed += Time.deltaTime * SkillManager.Instance.card_speed;
        if (HandElapsed > HandInterval) HandElapsed = HandInterval;

        // ハンドクラフト
        if (Input.GetKeyDown(KeyCode.E))
        {
            CraftBenchObj.SetActive(true);
            CraftBench.mode = CraftBenchMode.HandCraft;
            StartCoroutine(usetile(-1, () =>
            {
                CraftBenchObj.SetActive(false);
            }));
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

                if (Inventory.PeekItem(HandIndex).id == 8)
                {
                    Vector2Int size = new Vector2Int(10, 10);
                    for (int y = 0; y < size.y; y++)
                    {
                        for (int x = 0; x < size.x; x++)
                        {
                            Vector2 pos = PlayerCursor.Pos + new Vector2(x - size.x / 2, y - size.y / 2);
                            Vector2Int tilepos = Utility.GetTilePos(pos);
                            Chunk c = MapData.GetChunk(Utility.GetChunkPos(pos), Dimension.UnderGround);
                            c.tiles[tilepos.x, tilepos.y].id = 0;
                        }
                    }

                    Chunk chunk = MapData.GetChunk(PlayerCursor.chunkPos, Dimension.UnderGround);
                    chunk.tiles[PlayerCursor.tilePos.x, PlayerCursor.tilePos.y].id = 2;
                }

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
                // はしご
                case 2:
                    Dimension dimension;
                    if (MapData.GetDimension() == Dimension.UnderGround) dimension = Dimension.Ground;
                    else dimension = Dimension.UnderGround;
                    MapData.ChangeDimension(dimension);
                    HandElapsed = 0;
                    break;
                // タメルボックス
                case 6:
                    int id = Inventory.PeekItem(HandIndex).id;
                    if (Utility.GetItemData(id).Can_AddPoint
                        && Inventory.quantity[HandIndex] > 0) 
                    {
                        PointManager.AddPoint(id);
                        Inventory.quantity[HandIndex]--;
                        if (Inventory.quantity[HandIndex] <= 0) Inventory.data[HandIndex] = new Item() { id = 0 };
                        AudioManager.Play(AudioType.Success);
                    }
                    HandElapsed = HandInterval - 0.1f;
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
            power = Mathf.CeilToInt(power * SkillManager.Instance.card_power);
            MapData.GetChunk(PlayerCursor.chunkPos).tiles[PlayerCursor.tilePos.x, PlayerCursor.tilePos.y].addDamage(power);

            // クリティカル
            if (Random.Range(0, SkillManager.Instance.card_critical) > 1)
            {
                MapData.GetChunk(PlayerCursor.chunkPos).tiles[PlayerCursor.tilePos.x, PlayerCursor.tilePos.y].addDamage(power);
            }

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

    /// <summary>
    /// -1の場合は現在のタイルを初期値とする
    /// </summary>
    IEnumerator usetile(int id, System.Action finish)
    {
        var pos = PlayerCursor.Pos;
        if (id == -1)
        {
            id = MapData.GetChunk(PlayerCursor.chunkPos).tiles[PlayerCursor.tilePos.x, PlayerCursor.tilePos.y].id;
        }

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
