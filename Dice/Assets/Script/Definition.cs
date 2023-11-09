using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unit
{
    private string _id;
    private int _hp; // HealthPoint
    private int _br; // Barrier
    private List<Dice> _dices;
    public bool IsDead => _hp <= 0;
    public int HP => _hp;
    public int BR => _br;
    public string ID => _id;

    public Unit(int hp, List<Dice> dices, string id)
    {
        _id = id;
        _hp = hp;
        _br = 0;
        _dices = dices;
    }

    public void Clear()
    {
        _br = 0;
    }

    public void DealDamage(int dmg)
    {
        Debug.Log($"Before take damage, HP {_hp}, BR {_br}");
        var block = _br;
        _br = Mathf.Max(_br - dmg, 0);
        dmg -= block;
        if (dmg > 0)
        {
            _hp -= dmg;
        }
        Debug.Log($"After take damage, HP {_hp}, BR {_br}");
    }

    public void GainBlock(int value)
    {
        _br += value;
    }

    public List<DiceSide> Roll()
    {
        var dices = _dices.Select(_ => _.RandomlyGetOne()).ToList();
        return dices;
    }
}