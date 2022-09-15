using UnityEngine;
using System;

public static class GameInput
{
    // 使用ボタン
    public static Action Use;

    // 破壊ボタン
    public static Action Attack;

    // ハンドクラフト
    public static Action HandCraft;

    // ドロップ
    public static Action Drop;

    // UIを一つ戻す
    public static Action UIPrev;

    // ゲームスタート
    public static Action GameStart;

    // デバッグキー
    public static Action DebugKey;




    // マウス位置
    public static Vector2Int MouseTilePosition;

    // 移動
    public static Vector2 Move;

    // Inventory
    public static float InventoryScroll; 
}