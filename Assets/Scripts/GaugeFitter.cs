using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaugeFitter : MonoBehaviour
{
    public GameObject number1;
    public GameObject slash;
    public GameObject number2;
    void Start()
    {
    }

    void Update()
    {
        number1.transform.localPosition = new Vector3(-6, 0, 0);
        slash.transform.localPosition = new Vector3(0, 0, 0);
        number2.transform.localPosition = new Vector3(number2.transform.childCount * 4 + 2, 0, 0);
    }
}
