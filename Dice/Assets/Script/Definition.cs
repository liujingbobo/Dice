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
    public List<Dice> Dices;
    public Dictionary<BuffType, int> Buffs;

    public bool IsDead => HP <= 0;

    public List<DiceSideEffect> Roll()
    {
        var dices = Dices.Select(_ => _.RandomlyGetOne()).ToList();
        return dices;
    }
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

public class HealInfo
{
    public string Source;
    public string Target;
    public int Value;
    public bool IsCanceled;
}

