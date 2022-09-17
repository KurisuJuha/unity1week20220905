using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : SingletonMonoBehaviour<PointManager>
{
    public int Point;
    public int MaxPoint;
    public int Wave;

    public void Start()
    {
        MaxPoint = Mathf.FloorToInt(GameManager.Instance.settings.MaxPointCurve.Evaluate(0));
    }

    public void Update()
    {
        if (Point >= MaxPoint && !SkillManager.Instance.show)
        {
            Wave++;
            Point -= MaxPoint;
            MaxPoint = Mathf.FloorToInt(GameManager.Instance.settings.MaxPointCurve.Evaluate(Wave));
            SkillManager.Instance.Show();
        }
    }

    public void addPoint(int id, int quantity)
    {
        Point += GameManager.Instance.settings.items[id].point * quantity;
    }

    public static void AddPoint(int id, int quantity) => Instance.addPoint(id, quantity);
}
