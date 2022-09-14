using UnityEngine;
using System;

public static class GameInput
{
    // 使用ボタン
    public static Action Use;
    public static Action UseDown;
    public static Action UseUp;

    // 破壊ボタン
    public static Action Attack;
    public static Action AttackDown;
    public static Action AttackUp;

    // 移動
    public static Vector2 Velocity;

    // ハンドクラフト
    public static Action HandCraft;
    public static Action HandCraftDown;
    public static Action HandCraftUp;

    // ドロップ
    public static Action Drop;
    public static Action DropDown;
    public static Action DropUp;

    // タイトル
    public static Action Esc;
    public static Action EscDown;
    public static Action EscUp;
}