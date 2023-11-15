using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Buff", menuName = "Buff/Effect", order = 0)]
public class BF_Toxic : Effect, BeforeTurnStart
{
    public int damageMultiplier = 1;
    
    
    public override IEnumerator Init(BuffAction buffAction)
    {
        yield break;
    }

    IEnumerator Action(Unit unit, BuffAction buffAction)
    {
        if (unit.ID == buffAction.Target)
        {
            var state = BattleManager.Instance.Units[buffAction.Target].Value.Buffs[BuffType];
            
            var info = new DamageInfo()
            {
                SourceType = SourceType.Buff,
                Source = buffAction.BuffType.ToString(),
                Target = buffAction.Target,
                Value = state * damageMultiplier,
                BuffType = buffAction.BuffType
            };

            yield return BattleManager.Instance.DealDamage(info);
        }
    }

    public IEnumerator Trigger(Unit unit)
    {
        throw new System.NotImplementedException();
    }
}

// 