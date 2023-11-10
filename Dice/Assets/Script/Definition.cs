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

    public bool IsDead => HP <= 0;

    public List<DiceSide> Roll()
    {
        // var dices = _dices.Select(_ => _.RandomlyGetOne()).ToList();
        // return dices;
        return null;
    }
}

public enum Buff
{
    Toxic,
    Broken
}

public class DamageInfo
{
    public string Source;
    public string Target;
    public int Value;
    public bool IgnoreBarrier;
    public bool IsCanceled;
    public DiceSide SideEffect;
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

