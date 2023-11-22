using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UI_Dice : MonoBehaviour
{
    public GameObject SidePrefab;

    public RTDiceData Target;

    [SerializeField] private CacheLayoutPattern cache;
    
    public void Init(RTDiceData data)
    {
        Target = data;
        foreach (var p in cache.Cache.Use(data.Dices))
        {
            if (p.Key.TryGetComponent<UI_Dice>(out var dice))
            {
                
            }
        }
    }
}
