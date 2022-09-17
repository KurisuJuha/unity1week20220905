using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TameruBox : MonoBehaviour
{
    public List<TameruGrid> tameruGrids;

    void Start()
    {

    }

    void Update()
    {

    }

    public void TameruButton()
    {
        foreach (var item in tameruGrids)
        {
            PointManager.AddPoint(item.id, item.quantity);
            item.quantity = 0;
        }
    }
}
