using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public struct BTUnit
{
    public string id; 
    public int maxHp;
    public int hp; // HealthPoint
    public int br; // Barrier
    public List<RTDiceData> Dices;
    public Dictionary<BuffType, int> buffs;
    public bool dead;

    public List<(int sideIndex, RTSideData side)> Roll()
    {
        var dices = Dices.Select(_ => _.Roll()).ToList();
        return dices;
    }

    public void AddBuff(BuffAction act)
    {
        int value = 0;
            
        buffs.TryGetValue(act.BuffType, out value);

        value += act.Stacks;
        
        if (act.ByStack)
        {
            buffs[act.BuffType] = value;
        }
    }

    public void LoseBuff(BuffAction act)
    {
        int value = 0;
        
        buffs.TryGetValue(act.BuffType, out value);

        if (value == 0) return;

        value = Mathf.Max(0, value - act.Stacks);
        
        buffs[act.BuffType] = value;
    }

    public bool HasBuff(BuffType buffType)
    {
        int value = 0;
        buffs.TryGetValue(buffType, out value);
        return value > 0;
    }

    public int GetBuffStack(BuffType buffType)
    {
        int value = 0;
        buffs.TryGetValue(buffType, out value);
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
    Side,
    Effect
}

public class DamageInfo
{
    public SourceType SourceType;
    public string Source;
    public string Target;
    public int Value;
    public bool IgnoreBarrier;
    public bool IsCanceled;
    public DiceSideEffect SideEffect;
    public Effect Effect;

    public DamageInfo(string source, string target, int value, bool ignoreBarrier, DiceSideEffect side)
    {
        Source = source;
        Target = target;
        Value = value;
        IgnoreBarrier = ignoreBarrier;
        SideEffect = side;
        SourceType = SourceType.Side;
    }

    public DamageInfo(string source, string target, int value, bool ignoreBarrier, Effect effect)
    {
        Source = source;
        Target = target;
        Value = value;
        IgnoreBarrier = ignoreBarrier;
        Effect = effect;
        SourceType = SourceType.Effect;
    }
}

public class GainBlockInfo
{
    public string Source;
    public string Target;
    public int Value;
    public float Multiplier;
    public int AddOn;
    public bool IsCanceled;

    public GainBlockInfo(string s, string t, int v, float m = 1.0f, int a = 0)
    {
        Source = s;
        Target = t;
        Value = v;
        Multiplier = m;
        AddOn = a;
    }
    
    public int GetValue()
    {
        return Math.Clamp((int)((Value + AddOn) * Multiplier), 0, int.MaxValue);
    }
    
    
}

public class LoseBlockInfo
{
    public string Source;
    public string Target;
    public int Value;
    public bool IsAuto; // 
    public bool IsCanceled;

    public LoseBlockInfo(string source, string target, int value, bool isAuto = false)
    {
        Source = source;
        Target = target;
        Value = value;
        IsAuto = isAuto;
    }
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
    public float Multiplier;
    public int AddOn;
    public bool IsCanceled;
    public int ExtraHeal;

    public HealInfo(string source, string target, int amt, float multiplier = 1f, int addOn = 0)
    {
        Source = source;
        Target = target;
        Value = amt;
        Multiplier = multiplier;
        AddOn = addOn;
    }

    public int GetValue()
    {
        return Math.Clamp((int)((Value + AddOn) * Multiplier), 0, int.MaxValue);
    }
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
}

public interface IUIThumbnail<T>
{
    public void FillWith(T content);
}

public class TurnInfo
{
    public bool Skipped;
}
