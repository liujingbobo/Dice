using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public Unit Player;
    public Unit Enemy;

    private Queue<IEnumerator> ActionList = new Queue<IEnumerator>();

    IEnumerator StartBattle()
    {
        // Init Enemy
        // Init Player
        Player = new Unit(10);
        Enemy = new Unit(10);
        StartCoroutine(PlayerTurn());
        yield return null;
    }

    IEnumerator PlayerTurn()
    {
        Player.Clear();
        
        if (CheckGameEnd())
        {
            yield return null;
        }
        
        yield return null;
    }
    
    IEnumerator Roll()
    {
        Player.Clear();
        
        yield return null;
    }

    IEnumerator EnemyTurn()
    {
        Enemy.Clear();
        
        yield return null;
    }

    IEnumerator EndGame()
    {
        
        yield return null;
    }

    private bool CheckGameEnd()
    {
        return Player.IsDead || Enemy.IsDead;
    }
}

public enum BattleState
{
    Player,
    Enemy
}

