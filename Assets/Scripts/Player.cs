using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public PlayerAnimator animator;
    public float speed;
    private static float _handIndex;
    private static float HandIndexFloat
    {
        get { return _handIndex; }
        set
        {
            float i = value;
            while (i < 0)
            {
                i += 9;
            }
            i %= 9;

            _handIndex = i;
        }
    }
    public static int HandIndex
    {
        get { return Mathf.FloorToInt(HandIndexFloat); }
        set { HandIndexFloat = value; }
    }
    public float HandInterval;
    public float HandElapsed;
    public bool flip;
    public CameraShake shake;
    public GameObject CraftBenchObj;
    public UIElement CraftBenchUIElement;
    public UIElement TameruBoxUIElement;

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

        GameInput.Use = () =>
        {
            Set();
        };
        GameInput.Attack = () =>
        {
            Attack();
        };
        GameInput.Drop = () =>
        {
            Drop();
        };
        GameInput.HandCraft = () =>
        {
            UIManager.Show(CraftBenchUIElement);
            CraftBench.mode = CraftBenchMode.HandCraft;
        };
    }

    void Update()
    {
        PlayerPos = transform.position;
        GameInput.Move = Vector2.ClampMagnitude(GameInput.Move, 1);
        GameInput.Move *= speed;

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
        if (GameInput.Move.sqrMagnitude > 0.4f) animator.state = PlayerAnimationState.Walk;
        else animator.state = PlayerAnimationState.Idle;

        // HandIndex操作
        SetHandIndex();

        // 左右反転
        if (GameInput.MousePosition.x - transform.position.x < 0) flip = true;
        if (GameInput.MousePosition.x - transform.position.x > 0) flip = false;
        transform.localScale = new Vector3(flip ? -1 : 1, 1, 1);

        rb2d.velocity = GameInput.Move;

        // 経過時間関係
        HandElapsed += Time.deltaTime * SkillManager.Instance.card_speed;
        if (HandElapsed > HandInterval) HandElapsed = HandInterval;
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

        // マウスホイール
        HandIndexFloat -= GameInput.InventoryScroll;
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
                    UIManager.Show(CraftBenchUIElement);
                    CraftBench.mode = CraftBenchMode.CraftBench;
                    break;
                // かまど
                case 4:
                    UIManager.Show(CraftBenchUIElement); 
                    CraftBench.mode = CraftBenchMode.Furnace;
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
                    UIManager.Show(TameruBoxUIElement);
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
