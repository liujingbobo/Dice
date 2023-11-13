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

    public BuffManager _buffManager;
    private void Awake()
    {
        if (Instance)
        {
            Destroy(Instance);            
        }

        Instance = this;
    }

    private readonly Dictionary<string, ReactiveProperty<Unit>> units = new Dictionary<string, ReactiveProperty<Unit>>();
    public Dictionary<string, ReactiveProperty<Unit>> Units => units;
    public Unit Player => units[_playerID].Value;
    public Unit Enemy => units[_enemyID].Value;

    public string _playerID;
    
    public string _enemyID;
    
    public ReactiveProperty<BattleState> State;

    private readonly Queue<IEnumerator> _actionQueue = new Queue<IEnumerator>();

    private Coroutine processingQueue;

    public bool rolled = false;
    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void Start()
    {
        StartCoroutine(StartBattle());
    }

    public void Init(Unit player, Unit enemy)
    {
        State = new ReactiveProperty<BattleState>(BattleState.Init);

        units[player.ID].Value = player;
        units[enemy.ID].Value = enemy;

        _playerID = player.ID;
        _enemyID = enemy.ID;
    }
    
    IEnumerator StartBattle()
    {
        yield return StartCoroutine(PlayerTurn());
    }

    IEnumerator PlayerTurn()
    {
        State.Value = BattleState.Player;
        
        print("Player turn start. ");
        
        yield return new WaitForSeconds(1);
        
        ClearBlock(_playerID);

        yield return new WaitUntil(() => rolled);
        
        rolled = false;
        
        var sides = Player.Roll();

        foreach (var side in sides)
        {
            Events.BeforeTakeAction.Invoke((side, Player, Enemy));
            
            yield return StartCoroutine(side.TakeAction(new ActionInfo()));
            
            yield return new WaitForSeconds(1);
            
            Events.AfterTakeAction.Invoke((side, Player, Enemy));

            if (CheckGameEnd())
            {
                yield return StartCoroutine(EndGame());
                yield break;
            }
        }
        
        yield return StartCoroutine(EnemyTurn());
    }
    
    public IEnumerator ProcessActions()
    {
        if (processingQueue == null)
        {
            processingQueue = StartCoroutine(Do());
        }
        else
        {
            yield return new WaitUntil(() => processingQueue == null);
        }
    }
    
    IEnumerator Do()
    {
        while (_actionQueue.Count > 0)
        {
            var action = _actionQueue.Dequeue();
            yield return StartCoroutine(action);
        }

        processingQueue = null;
    }
    
    public void Roll()
    {
        rolled = true;
    }

    IEnumerator EnemyTurn()
    {
        State.Value = BattleState.Enemy;

        print("Enemy turn start. ");
        
        yield return new WaitForSeconds(1);
        
        ClearBlock(_enemyID);
        
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
        Events.BeforeProcessDamage.Invoke(info);

        if (_actionQueue.Count > 0)
        {
            yield return StartCoroutine(ProcessActions());
        }
        
        if (info.IsCanceled) yield break;

        Unit state = units[info.Target].Value;
        
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
        
        Debug.Log($"After take damage, HP {state.HP}, BR {state.BR}");
        
        Events.AfterProcessDamage.Invoke((info, hpCost, blockUsed));
        
        if (_actionQueue.Count > 0)
        {
            yield return StartCoroutine(Do());
        }
    }
    
    public IEnumerator GainBlock(GainBlockInfo info)
    {
        Events.BeforeGainBlock.Invoke(info);

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
        Events.BeforeHeal.Invoke(info);

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
    
    private bool CheckGameEnd()
    {
        return Player.IsDead || Enemy.IsDead;
    }

    public void PushAction(IEnumerator enumerator)
    {
        _actionQueue.Enqueue(enumerator);
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

