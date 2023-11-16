using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff", menuName = "Buff/ToxicAttack", order = 0)]
public class BF_ToxicAttack : Effect, AfterDealDamage, BeforeTurnEnd
{
    public string Source;
    
    public override IEnumerator AddBuff(BuffAction buffAction)
    {
        Source = buffAction.Target;
        yield break;
    }

    public IEnumerator AfterDealDamage(DamageInfo DmgInfo)
    {
        if (Source == DmgInfo.Source && Source == DmgInfo.Target && DmgInfo.SourceType == SourceType.Unit)
        {
            BuffAction buffAction = new BuffAction()
            {
                BuffType = BuffType,
                ByStack = true,
                Stacks = 1,
                Source = DmgInfo.Source,
                Target = DmgInfo.Target
            };
        
            yield return BattleManager.Instance.GainBuff(buffAction);
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
