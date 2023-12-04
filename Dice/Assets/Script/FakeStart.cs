using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class FakeStart : MonoBehaviour
{
    public BTUnit playerBase;
    public BTUnit enemyBase;

    public List<PresetDice> playerDices;
    public List<PresetDice> enemyDices;

    public BattleManager bm;
    public void Start()
    {
        var player = new BTUnit()
        {
            ID = playerBase.ID,
            HP = playerBase.HP,
            MaxHP = playerBase.MaxHP,
            BR = 0,
            Buffs = new Dictionary<BuffType, int>(),
            Dices = playerDices.Select(_ =>
            {
                var data = new RTDiceData()
                {
                    Sides = _.Sides.Select(side => new RTSideData()
                    {
                        Side = side,
                    }).ToList()
                };
                return data;
            }).ToList()
        };
        
        var enemy = new BTUnit()
        {
            ID = enemyBase.ID,
            HP = enemyBase.HP,
            MaxHP = enemyBase.MaxHP,
            BR = 0,
            Buffs = new Dictionary<BuffType, int>(),
            Dices = enemyDices.Select(_ =>
            {
                var data = new RTDiceData()
                {
                    Sides = _.Sides.Select(side => new RTSideData()
                    {
                        Side = side,
                    }).ToList()
                };
                return data;
            }).ToList()
        };
        
        bm.Init(player, enemy);
    }
}
