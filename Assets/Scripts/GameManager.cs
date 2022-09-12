using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public Settings settings;
    public Tilemap tilemap;
    public float SceneChangeSpeed;
    public bool game;
    public GameObject GaugePrefab;

    private void Start()
    {
        
    }

    public void Update()
    {
        if (Input.anyKey) game = true;
        if (Input.GetKey(KeyCode.Tab)) game = false;

        SceneChanger.Instance.range += Time.deltaTime * SceneChangeSpeed * (game ? 1 : -1);
        SceneChanger.Instance.range = Mathf.Clamp01(SceneChanger.Instance.range);
    }
}
