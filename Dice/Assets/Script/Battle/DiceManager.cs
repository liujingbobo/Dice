using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public CacheLayoutPattern cache;
    
    [SerializeField] private float rollGap;

    public void Init(List<RTDiceData> dices)
    {
        var triplets = cache.Cache.UseByIndex(dices);
        
        foreach (var triplet in triplets)
        {
            if (triplet.Key.TryGetComponent<UI_Dice>(out var dice))
            {
                dice.Init(triplet.Value1);
            }
        }
    }
    
    public IEnumerator Roll(List<(int dice, int result)> infos)
    {
        var pairs = cache.Cache.UsingByIndex().ToList();
        for (int i = 0; i < infos.Count; i++)
        {
            var info = infos[i];
            
            if (pairs[info.dice].Key.TryGetComponent<UI_Dice>(out var dice))
            {
                if (i == infos.Count - 1)
                {
                    yield return dice.MockRoll(info.result);
                }
                else
                {
                    StartCoroutine(dice.MockRoll(info.result));
                    yield return rollGap;
                }
            }
        }
    }

    public void RefreshAll(List<RTDiceData> datas)
    {
        var pairs = cache.Cache.UsingByIndex().PairWith(datas);
        foreach (var pair in pairs)
        {
            if (pair.Item1.Key.TryGetComponent<UI_Dice>(out var dice))
            {
                dice.Refresh(pair.Item2);
            }
        }
    }
}
