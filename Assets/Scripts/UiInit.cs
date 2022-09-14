using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiInit : MonoBehaviour
{
    public List<Obj> objs;

    void Start()
    {
        foreach (var obj in objs)
        {
            if (obj != null) obj.obj.SetActive(obj.init);
        }
    }

    void Update()
    {
        
    }
}

[System.Serializable]
public class Obj
{
    public GameObject obj;
    public bool init;
}