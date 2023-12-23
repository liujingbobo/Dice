using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIView_Dice : MonoBehaviour
{
    [SerializeField] private CacheLayoutPattern cache;
    
    [SerializeField] private int randomDisplayTime = 10;
    
    [SerializeField] private float randomDisplayGap = 0.1f;
    public void Fill(List<DiceSideEffect> sidesEffects)
    {
        var sides = cache.Cache.Use(sidesEffects).ToList();
        
        foreach (var p in sides)
        {
            if (p.Key.TryGetComponent<UI_BattleSide>(out var side))
            {
                // side.Init(p.Value);
            }
        }
    }
    
    public void SetHighLight(List<int> sideIndexes)
    {
        var curUsing = cache.Cache.UsingByIndex();
        foreach (var pair in curUsing)
        {
            if (pair.Key.GetComponent<UI_BattleSide>() is { } s)
            {
                s.SetHighLight(sideIndexes.Contains(pair.Value));
            }
        }
    }
    
    public IEnumerator MockRoll(int result)
    {
        var allSide = cache.Cache.UsingByIndex().ToList();
        
        allSide.ForEach(_ =>
        {
            if (_.Key.TryGetComponent<UI_BattleSide>(out var side))
            {
                side.SetSelected(false);
            }
        });
        
        var left = randomDisplayTime;
        while (left > 0)
        {
            int ran = Random.Range(0, allSide.Count);
            allSide.ForEach(_ =>
            {
                if (_.Key.TryGetComponent<UI_BattleSide>(out var side))
                {
                    side.SetHighLight(_.Value == ran);
                }
            });
            yield return new WaitForSeconds(randomDisplayGap);
            left--;
        }
        allSide.ForEach(_ =>
        {
            if (_.Key.TryGetComponent<UI_BattleSide>(out var side))
            {
                side.SetHighLight(_.Value == result);
            }
        });
        yield return new WaitForSeconds(randomDisplayGap);
        allSide.ForEach(_ =>
        {
            if (_.Key.TryGetComponent<UI_BattleSide>(out var side))
            {
                side.SetSelected(_.Value == result);
            }
        });
        
        // Refresh Reroll
    }

}
