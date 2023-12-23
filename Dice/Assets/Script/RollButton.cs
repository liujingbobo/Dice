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
        EventsManager.Instance.onSwitchingState.AddListener(_ =>
        {
            if (_ == BattleState.Waiting)
            {
                btn.onClick.RemoveAllListeners();
                btn.interactable = true;
                btn.onClick.AddListener(() =>
                {
                    BattleManager.Instance.EndPlayerTurn();
                    btn.interactable = false;
                    btn.onClick.RemoveAllListeners();
                });
            }
        });
    }
}
