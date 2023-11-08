using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit
{
    private int HP; // HealthPoint
    private int BR; // Barrier
    private List<Dice> _dices;
    public bool IsDead => HP <= 0;

    public Unit(int hp)
    {
        HP = hp;
        BR = 0;
    }

    public void Clear()
    {
        BR = 0;
    }

    public void DealDamage(int dmg)
    {
        dmg -= BR;
        BR = Mathf.Max(BR - dmg, 0);
        if (dmg > 0)
        {
            HP -= dmg;
        }
    }

    public void GainBlock(int value)
    {
        BR += value;
    }

    public List<DiceSide> Roll()
    {
        return null;
    }
}