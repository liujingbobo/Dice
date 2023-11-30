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
    
    public void Start()
    {
        var player = new BTUnit()
        {
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
                        Level = 0
                    }).ToList()
                };
                return data;
            }).ToList()
        };
    }
}
