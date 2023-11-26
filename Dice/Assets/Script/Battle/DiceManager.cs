using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public CacheLayoutPattern cache;
    
    public List<RTDiceData> Dices;

    [SerializeField] private float rollGap;

    [SerializeField] private ReactiveProperty<int> refreshTime;

    public void Init(List<RTDiceData> dices)
    {
        var triplets = cache.Cache.UseByIndex(dices);
        
        foreach (var triplet in triplets)
        {
            if (triplet.Key.TryGetComponent<UI_Dice>(out var dice))
            {
                // dice.Init(triplet.Value1);
            }
        }
    }
    public IEnumerator StartTurn(List<int> results, int refreshTime)
    {
        var pairs = cache.Cache.UsingByIndex().ToList();
        foreach (var pair in pairs)
        {
            if (pair.Key.TryGetComponent<UI_Dice>(out var dice))
            {
                if (pair.Value == pairs.Count - 1)
                {
                    yield return dice.MockRoll(results[pair.Value]);
                }
                else
                {
                    StartCoroutine(dice.MockRoll(results[pair.Value]));
                    yield return rollGap;
                }
            }
        }
    }
}
