using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class RollButton : MonoBehaviour
{
    public Button btn;
    
    private void Start()
    {
        btn.interactable = false;
        BattleManager.Instance.state.Subscribe(_ =>
        {
            if (_ == BattleState.Player)
            {
                btn.onClick.RemoveAllListeners();
                btn.interactable = true;
                btn.onClick.AddListener(() =>
                {
                    BattleManager.Instance.Roll();
                    btn.interactable = false;
                    btn.onClick.RemoveAllListeners();
                });
            }
        });
    }
}
