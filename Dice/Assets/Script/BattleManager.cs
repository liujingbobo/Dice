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
    
    private void Awake()
    {
        if (Instance)
        {
            Destroy(Instance);            
        }

        Instance = this;
        
        Init();
    }

    public Dictionary<string, ReactiveProperty<Unit>> Units = new Dictionary<string, ReactiveProperty<Unit>>();
    public Unit Player => Units[_playerID].Value;
    public Unit Enemy => Units[_enemyID].Value;

    public string _playerID;
    public string _enemyID;
    
    public Dice AtkDice;
    public Dice BlkDice;
    
    public ReactiveProperty<BattleState> State;

    public Queue<IEnumerator> actionQueue = new Queue<IEnumerator>();

    public bool rolled = false;
    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void Start()
    {
        StartCoroutine(StartBattle());
    }

    public void Init()
    {
        State = new ReactiveProperty<BattleState>(BattleState.Init);
        // Init Enemy
        // Init Player
        List<Dice> playerDices = new List<Dice>();
        playerDices.Add(Instantiate(AtkDice));
        playerDices.Add(Instantiate(BlkDice));
        
        List<Dice> enemyDices = new List<Dice>();
        enemyDices.Add(Instantiate(AtkDice));
        enemyDices.Add(Instantiate(BlkDice));
        // Units["Player"] = new Unit()
        // {
        //     HP = 10,
        //     MaxHP = 10,
        //     BR = 0,
        //     Dices = playerDices
        // };
        //
        // Units["Player"] = new Unit()
        // {
        //     HP = 10,
        //     MaxHP = 10,
        //     BR = 0,
        //     Dices = playerDices
        // };
        //
        // Units["Player"]
        // Enemy = new Unit(10,enemyDices, "Enemy");
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
        
        Player.Clear();

        yield return new WaitUntil(() => rolled);
        
        rolled = false;
        
        var sides = Player.Roll();

        foreach (var side in sides)
        {
            Events.BeforeTakeAction.Invoke((side, Player, Enemy));
            
            yield return side.TakeAction(Player, Enemy);
            
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
    
    IEnumerator Do()
    {
        while (actionQueue.Count > 0)
        {
            var action = actionQueue.Dequeue();
            yield return StartCoroutine(action);
        }
        
        yield break;
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
        
        Enemy.Clear();
        
        var sides = Player.Roll();

        foreach (var side in sides)
        {
            yield return side.TakeAction(Enemy, Player);
            
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

        if (actionQueue.Count > 0)
        {
            yield return StartCoroutine(Do());
        }
        
        if (info.IsCanceled) yield break;
        
        
        
        
        Events.AfterProcessDamage.Invoke(info);
        
        if (actionQueue.Count > 0)
        {
            yield return StartCoroutine(Do());
        }
    }
    
    private bool CheckGameEnd()
    {
        return Player.IsDead || Enemy.IsDead;
    }
}

public enum BattleState
{
    Init,
    Player,
    Enemy
}

