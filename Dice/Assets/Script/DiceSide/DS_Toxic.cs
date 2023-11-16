using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ToxicSide", menuName = "DiceSide/Toxic", order = 0)]
public class DS_Toxic : DiceSideEffect
{
    public int value;

    public override IEnumerator TakeAction(ActionInfo actionInfo)
    {
        BuffAction buffAction = new BuffAction()
        {
            BuffType = BuffType.Toxic,
            ByStack = true,
            Stacks = value,
            Source = actionInfo.Source,
            Target = actionInfo.Target
        };
        
        yield return BattleManager.Instance.GainBuff(buffAction);
    }
}
