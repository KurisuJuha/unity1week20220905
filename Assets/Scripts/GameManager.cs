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

    public void UpdateMap(Vector2Int[] keys)
    {
        StartCoroutine(_updateMap(keys));
    }

    private IEnumerator _updateMap(Vector2Int[] keys)
    {
        foreach (var chunkpos in keys)
        {
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    MapData.UpdateTile(chunkpos, new Vector2Int(x, y));
                }
            }
            yield return null;
        }

        MapData.Changing = false;
    }
}
