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

    public UnityEvent<DamageInfo> BeforeProcessDamage = new UnityEvent<DamageInfo>();
    public UnityEvent<DamageInfo> AfterProcessDamage = new UnityEvent<DamageInfo>();
    

    public UnityEvent<(DiceSide, Unit, Unit)> BeforeTakeAction = new UnityEvent<(DiceSide, Unit, Unit)>();
    public UnityEvent<(DiceSide, Unit, Unit)> AfterTakeAction = new UnityEvent<(DiceSide, Unit, Unit)>();
    
    public UnityEvent<DS_Attack.AttackInfo> BeforeAttack = new UnityEvent<DS_Attack.AttackInfo>();
    public UnityEvent<Unit> BeforeTurnStart = new UnityEvent<Unit>();
}
