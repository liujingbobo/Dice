using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff", menuName = "Buff/ToxicAttack", order = 0)]
public class BF_ToxicAttack : Effect, AfterDealDamage, BeforeTurnEnd
{
    public int damageMultiplier = 1;
    
    public string Source;
    
    public override IEnumerator Init(BuffAction buffAction)
    {
        Source = buffAction.Target;
        yield break;
    }

    public IEnumerator AfterDealDamage(DamageInfo DmgInfo)
    {
        if (Source == DmgInfo.Source && DmgInfo.SourceType == SourceType.Unit)
        {
            if (BattleManager.GetUnit(Source).HasBuff(BuffType))
            {
                var state = BattleManager.Instance.Units[DmgInfo.Target].Value.Buffs[BuffType];

                yield return BattleEvents.Instance.DO<AfterDealDamage>(a => a.AfterDealDamage(DmgInfo));
            }
        }
    }

    public IEnumerator BeforeTurnEnd(string ID)
    {
        if (ID == Source && BattleManager.GetUnit(ID).HasBuff(BuffType))
        {
            var info = new BuffAction()
            {
                BuffType = BuffType,
                Stacks = 1,
                ByStack = true,
            };
            
            yield return BattleManager.Instance.LoseBuff(info);
        }
    }
}
