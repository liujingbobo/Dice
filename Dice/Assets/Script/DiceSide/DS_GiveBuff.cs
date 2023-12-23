using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ToxicSide", menuName = "DiceSide/GiveBuff", order = 0)]
public class DS_GiveBuff : DiceSideEffect
{
    [SerializeField] private int value;
    [SerializeField] private Effect effect;
    
    public override IEnumerator TakeAction(ActionInfo actionInfo)
    {
        var buffAction = new BuffAction()
        {
            BuffType = effect.buffType,
            ByStack = effect.byStack,
            Stacks = value,
            Source = actionInfo.Source,
            Target = actionInfo.Target
        };
        
        yield return BattleManager.Instance.GainBuff(buffAction);
    }
}
