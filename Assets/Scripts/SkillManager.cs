using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : SingletonMonoBehaviour<SkillManager>
{
    [Range(0f, 1f)]
    public float Range;
    public AnimationCurve curve;
    public CardTable CardTable;
    public Vector2 StartPos;
    public Vector2 EndPos;
    public bool show;
    public float speed;

    public int[] quantity = new int[4];

    [Header("param")]
    public float card_power;
    public float card_luck;
    public float card_critical;
    public float card_speed;

    public void Show()
    {
        show = true;
        CardTable.Set();
    }

    public void Update()
    {
        int s = show ? -1 : 1;

        Range += s * Time.deltaTime * speed;

        Range = Mathf.Clamp01(Range);
        CardTable.transform.localPosition = Vector2.Lerp(StartPos, EndPos, curve.Evaluate(Range));
    }
}
