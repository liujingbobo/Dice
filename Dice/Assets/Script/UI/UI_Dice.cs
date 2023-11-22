using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Dice : MonoBehaviour
{
    public GameObject SidePrefab;

    public RTDiceData Target;

    public void Init(RTDiceData data)
    {
        Target = data;
    }
}
