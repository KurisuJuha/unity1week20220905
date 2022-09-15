using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class GameInputSetter : SingletonMonoBehaviour<GameInputSetter>
{
    public bool UIInputMask;

    void Update()
    {
        // 使用
        if (Input.GetMouseButton(1) && !UIInputMask) GameInput.Use();

        // アタック
        if (Input.GetMouseButton(0) && !UIInputMask) GameInput.Attack();

        //  ハンドクラフト
        if (Input.GetKeyDown(KeyCode.E) && !UIInputMask) GameInput.HandCraft();

        // ドロップ
        if (Input.GetKeyDown(KeyCode.Q) && !UIInputMask) GameInput.Drop();

        // ゲームスタート
        if (Input.anyKeyDown && !UIInputMask) GameInput.GameStart();

        // UIを一つ戻す
        if (Input.GetKeyDown(KeyCode.Escape)) GameInput.UIPrev();

        // デバッグキー
        if (Input.GetKeyDown(KeyCode.P)) GameInput.DebugKey();



        // マウス位置
        Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameInput.MouseTilePosition = new Vector2Int(Mathf.FloorToInt(mousepos.x), Mathf.FloorToInt(mousepos.y));

        // 移動ベクトル
        GameInput.Move = Vector2.ClampMagnitude(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), 1);

        // インベントリのスクロール
        GameInput.InventoryScroll = Input.mouseScrollDelta.y;
    }
}
