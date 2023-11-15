using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff", menuName = "Buff/ToxicAttack", order = 0)]
public class BF_ToxicAttack : Effect, AfterDealDamage
{
    public int damageMultiplier = 1;
    
    public string Source;
    
    public override IEnumerator Init(BuffAction buffAction)
    {
        yield break;
    }

    public IEnumerator AfterDealDamage(DamageInfo DmgInfo)
    {
        if (Source == DmgInfo.Source && DmgInfo.SourceType == SourceType.Unit)
        {
            var state = BattleManager.Instance.Units[DmgInfo.Target].Value.Buffs[BuffType];

            // give buff
            yield break;
        }
    }
}
