using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class BattleEvents : MonoBehaviour
{
    public static BattleEvents Instance;
    
    private void Awake()
    {
        if (Instance)
        {
            Destroy(Instance);            
        }
        Instance = this;
    }
    
    // public UnityEvent<DamageInfo> BeforeDealDamage_PS = new UnityEvent<DamageInfo>();
    // public UnityEvent<(DamageInfo info, int hpUsed, int brUsed)> AfterDealDamage_PS = new UnityEvent<(DamageInfo info, int hpUsed, int brUsed)>();
    //
    // public UnityEvent<GainBlockInfo> BeforeGainBlock_PS = new UnityEvent<GainBlockInfo>();
    // public UnityEvent<GainBlockInfo> AfterGainBlock = new UnityEvent<GainBlockInfo>();
    //
    // public UnityEvent<LoseBlockInfo> BeforeLoseBlock = new UnityEvent<LoseBlockInfo>();
    // public UnityEvent<LoseBlockInfo> AfterLoseBlock = new UnityEvent<LoseBlockInfo>();
    //
    // public UnityEvent<HealInfo> BeforeHeal = new UnityEvent<HealInfo>();
    //
    // public UnityEvent<(DiceSideEffect, Unit, Unit)> BeforeTakeAction = new UnityEvent<(DiceSideEffect, Unit, Unit)>();
    // public UnityEvent<(DiceSideEffect, Unit, Unit)> AfterTakeAction = new UnityEvent<(DiceSideEffect, Unit, Unit)>();
    //
    // public UnityEvent<DS_Attack.AttackInfo> BeforeAttack = new UnityEvent<DS_Attack.AttackInfo>();
    // public UnityEvent<Unit> BeforeTurnStart = new UnityEvent<Unit>();
    //
    // public UnityEvent<BuffAction> BeforeGiveBuff = new UnityEvent<BuffAction>();
    // public UnityEvent<BuffAction> AfterGiveBuff = new UnityEvent<BuffAction>();
    // public UnityEvent<BuffAction> BeforeLoseBuff = new UnityEvent<BuffAction>();
}
