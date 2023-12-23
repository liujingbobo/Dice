using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "HealSide", menuName = "DiceSide/Heal", order = 0)]
public class DS_Heal : DiceSideEffect
{
    [SerializeField] private int value;

    public override IEnumerator TakeAction(ActionInfo actionInfo)
    {
        HealInfo info = new HealInfo(actionInfo.Source, actionInfo.Target, value);

        yield return BattleManager.Instance.StartCoroutine(Action(info));
    }

    IEnumerator Action(HealInfo info)
    {
        yield return BattleManager.Instance.StartCoroutine(BattleManager.Instance.Heal(info));
    }
}
