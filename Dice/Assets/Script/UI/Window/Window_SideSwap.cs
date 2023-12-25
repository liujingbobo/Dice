using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Window_SideSwap : MonoBehaviour
{
    [SerializeField] private UIView_Dice dicePresent;
    [SerializeField] private int width = 2;
    [SerializeField] private int height = 3;
    
    public void Open(List<int> indexes)
    {
        gameObject.SetActive(true);
    }
    
    
    
    
}
