using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public struct Unit
{
    public string ID;
    public int MaxHP;
    public int HP; // HealthPoint
    public int BR; // Barrier
    public List<RTDiceData> Dices;
    public Dictionary<BuffType, int> Buffs;

    public bool IsDead => HP <= 0;

    public List<DiceSideEffect> Roll()
    {
        return null;
        // var dices = Dices.Select(_ => _.RandomlyGetOne()).ToList();
        // return dices;
    }

    public void AddBuff(BuffAction act)
    {
        int value = 0;
        
        Buffs.TryGetValue(act.BuffType, out value);

        value += act.Stacks;
        
        if (act.ByStack)
        {
            Buffs[act.BuffType] = value;
        }
    }

    public void LoseBuff(BuffAction act)
    {
        int value = 0;
        
        Buffs.TryGetValue(act.BuffType, out value);

        if (value == 0) return;

        value = Mathf.Max(0, value - act.Stacks);
        
        Buffs[act.BuffType] = value;
    }
    
}

public class RuntimeDice
{
    public List<DiceSideEffect> sides;
}

public class RuntimeSide
{
}

public struct BuffInfo
{
    public int Stacks;
    public string Source;
}

public enum SourceType
{
    Unit,
    Buff
}

public class DamageInfo
{
    public SourceType SourceType;
    public string Source;
    public string Target;
    public int Value;
    public bool IgnoreBarrier;
    public bool IsCanceled;
    public DiceSideEffect SideEffectEffect;
    public BuffType BuffType;
}

public class GainBlockInfo
{
    public string Source;
    public string Target;
    public int Value;
    public bool IsCanceled;
}

public class LoseBlockInfo
{
    public string Source;
    public string Target;
    public int Value;
    public bool FromClear;
}

public class BuffAction
{
    public BuffType BuffType;
    public bool ByStack;
    public int Stacks;
    public string Source;
    public string Target;
    public bool IsCanceled;
}

public class HealInfo
{
    public string Source;
    public string Target;
    public int Value;
    public bool IsCanceled;
}

public enum SideType
{
    Attack,
    Skill
}

public class ActionInfo
{
    public Unit Source;
    public Unit Target;
    public int Level;
}