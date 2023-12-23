using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff", menuName = "Buff/ToxicAttack", order = 0)]
public class BF_ToxicAttack : Effect, AfterDealDamage, BeforeTurnEnd
{
    private string _target;
    
    public override IEnumerator AddBuff(BuffAction buffAction)
    {
        _target = buffAction.Target;
        yield break;
    }

    public IEnumerator AfterDealDamage(DamageInfo DmgInfo)
    {
        if (_target == DmgInfo.Source && _target != DmgInfo.Target && DmgInfo.SourceType == SourceType.Side)
        {
            var buffAction = new BuffAction()
            {
                BuffType = BuffType.Toxic,
                ByStack = true,
                Stacks = 1,
                Source = DmgInfo.Source,
                Target = DmgInfo.Target
            };
        
            yield return BattleManager.Instance.GainBuff(buffAction);
        }
    }
    public IEnumerator BeforeTurnEnd(bool isPlayer, List<string> units)
    {
        foreach (var unit in units)
        {
            if (unit == _target && BattleManager.GetUnit(unit).HasBuff(buffType))
            {
                var info = new BuffAction()
                {
                    BuffType = buffType,
                    Stacks = 1,
                    ByStack = true,
                    Source = _target,
                    Target = _target
                };
            
                yield return BattleManager.Instance.LoseBuff(info);
            }
        }
    }
}
