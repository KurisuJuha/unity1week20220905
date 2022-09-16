using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionTransition : SingletonMonoBehaviour<DimensionTransition>
{
    [Range(0f, 1f)]
    public float Range;
    public GameObject Target;
    public AnimationCurve curve;
    public Vector2 StartPos;
    public Vector2 EndPos;
    public float Speed;

    private bool startChange;
    private bool updateMap;
    private Dimension dimension;
    private Vector2Int[] keys;

    void Start()
    {

    }

    void Update()
    {
        Target.transform.localPosition = Vector2.Lerp(StartPos, EndPos, 1 - curve.Evaluate(Range));

        if (startChange)
        {
            int a = dimension == Dimension.Ground ? -1 : 1;
            if ((a == 1 && Range <= 0.5f) || (a == -1 && Range > 0.5f))
            {
                Range += Time.deltaTime * a * Speed;
            }
            else
            {
                if (!updateMap)
                {
                    // 描画を更新
                    GameManager.Instance.UpdateMap(keys);
                    // 新しいDropItemを描画する
                    DropItemManager.Instance.SetActive(true);

                    updateMap = true;
                }
                if (!MapData.Changing)
                {
                    Range += Time.deltaTime * a * Speed;
                    if ((a == 1 && Range > 1f) || (a == -1 && Range < 0)) startChange = false;
                }
            }
        }
    }

    public void change(Dimension dimension, Vector2Int[] keys)
    {
        this.dimension = dimension;
        startChange = true;
        updateMap = false;
        this.keys = keys;
    }

    public static void Change(Dimension dimension, Vector2Int[] keys) => Instance.change(dimension, keys);
}
