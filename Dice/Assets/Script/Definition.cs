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
    public void Clear()
    {
        // _br = 0;
    }
    public void DealDamage(int dmg)
    {
        // Debug.Log($"Before take damage, HP {_hp}, BR {_br}");
        // var block = _br;
        // _br = Mathf.Max(_br - dmg, 0);
        // dmg -= block;
        // if (dmg > 0)
        // {
        //     _hp -= dmg;
        // }
        // Debug.Log($"After take damage, HP {_hp}, BR {_br}");
    }
    public void GainBlock(int value)
    {
        // _br += value;
    }
    public void Heal(int value)
    {
    //     int hpAfterHeal = value + _hp;
    //     _hp += value;
    }

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
    public int Value;
    public bool IgnoreBarrier;
    public bool IsCanceled;
    public DiceSide SideEffect;
    
    // After process
    public int BlockedByBarrier;
}