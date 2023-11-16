using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : ScriptableObject
{
    public BuffType BuffType;
    public bool ByStack;
    
    public virtual IEnumerator AddBuff(BuffAction action)
    {
        yield break;
    }

    public virtual IEnumerator RemoveBuff(BuffAction action)
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
    ToxicAttack
}

