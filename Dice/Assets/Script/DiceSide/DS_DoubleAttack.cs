using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AttackSide", menuName = "DiceSide/MultipleAttack", order = 0)]
public class DS_DoubleAttack : DiceSideEffect
{
    public List<int> value;
    public override IEnumerator TakeAction(ActionInfo actionInfo)
    {
        foreach (var dmg in value)
        {
            if (BattleManager.GetUnit(actionInfo.Target).dead)
            {
                yield break;
            }

            var dmgInfo = new DamageInfo(actionInfo.Source, actionInfo.Target, dmg, false, this);
         
            yield return Action(dmgInfo);
        }
    }

    IEnumerator Action(DamageInfo info)
    {
        yield return BattleManager.Instance.DealDamage(info);
    }
}
