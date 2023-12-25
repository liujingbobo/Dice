using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class B
{
    public static void AddEventTriggers(Effect ef, string owner)
    {
        EventsManager.Instance.AddTrigger(ef, owner);
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
        yield return EventsManager.Instance.Do<T>(act);
    }

    public static bool BelongsToTarget(string target, TargetType targetType)
    {
        if (targetType == TargetType.Self)
        {
            return target == BattleManager.Instance.playerRP.Value.id;
        }
        if (targetType == TargetType.Opponent)
        {
            return target != BattleManager.Instance.playerRP.Value.id;
        }
        return true;
    }

    public static bool IsMainPlayer(string id)
    {
        return id == BattleManager.Instance.playerRP.Value.id;
    }

    public static bool IsPlayerUnit(string id)
    {
        return id == BattleManager.Instance.playerRP.Value.id;
    }
}
