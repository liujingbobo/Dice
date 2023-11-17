using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "HealSide", menuName = "DiceSide/Attack", order = 0)]
public class DS_Heal : DiceSideEffect
{
    public List<int> Values;
    public override int MaxLevel => Values.Count - 1;

    public override IEnumerator TakeAction(ActionInfo actionInfo)
    {
        HealInfo info = new HealInfo()
        {
            Value = Values[actionInfo.Level],
            Source = actionInfo.Source,
            Target = actionInfo.Target,
        };

        yield return BattleManager.Instance.StartCoroutine(Action(info));
    }

    IEnumerator Action(HealInfo info)
    {
        yield return BattleManager.Instance.StartCoroutine(BattleManager.Instance.Heal(info));
    }
}
