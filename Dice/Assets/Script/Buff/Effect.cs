using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Effect : ScriptableObject
{
    public Sprite icon;
    public BuffType buffType;
    public bool byStack;
    public bool showOnUI;
    
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

