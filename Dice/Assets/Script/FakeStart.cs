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
    
    public int enemyCount;

    public BattleManager bm;

    [SerializeField] private string unitBaseName;
    private int _currentUnitCount;
    
    public void Start()
    {
        _currentUnitCount = 0;
        
        var playerRTDices = new List<BTDiceData>();
        
        for (int i = 0; i < playerDices.Count; i++)
        {
            playerRTDices.Add(new BTDiceData()
            {
                Sides = playerDices[i].Sides.Select(side => new BTSideData()
                {
                    Side = Instantiate(side),
                }).ToList(),
                Index = i
            });
        }
        
        var player = new BTUnit()
        {
            id = unitBaseName + _currentUnitCount,
            hp = playerBase.hp,
            maxHp = playerBase.maxHp,
            br = 0,
            buffs = new Dictionary<BuffType, int>(),
            Dices = playerRTDices
        };

        _currentUnitCount++;

        var enemies = new List<BTUnit>();

        for (int e = 0; e < enemyCount; e++)
        {
            var enemyRTDices = new List<BTDiceData>();
            
            for (int i = 0; i < enemyDices.Count; i++)
            {
                enemyRTDices.Add(new BTDiceData()
                {
                    Sides = enemyDices[i].Sides.Select(side => new BTSideData()
                    {
                        Side = Instantiate(side),
                    }).ToList(),
                    Index = i
                });
            }
        
            var enemy = new BTUnit()
            {
                id = unitBaseName + _currentUnitCount,
                hp = enemyBase.hp,
                maxHp = enemyBase.maxHp,
                br = 0,
                buffs = new Dictionary<BuffType, int>(),
                Dices = enemyRTDices
            };
            
            enemies.Add(enemy);

            _currentUnitCount++;
        }

        
        bm.Init(player, enemies);
    }
}