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
    private EventsManager EventsManager => EventsManager.Instance;

    [SerializeField] private BuffManager buffManager;
    [SerializeField] private DiceManager diceManager;
    [SerializeField] private UnitManager unitManager;
    [SerializeField] private EventsManager eventManager;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(Instance);
        }

        Instance = this;
    }

    private readonly Dictionary<string, ReactiveProperty<BTUnit>> units =
        new Dictionary<string, ReactiveProperty<BTUnit>>();

    public Dictionary<string, ReactiveProperty<BTUnit>> Units => units;

    public ReactiveProperty<BTUnit> playerRP = new ReactiveProperty<BTUnit>();

    public List<ReactiveProperty<BTUnit>> enemyRPs = new List<ReactiveProperty<BTUnit>>();

    public ReactiveProperty<BattleState> state;

    public ReactiveProperty<int> RerollChance;

    private List<RTDiceData> GetDiceDatas => playerRP.Value.Dices;

    public static BTUnit GetUnit(string id)
    {
        return Instance.units[id].Value;
    }
    public void Init(BTUnit player, List<BTUnit> enemies)
    {
        state = new ReactiveProperty<BattleState>(BattleState.Init);

        playerRP = new ReactiveProperty<BTUnit>(player);
        units[playerRP.Value.id] = playerRP;
        unitManager.AddUnit(playerRP.Value.id);

        enemyRPs = enemies.Select(_ =>
        {
            var rp = new ReactiveProperty<BTUnit>(_);
            units[_.id] = rp;
            
            unitManager.AddUnit(_.id);
            return rp;
        }).ToList();

        eventManager.Init();
        diceManager.Init(player.Dices);
        buffManager.Init(new List<string>() { player.id }.Concat(enemies.Select(_ => _.id)).ToList());

        StartCoroutine(StartBattle());
    }
    private void SetState(BattleState targetState)
    {
        print($"Prev State is {this.state.Value.ToString()}, switching to {targetState.ToString()}");
        state.Value = targetState;
        eventManager.onSwitchingState.Invoke(state.Value);
    }
    private IEnumerator StartBattle()
    {
        yield return PlayerTurn();
    }
    private IEnumerator PlayerTurn()
    {
        SetState(BattleState.Init);

        print("Player turn start. ");

        RerollChance.Value = 1;

        yield return B.DO<BeforeTurnStart>(_ => _.BeforeTurnStart(true, new List<string>() { playerRP.Value.id }));

        yield return LoseBlock(new LoseBlockInfo(playerRP.Value.id, playerRP.Value.id, playerRP.Value.br, true));

        if (CheckGameEnd())
        {
            yield return EndGame();
            yield break;
        }

        var sides = playerRP.Value.Roll();

        // Reset Dice

        var dices = playerRP.Value.Dices;

        for (int i = 0; i < dices.Count; i++)
        {
            var dice = dices[i];
            dice.Used = false;
            dice.RerollCount = 0;
            dice.Rerollable = RerollChance.Value > 0;
            dice.RolledResult = sides[dice.Index].sideIndex;
            dices[i] = dice;
        }

        yield return diceManager.Roll(GetDiceDatas);

        SetState(BattleState.Waiting);

        diceManager.RefreshAll(GetDiceDatas);
    }
    private IEnumerator EnemyTurn()
    {
        SetState(BattleState.Enemy);

        print("Enemy turn start. ");

        var names = enemyRPs.Select(_ => _.Value.id).ToList();

        yield return B.DO<BeforeTurnStart>(_ =>
            _.BeforeTurnStart(false, enemyRPs.Where(e => !e.Value.dead).Select(e => e.Value.id).ToList()));

        if (CheckGameEnd())
        {
            StartCoroutine(EndGame());
            yield break;
        }

        foreach (var enemyRP in enemyRPs)
        {
            var sides = enemyRP.Value.Roll();

            var source = enemyRP.Value.id;

            foreach (var side in sides)
            {
                yield return side.side.Side.TakeAction(new ActionInfo()
                {
                    Source = source,
                    Target = side.side.Side.TargetType == TargetType.Opponent ? playerRP.Value.id : source,
                });
            }

            if (CheckGameEnd())
            {
                StartCoroutine(EndGame());
                yield break;
            }
        }


        yield return B.DO<BeforeTurnEnd>(_ =>
            _.BeforeTurnEnd(false, enemyRPs.Where(_ => !_.Value.dead).Select(_ => _.Value.id).ToList()));

        StartCoroutine(CheckGameEnd() ? EndGame() : PlayerTurn());
    }
    private IEnumerator EndGame()
    {
        print("Game End!!!");

        if (enemyRPs.All(_ => _.Value.dead))
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
        if (units[info.Target].Value.dead) yield break;

        yield return B.DO<BeforeDealDamage>(_ => _.BeforeDealDamage(info));

        if (info.IsCanceled) yield break;

        BTUnit unitState = units[info.Target].Value;

        Debug.Log($"Before take damage, HP {unitState.hp}, BR {unitState.br}");

        var dmg = info.Value;

        var block = unitState.br;

        var blockUsed = info.IgnoreBarrier ? 0 : Mathf.Min(dmg, block);

        if (blockUsed > 0)
        {
            yield return LoseBlock(new LoseBlockInfo(info.Source, info.Target, blockUsed));
            unitState = units[info.Target].Value;
        }

        dmg -= blockUsed;

        if (dmg > 0)
        {
            unitState.hp -= dmg;
            unitState.hp = Math.Max(0, unitState.hp);
        }

        units[info.Target].Value = unitState;

        yield return B.DO<AfterDealDamage>(_ => _.AfterDealDamage(info));

        Debug.Log($"After take damage, HP {unitState.hp}, BR {unitState.br}");

        if (units[info.Target].Value.hp == 0)
        {
            unitState = units[info.Target].Value;
            unitState.dead = true;
            units[info.Target].Value = unitState;
            yield return SetUnitDead(info.Target);
        }
    }
    public IEnumerator GainBlock(GainBlockInfo info)
    {
        yield return B.DO<BeforeGainBlock>(_ => _.BeforeGainBlock(info));

        if (info.IsCanceled) yield break;

        var unitState = units[info.Target].Value;

        unitState.br += info.Value;

        units[info.Target].Value = unitState;

        yield return B.DO<AfterGainBlock>(_ => _.AfterGainBlock(info));
    }
    public IEnumerator LoseBlock(LoseBlockInfo info)
    {
        yield return B.DO<BeforeLoseBlock>(_ => _.BeforeLoseBlock(info));

        if (info.IsCanceled) yield break;

        var unitState = units[info.Target].Value;

        unitState.br -= info.Value;

        units[info.Target].Value = unitState;

        yield return B.DO<AfterLoseBlock>(_ => _.AfterLoseBlock(info));
    }
    public IEnumerator Heal(HealInfo info)
    {
        yield return B.DO<BeforeHeal>(_ => _.BeforeHeal(info));

        if (info.IsCanceled) yield break;

        var unitState = units[info.Target].Value;

        int gap = unitState.maxHp - unitState.hp;

        if (info.GetValue() - gap > 0)
        {
            info.ExtraHeal = info.GetValue() - gap;
        }

        unitState.hp += info.Value;

        unitState.hp = Mathf.Min(unitState.hp, unitState.maxHp);

        units[info.Target].Value = unitState;

        yield return B.DO<AfterHeal>(_ => _.AfterHeal(info));
    }
    public IEnumerator GainBuff(BuffAction action)
    {
        if (action.IsCanceled)
        {
            //
        }
        else
        {
            var unitState = Units[action.Target].Value;

            unitState.AddBuff(action);

            yield return buffManager.AddBuff(action);

            Units[action.Target].SetValueAndForceNotify(unitState);
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
            var unitState = Units[action.Target].Value;

            unitState.LoseBuff(action);

            yield return buffManager.RemoveBuff(action);

            Units[action.Target].SetValueAndForceNotify(unitState);
        }
    }
    private bool CheckGameEnd()
    {
        return playerRP.Value.dead || enemyRPs.All(_ => _.Value.dead);
    }
    private void ClearBlock(string id)
    {
        var unitState = units[id].Value;

        unitState.br = 0;

        units[id].Value = unitState;
    }
    public void EndPlayerTurn()
    {
        StartCoroutine(EndPlayerTurnAction());
    }
    private IEnumerator EndPlayerTurnAction()
    {
        yield return B.DO<BeforeTurnEnd>(_ => _.BeforeTurnEnd(true, new List<string>() { playerRP.Value.id }));
        StartCoroutine(CheckGameEnd() ? EndGame() : EnemyTurn());
    }
    public void Cast(int diceIndex, string target)
    {
        if (state.Value != BattleState.Waiting) return;

        StartCoroutine(CastAction(diceIndex, target));
    }

    public void Cast(int diceIndex)
    {
        if (state.Value != BattleState.Waiting) return;

        StartCoroutine(CastAction(diceIndex, playerRP.Value.id));
    }

    private IEnumerator CastAction(int DiceIndex, string target)
    {
        SetState(BattleState.Casting);

        var playerDices = playerRP.Value.Dices;

        var diceData = playerDices[DiceIndex];
        diceData.Used = true;
        playerDices[DiceIndex] = diceData;

        var side = playerRP.Value.Dices[DiceIndex].GetSide();

        yield return side.Side.TakeAction(new ActionInfo()
        {
            Source = playerRP.Value.id,
            Target = target,
        });

        if (CheckGameEnd())
        {
            StartCoroutine(EndGame());
            yield break;
        }
        else
        {
            SetState(BattleState.Waiting);
            RefreshDices();
        }
    }

    public void RefreshDices()
    {
        var dices = playerRP.Value.Dices;

        for (int i = 0; i < dices.Count; i++)
        {
            var data = dices[i];
            data.Rerollable = RerollChance.Value > 0 && !data.Used;
            dices[data.Index] = data;
        }

        diceManager.RefreshAll(GetDiceDatas);
    }

    public void ReRoll(int diceIndex)
    {
        StartCoroutine(ReRollAction(diceIndex));
    }

    private IEnumerator ReRollAction(int diceIndex)
    {
        SetState(BattleState.Rolling);
        RerollChance.Value--;

        var unitState = playerRP.Value;

        var result = unitState.Dices[diceIndex].Roll();

        var diceState = unitState.Dices[diceIndex];

        diceState.RerollCount++;

        diceState.RolledResult = result.sideIndex;

        unitState.Dices[diceIndex] = diceState;

        yield return diceManager.Roll(new List<RTDiceData>() { diceState });

        RefreshDices();

        playerRP.SetValueAndForceNotify(unitState);

        state.Value = BattleState.Waiting;
    }

    private IEnumerator SetUnitDead(string id)
    {
        yield return buffManager.RemoveUnit(id);
        yield return eventManager.RemoveUnit(id);
        unitManager.RemoveUnit(id);
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