using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UI_BattleSide : MonoBehaviour
{
    [SerializeField] private UIView_Side view;

    private BTSideData _target;

    public void Init(BTSideData content)
    {
        _target = content;
        view.Fill(content.Side);
    }

    public void ManuelReset()
    {
        SetHighLight(false);
        SetSelected(true);
    }

    public void SetHighLight(bool s)
    {
        view.SetHighLight(s);
    }

    public void SetSelected(bool s)
    {
        view.SetSelected(s);
    }
}