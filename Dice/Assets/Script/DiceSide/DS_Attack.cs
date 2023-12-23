using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "AttackSide", menuName = "DiceSide/Attack", order = 0)]
public class DS_Attack : DiceSideEffect
{
    [SerializeField] private int value;

    public override IEnumerator TakeAction(ActionInfo actionInfo)
    {
        var dmgInfo = new DamageInfo(actionInfo.Source, actionInfo.Target, value, false, this);

        yield return Action(dmgInfo);
    }

    IEnumerator Action(DamageInfo info)
    {
        yield return BattleManager.Instance.DealDamage(info);
    }
}