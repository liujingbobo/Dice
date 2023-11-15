using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiceSide", menuName = "DiceSide/Block", order = 1)]
public class DS_Block : DiceSideEffect
{
    public int Value;
    
    public override IEnumerator TakeAction(ActionInfo actionInfo)
    {
        var info = new GainBlockInfo()
        {
            Value = Value,
            Source = actionInfo.Source.ID,
            Target = actionInfo.Source.ID
        };
        
        yield return BattleManager.Instance.StartCoroutine(Action(info));
    }
    
    IEnumerator Action(GainBlockInfo info)
    {
        yield return BattleManager.Instance.StartCoroutine(BattleManager.Instance.GainBlock(info));
    }
}
