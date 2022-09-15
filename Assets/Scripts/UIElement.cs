using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElement : MonoBehaviour
{
    public KeyCode keyCode;

    private void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            UIManager.Prev();
        }
    }
}