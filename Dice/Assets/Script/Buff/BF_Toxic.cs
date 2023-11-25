using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Buff", menuName = "Buff/Effect", order = 0)]
public class BF_Toxic : Effect, BeforeTurnStart
{
    public int damageMultiplier = 1;

    public string source;
    public string owner;
    
    public override IEnumerator AddBuff(BuffAction buffAction)
    {
        source = buffAction.Source;
        owner = buffAction.Target;
        yield break;
    }

    public IEnumerator BeforeTurnStart(bool isPlayer, List<string> units)
    {
        foreach (var unit in units)
        {
            if (unit == owner && B.HasBuff(owner, BuffType.Toxic, out int stacks))
            {
                var state = B.GetUnit(owner).Buffs[BuffType];
            
                var info = new DamageInfo()
                {
                    SourceType = SourceType.Buff,
                    Source = source,
                    Target = owner,
                    Value = stacks * damageMultiplier,
                    BuffType = BuffType.Toxic
                };

                yield return BattleManager.Instance.DealDamage(info);
            }
        }
    }
}

// 