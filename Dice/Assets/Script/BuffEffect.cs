using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff", menuName = "DiceSide/Attack", order = 0)]
public class BuffEffect : ScriptableObject
{
    public virtual IEnumerator GiveBuff(int amount)
    {
        yield break;
    }
}

public enum BuffType
{
    Bleeding, // 
    Toxic, 
    Fragile,
    Enraged,
    Fortified,
}