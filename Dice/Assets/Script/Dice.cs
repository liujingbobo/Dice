using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiceSide", menuName = "Dice/Regular", order = 1)]
public class Dice : ScriptableObject
{
    public List<DiceSideEffect> Sides;
    public DiceSideEffect RandomlyGetOne()
    {
        int ran = Random.Range(0, Sides.Count);
        return Sides[ran];
    }
}
