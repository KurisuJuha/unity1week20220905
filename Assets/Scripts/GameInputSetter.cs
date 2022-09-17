using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class GameInputSetter : SingletonMonoBehaviour<GameInputSetter>
{
    public bool UIInputMask;
    public bool InventoryInputMask;
    public bool CardTableInputMask;

    void LateUpdate()
    {
        if (!UIInputMask && !InventoryInputMask && !CardTableInputMask)
        {
            // 使用
            if (Input.GetMouseButton(1)) GameInput.Use();

            // アタック
            if (Input.GetMouseButton(0)) GameInput.Attack();

            //  ハンドクラフト
            if (Input.GetKeyDown(KeyCode.E)) GameInput.HandCraft();

            // ドロップ
            if (Input.GetKeyDown(KeyCode.Q)) GameInput.Drop();

            // ゲームスタート
            if (Input.anyKeyDown) GameInput.GameStart();

            // 移動ベクトル
            GameInput.Move = Vector2.ClampMagnitude(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), 1);
        }
        else
        {
            GameInput.Move = Vector2.zero;
        }

        // UIを一つ戻す
        if (!CardTableInputMask)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) GameInput.UIPrev();
        }

        // デバッグキー
        if (Input.GetKeyDown(KeyCode.P)) GameInput.DebugKey();



        // マウス位置
        Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameInput.MousePosition = mousepos;


        // インベントリのスクロール
        GameInput.InventoryScroll = Input.mouseScrollDelta.y;
    }
}
