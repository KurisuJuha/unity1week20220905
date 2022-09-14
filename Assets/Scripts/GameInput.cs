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

    // ハンドクラフト
    public static Action HandCraft;
    public static Action HandCraftDown;
    public static Action HandCraftUp;

    // ドロップ
    public static Action Drop;
    public static Action DropDown;
    public static Action DropUp;

    // タイトル
    public static Action Title;
    public static Action TitleDown;
    public static Action TitleUp;




    // マウス位置
    public static Vector2 MousePosition;

    // 移動
    public static Vector2 MoveVector;
}