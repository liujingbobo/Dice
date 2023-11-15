using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BeforeTurnStart
{
    public IEnumerator Trigger(Unit unit);
}

public interface AfterDealDamage
{
    public IEnumerator AfterDealDamage(DamageInfo DmgInfo);
}

public interface BeforeTurnEnd
{
    public IEnumerator BeforeTurnEnd(DamageInfo info);
}