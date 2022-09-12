using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionTransition : MonoBehaviour
{
    [Range(0f, 1f)]
    public float Range;
    public GameObject Target;
    public AnimationCurve curve;
    public Vector2 StartPos;
    public Vector2 EndPos;

    public Dimension load;

    void Start()
    {
        
    }

    void Update()
    {
        Target.transform.localPosition = Vector2.Lerp(StartPos, EndPos, curve.Evaluate(Range));
    }
}
