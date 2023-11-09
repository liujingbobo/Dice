using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    private void Awake()
    {
        if (Instance)
        {
            Destroy(Instance);            
        }

        Instance = this;
        
        Init();
    }

    public Unit Player;
    
    public Unit Enemy;

    public Dice AtkDice;
    public Dice BlkDice;
    
    public ReactiveProperty<BattleState> State;

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
        Player = new Unit(10, playerDices, "Player");
        Enemy = new Unit(10,enemyDices, "Enemy");
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
            yield return side.TakeAction(Player, Enemy);
            
            yield return new WaitForSeconds(1);

            if (CheckGameEnd())
            {
                yield return StartCoroutine(EndGame());
                yield break;
            }
        }

        yield return StartCoroutine(EnemyTurn());
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

