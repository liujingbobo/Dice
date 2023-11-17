using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiceSide", menuName = "DiceSide/Block", order = 1)]
public class DS_Block : DiceSideEffect
{
    public List<int> Values;

    public override int MaxLevel => Values.Count - 1;

    public override IEnumerator TakeAction(ActionInfo actionInfo)
    {
        var info = new GainBlockInfo()
        {
            Value = Values[actionInfo.Level],
            Source = actionInfo.Source,
            Target = actionInfo.Source
        };
        
        yield return Action(info);
    }
    
    IEnumerator Action(GainBlockInfo info)
    {
        yield return BattleManager.Instance.GainBlock(info);
    }
}
