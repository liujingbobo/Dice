using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public float a;
    public float b;
    public float c;
    public void Update()
    {
        print(Mathf.Lerp(a, b, c));
    }
}
