using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Buff", menuName = "Buff/Toxic", order = 0)]
public class BF_Toxic : Effect, BeforeTurnStart
{
    [SerializeField] private int damageMultiplier = 1;

    private string _source;
    private string _owner;
    
    public override IEnumerator AddBuff(BuffAction buffAction)
    {
        _source = buffAction.Source;
        _owner = buffAction.Target;
        yield break;
    }

    public IEnumerator BeforeTurnStart(bool isPlayer, List<string> units)
    {
        foreach (var unit in units)
        {
            if (unit == _owner && B.HasBuff(_owner, BuffType.Toxic, out int stacks))
            {
                var info = new DamageInfo(_source, _owner, stacks * damageMultiplier, false,this);

                yield return BattleManager.Instance.DealDamage(info);

                yield return BattleManager.Instance.LoseBuff(new BuffAction()
                {
                    BuffType = buffType,
                    Target = _owner,
                    Source = _source,
                    Stacks = 1,
                    ByStack = true
                });
            }
        }
    }
}

// 