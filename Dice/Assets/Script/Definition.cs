using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public struct BTUnit
{
    public string ID;
    public int MaxHP;
    public int HP; // HealthPoint
    public int BR; // Barrier
    public List<RTDiceData> Dices;
    public Dictionary<BuffType, int> Buffs;

    public bool IsDead => HP <= 0;

    public List<(int index, RTSideData side)> Roll()
    {
        var dices = Dices.Select(_ => _.Roll()).ToList();
        return dices;
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

    public bool HasBuff(BuffType buffType)
    {
        int value = 0;
        Buffs.TryGetValue(buffType, out value);
        return value > 0;
    }

    public int GetBuffStack(BuffType buffType)
    {
        int value = 0;
        Buffs.TryGetValue(buffType, out value);
        return value;
    }
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

public enum TargetType
{
    Self = 1,
    Opponent = 2,
    Both = 3
}

public class ActionInfo
{
    public string Source;
    public string Target;
    public int Level;
}

public interface IUIThumbnail<T>
{
    public void FillWith(T content);
}