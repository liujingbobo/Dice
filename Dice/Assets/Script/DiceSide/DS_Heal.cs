using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealSide", menuName = "DiceSide/Attack", order = 0)]
public class DS_Heal : DiceSide
{
    public int value;
    
    public override IEnumerator TakeAction(Unit self, Unit target)
    {
        HealInfo info = new HealInfo()
        {
            Value = value,
            Source = self.ID,
            Target = target.ID,
        };

        yield return BattleManager.Instance.StartCoroutine(BattleManager.Instance.Heal(info));
        
        Debug.Log($"{self.ID} heal {value};");
        
        yield return null;
    }
}
