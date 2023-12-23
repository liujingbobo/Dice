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

public interface BeforeHeal : EffectTrigger
{
    public IEnumerator BeforeHeal(HealInfo healInfo);
}

public interface AfterHeal : EffectTrigger
{
    public IEnumerator AfterHeal(HealInfo healInfo);
}

public interface BeforeGainBlock : EffectTrigger
{
    public IEnumerator BeforeGainBlock(GainBlockInfo info);
}

public interface AfterGainBlock : EffectTrigger
{
    public IEnumerator AfterGainBlock(GainBlockInfo info);
}

public interface BeforeLoseBlock : EffectTrigger
{
    public IEnumerator BeforeLoseBlock(LoseBlockInfo info);
}

public interface AfterLoseBlock : EffectTrigger
{
    public IEnumerator AfterLoseBlock(LoseBlockInfo info);
}

public interface OnUnitDead : EffectTrigger
{
    public IEnumerator OnUnitDead(string id);
}

