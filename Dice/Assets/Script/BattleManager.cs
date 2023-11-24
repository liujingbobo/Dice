using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    private BattleEvents Events => BattleEvents.Instance;

    public BuffManager buffManager;
    private void Awake()
    {
        if (Instance)
        {
            Destroy(Instance);            
        }

        Instance = this;
    }

    private readonly Dictionary<string, ReactiveProperty<BTUnit>> units = new Dictionary<string, ReactiveProperty<BTUnit>>();
    public Dictionary<string, ReactiveProperty<BTUnit>> Units => units;

    public List<ReactiveProperty<BTUnit>> players = new List<ReactiveProperty<BTUnit>>();
    
    public List<ReactiveProperty<BTUnit>> enemies = new List<ReactiveProperty<BTUnit>>();
    
    public ReactiveProperty<BattleState> state;

    public bool rolled = false;

    public static BTUnit GetUnit(string id)
    {
        return Instance.units[id].Value;
    }
    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void Start()
    {
        StartCoroutine(StartBattle());
    }

    public void Init(List<BTUnit> player, List<BTUnit> enemy)
    {
        state = new ReactiveProperty<BattleState>(BattleState.Init);

        player.ForEach(_ =>
        {
            var re = new ReactiveProperty<BTUnit>(_);
            units[_.ID] = re;
            players.Add(re);
        });
        
        enemy.ForEach(_ =>
        {
            var re = new ReactiveProperty<BTUnit>(_);
            units[_.ID] = re;
            enemies.Add(re);
        });
    }
    
    IEnumerator StartBattle()
    {
        yield return PlayerTurn();
    }

    IEnumerator PlayerTurn()
    {
        state.Value = BattleState.Player;
        
        print("Player turn start. ");

        foreach (var reUnit in players)
        {
            if (!reUnit.Value.IsDead)
            {
                yield return B.DO<BeforeTurnStart>(_ => _.BeforeTurnStart(reUnit.Value.ID));
            }
        }

        yield return CheckGameEnd();

        yield return new WaitUntil(() => rolled);
        
        rolled = false;
        
        var sides = Player.Roll();

        foreach (var side in sides)
        {
            yield return side.Side.TakeAction(new ActionInfo()
            {
                
            });
            
            yield return new WaitForSeconds(1);
            
            if (CheckGameEnd())
            {
                yield return EndGame();
                yield break;
            }
        }
        
        yield return EnemyTurn();
    }

    public IEnumerator FinishRound()
    {
        yield return B.DO<BeforeTurnEnd>(_ => _.BeforeTurnEnd(info));
    }
    
    public void Roll()
    {
        rolled = true;
    }

    IEnumerator EnemyTurn()
    {
        state.Value = BattleState.Enemy;

        print("Enemy turn start. ");
        
        yield return new WaitForSeconds(1);
        
        ClearBlock(enemyID);
        
        var sides = Player.Roll();

        foreach (var side in sides)
        {
            yield return side.TakeAction(new ActionInfo());
            
            yield return new WaitForSeconds(1);

            if (CheckGameEnd())
            {
                yield return StartCoroutine(EndGame());
                yield break;
            }
        }

        StartCoroutine(PlayerTurn());
    }

    IEnumerator EndGame()
    {
        print("Game End!!!");

        if (Enemy.IsDead)
        {
            print("Win!!");
        }
        else
        {
            print("Lose!!");
        }
        yield break;
    }

    public IEnumerator DealDamage(DamageInfo info)
    {
        yield return B.DO<BeforeDealDamage>(_ => _.BeforeDealDamage(info));

        if (info.IsCanceled) yield break;

        BTUnit state = units[info.Target].Value;
        
        Debug.Log($"Before take damage, HP {state.HP}, BR {state.BR}");

        int dmg = info.Value;
        
        int block = state.BR;
        
        int blockUsed = info.IgnoreBarrier ? 0 : Mathf.Min(dmg, block);
        
        state.BR -= blockUsed;
        
        dmg -= blockUsed;

        int hpCost = dmg;
        
        if (dmg > 0)
        {
            state.HP -= dmg;
        }

        units[info.Target].Value = state;

        yield return B.DO<AfterDealDamage>(_ => _.AfterDealDamage(info));
        
        Debug.Log($"After take damage, HP {state.HP}, BR {state.BR}");
    }
    
    public IEnumerator GainBlock(GainBlockInfo info)
    {
        if (info.IsCanceled)
        {
            yield break;
        }
        else
        {
            var state = units[info.Target].Value;

            state.BR += info.Value;

            units[info.Target].Value = state;
        }
    }

    public IEnumerator Heal(HealInfo info)
    {
        if (info.IsCanceled)
        {
            yield break;
        }else
        {
            var state = units[info.Target].Value;

            state.HP += info.Value;

            state.HP = Mathf.Min(state.HP, state.MaxHP);

            units[info.Target].Value = state;
        }
    }

    public IEnumerator GainBuff(BuffAction action)
    {
        if (action.IsCanceled)
        {
            //
        }
        else
        {
            var state = Units[action.Target].Value;

            state.AddBuff(action);

            Units[action.Target].Value = state;
            
            yield return buffManager.AddBuff(action);
        }
    }

    public IEnumerator LoseBuff(BuffAction action)
    {
        if (action.IsCanceled)
        {
            //
        }
        else
        {
            var state = Units[action.Target].Value;

            state.LoseBuff(action);

            Units[action.Target].Value = state;
            
            yield return buffManager.RemoveBuff(action);
        }
    }
    
    private bool CheckGameEnd()
    {
        return Player.IsDead || Enemy.IsDead;
    }

    private void ClearBlock(string id)
    {
        var state = units[id].Value;

        state.BR = 0;

        units[id].Value = state;
    }
}

public enum BattleState
{
    Init,
    Player,
    Enemy
}

