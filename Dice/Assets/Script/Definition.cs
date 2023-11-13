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
    public List<PresetDice> Dices;
    public Dictionary<BuffType, BuffInfo> Buffs;

    public bool IsDead => HP <= 0;

    public List<DiceSideEffect> Roll()
    {
        var dices = Dices.Select(_ => _.RandomlyGetOne()).ToList();
        return dices;
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

public class DamageInfo
{
    public string Source;
    public string Target;
    public int Value;
    public bool IgnoreBarrier;
    public bool IsCanceled;
    public DiceSideEffect SideEffectEffect;
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

public class GiveBuffInfo
{
    public int stacks;
    public string source;
    public string target;
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