using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EffectTrigger
{
}
public interface BeforeTurnStart : EffectTrigger
{
    public IEnumerator BeforeTurnStart(string btUnit);
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
    public IEnumerator BeforeTurnEnd(string ID);
}
