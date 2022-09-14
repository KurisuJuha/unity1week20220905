using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class GameInputSetter : MonoBehaviour
{
    void Update()
    {

        // 使用
        if (Input.GetMouseButton(1)) GameInput.Use();
        if (Input.GetMouseButtonDown(1)) GameInput.UseDown();
        if (Input.GetMouseButtonUp(1)) GameInput.UseUp();

        // アタック
        if (Input.GetMouseButton(0)) GameInput.Attack();
        if (Input.GetMouseButtonDown(0)) GameInput.AttackDown();
        if (Input.GetMouseButtonUp(0)) GameInput.AttackUp();

        //  ハンドクラフト
        if (Input.GetKey(KeyCode.E)) GameInput.HandCraft();
        if (Input.GetKeyDown(KeyCode.E)) GameInput.HandCraftDown();
        if (Input.GetKeyUp(KeyCode.E)) GameInput.HandCraftUp();

        // ドロップ
        if (Input.GetKey(KeyCode.Q)) GameInput.Drop();
        if (Input.GetKeyDown(KeyCode.Q)) GameInput.DropDown();
        if (Input.GetKeyUp(KeyCode.Q)) GameInput.DropUp();

        // タイトル
        if (Input.GetKey(KeyCode.Escape)) GameInput.Title();
        if (Input.GetKeyDown(KeyCode.Escape)) GameInput.TitleDown();
        if (Input.GetKeyUp(KeyCode.Escape)) GameInput.TitleUp();



        // マウス位置
        GameInput.MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 移動ベクトル
        GameInput.MoveVector = Vector2.ClampMagnitude(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), 1);
    }
}