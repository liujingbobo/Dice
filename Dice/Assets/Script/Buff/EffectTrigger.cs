using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EffectTrigger
{
}
public interface BeforeTurnStart : EffectTrigger
{
    public IEnumerator BeforeTurnStart(bool isPlayer, List<string> units);
}

public interface AfterDealDamage : EffectTrigger
{
    public IEnumerator AfterDealDamage(DamageInfo DmgInfo);
}

public interface BeforeDealDamage : EffectTrigger
{
    public IEnumerator BeforeDealDamage(DamageInfo DmgInfo);
}

public interface BeforeTurnEnd : EffectTrigger
{
    public IEnumerator BeforeTurnEnd(bool isPlayer, List<string> units);
}
