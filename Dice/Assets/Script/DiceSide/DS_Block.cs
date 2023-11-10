using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiceSide", menuName = "DiceSide/Block", order = 1)]
public class DS_Block : DiceSide
{
    public int Value;
    
    public override IEnumerator TakeAction(Unit self, Unit target)
    {
        Debug.Log($"{self.ID} gain {Value} blocks.");

        BattleManager.Instance.StartCoroutine(BattleManager.Instance.GainBlock(new GainBlockInfo()
            {
                Value = Value,
                Source = self.ID,
                Target = self.ID
            }));
        
        yield return null;
    }
}
