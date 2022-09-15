#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDebug : MonoBehaviour
{
    void Start()
    {
        GameInput.DebugKey = () =>
        {
            Debug.Log(string.Join(',', Inventory.data));
            Debug.Log(string.Join(',', Inventory.quantity));
        };
    }

    void Update()
    {

    }
}
#endif
