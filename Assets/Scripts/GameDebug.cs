#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDebug : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log(string.Join(',', Inventory.data));
            Debug.Log(string.Join(',', Inventory.quantity));
        }
    }
}
#endif
