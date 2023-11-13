using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffEffect : ScriptableObject
{
    public BuffType BuffType;
    
    public virtual IEnumerator GiveBuff(GiveBuffInfo info)
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