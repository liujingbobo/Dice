using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealSide", menuName = "DiceSide/Attack", order = 0)]
public class DS_Heal : DiceSideEffect
{
    public int value;
    
    public override IEnumerator TakeAction(ActionInfo actionInfo)
    {
        HealInfo info = new HealInfo()
        {
            Value = value,
            Source = actionInfo.Source.ID,
            Target = actionInfo.Target.ID,
        };

        yield return BattleManager.Instance.StartCoroutine(Action(info));
    }

    IEnumerator Action(HealInfo info)
    {
        yield return BattleManager.Instance.StartCoroutine(BattleManager.Instance.Heal(info));
    }
}
