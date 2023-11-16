using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class B
{
    public static void AddEventTriggers(Effect ef)
    {
        BattleEvents.Instance.AddTrigger(ef);
    }

    public static bool HasBuff(string unitID, BuffType buffType, out int stacks)
    {
        var unit = BattleManager.GetUnit(unitID);
        stacks = unit.GetBuffStack(buffType);
        return unit.HasBuff(buffType);
    }

    public static BTUnit GetUnit(string unit)
    {
        return BattleManager.GetUnit(unit);
    }

    public static IEnumerator DO<T>(Func<T, IEnumerator> act)
    {
        yield return BattleEvents.Instance.DO<T>(act);
    }
}
