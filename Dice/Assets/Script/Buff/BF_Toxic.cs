using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff", menuName = "Buff/Effect", order = 0)]
public class BF_Toxic : BuffEffect
{
    public override IEnumerator GiveBuff(GiveBuffInfo giveBuffInfo)
    {
        BattleEvents.Instance.BeforeGiveBuff.Invoke(giveBuffInfo);
        
        BattleEvents.Instance.BeforeTurnStart.AddListener(_ =>
        {
            // Unfinished:
            // if (_.ID == giveBuffInfo.target)
            // {
            //     var buffInfo = BattleManager.Instance.Units[giveBuffInfo.target].Value.Buffs[BuffType];
            //
            //     var info = new DamageInfo()
            //     {
            //         Source = buffInfo.Source,
            //         Target = giveBuffInfo.target,
            //         Value = buffInfo.Stacks * 1
            //     };
            //
            //     BattleManager.Instance.StartCoroutine(BattleManager.Instance.DealDamage(info));
            // }
        });

        yield break;
    }
}
