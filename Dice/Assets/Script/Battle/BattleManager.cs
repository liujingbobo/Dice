using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    private BattleEvents Events => BattleEvents.Instance;

    [SerializeField] private BuffManager buffManager;

    [SerializeField] private DiceManager diceManager;

    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private Transform unitContainer;
    
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

    public ReactiveProperty<BTUnit> playerRP = new ReactiveProperty<BTUnit>();
    
    public ReactiveProperty<BTUnit> enemyRP = new ReactiveProperty<BTUnit>();
    
    public ReactiveProperty<BattleState> state;

    public ReactiveProperty<int> RerollChance;

    public List<ReactiveProperty<bool>> Used;

    public List<int> rolledResult = new List<int>();

    public static BTUnit GetUnit(string id)
    {
        return Instance.units[id].Value;
    }
    public void Restart()
    {
        SceneManager.LoadScene("GameMenu");
    }
    
    public void Init(BTUnit player, BTUnit enemy)
    {
        state = new ReactiveProperty<BattleState>(BattleState.Init);

        playerRP = new ReactiveProperty<BTUnit>(player);
        units[playerRP.Value.ID] = playerRP;

        enemyRP = new ReactiveProperty<BTUnit>(enemy);
        units[enemyRP.Value.ID] = enemyRP;
        
        var playerUI = Instantiate(unitPrefab, unitContainer);
        if (playerUI.TryGetComponent<UIView_UnitStatus>(out var pui))
        {
            pui.Init(player.ID);
        }
        
        var enemyUI= Instantiate(unitPrefab, unitContainer);
        if (enemyUI.TryGetComponent<UIView_UnitStatus>(out var eui))
        {
            eui.Init(enemy.ID);
        }
        
        diceManager.Init(player.Dices);
        buffManager.Init(new List<string>(){player.ID, enemy.ID});

        StartCoroutine(StartBattle());
    }
    
    IEnumerator StartBattle()
    {
        yield return PlayerTurn();
    }

    IEnumerator PlayerTurn()
    {
        state.Value = BattleState.Init;
        
        print("Player turn start. ");

        yield return B.DO<BeforeTurnStart>(_ => _.BeforeTurnStart(true, new List<string>(){playerRP.Value.ID}));

        if (CheckGameEnd())
        {
            yield return EndGame();
            yield break;
        }
        
        var sides = playerRP.Value.Roll();

        rolledResult = sides.Select(_ => _.index).ToList();

        state.Value = BattleState.Waiting;
    }

    IEnumerator EnemyTurn()
    {
        state.Value = BattleState.Enemy;

        print("Enemy turn start. ");

        yield return B.DO<BeforeTurnStart>(_ => _.BeforeTurnStart(false, new List<string>() { enemyRP.Value.ID }));

        if (CheckGameEnd())
        {
            StartCoroutine(EndGame());
            yield break;
        }

        var sides = enemyRP.Value.Roll();

        var source = enemyRP.Value.ID;
            
        foreach (var side in sides)
        {
            yield return side.side.Side.TakeAction(new ActionInfo()
            {
                Source = source,
                Target = playerRP.Value.ID,
                Level = side.side.Level
            });
        }
            
        if (CheckGameEnd())
        {
            StartCoroutine(EndGame());
            yield break;
        }

        yield return B.DO<BeforeTurnEnd>(_ => _.BeforeTurnEnd(false, new List<string>() { enemyRP.Value.ID }));
        
            StartCoroutine(CheckGameEnd()? EndGame() : PlayerTurn());
    }

    IEnumerator EndGame()
    {
        print("Game End!!!");

        if (enemyRP.Value.IsDead)
        {
            print("Win!!");
        }
        else
        {
            print("Lose!!");
        }
        
        Restart();
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
        return playerRP.Value.IsDead || enemyRP.Value.IsDead;
    }
    private void ClearBlock(string id)
    {
        var state = units[id].Value;

        state.BR = 0;

        units[id].Value = state;
    }
    public void EndPlayerTurn()
    {
        StartCoroutine(EndPlayerTurnAction());
    }
    private IEnumerator EndPlayerTurnAction()
    {
        yield return B.DO<BeforeTurnEnd>(_ => _.BeforeTurnEnd(true, new List<string>() { playerRP.Value.ID }));
        StartCoroutine(CheckGameEnd() ? EndGame() : EnemyTurn());
    }
    
    public void Cast(int diceIndex)
    {
        if (state.Value != BattleState.Waiting) return;
        
        StartCoroutine(CastAction(diceIndex));
    }
    
    private IEnumerator CastAction(int DiceIndex)
    {
        state.Value = BattleState.Casting;
        Used[DiceIndex].Value = true;
        var side = playerRP.Value.Dices[DiceIndex].Sides[rolledResult[DiceIndex]];
        yield return side.Side.TakeAction(new ActionInfo()
        {
            Source = playerRP.Value.ID,
            Target = enemyRP.Value.ID,
            Level = side.Level
        });

        if (CheckGameEnd())
        {
            StartCoroutine(EndGame());
            yield break;
        }
        else
        {
            state.Value = BattleState.Waiting;
        }

    }

    public void Reroll(int diceIndex)
    {
        StartCoroutine(RerollAction(diceIndex));
    }
    
    private IEnumerator RerollAction(int diceIndex)
    {
        state.Value = BattleState.Rolling;
        RerollChance.Value--;
        var result = playerRP.Value.Dices[diceIndex].Roll();
        rolledResult[diceIndex] = result.sideIndex;
        yield return diceManager.Roll(new List<(int dice, int result)>(){(diceIndex,result.sideIndex)});
        state.Value = BattleState.Waiting;
    }
}

public enum BattleState
{
    Init = 10,
    Waiting = 20,
    Casting = 30,
    Rolling = 40,
    Enemy = 100
}

