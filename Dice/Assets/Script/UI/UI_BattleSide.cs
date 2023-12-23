using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UI_BattleSide : MonoBehaviour
{
    private UIView_Side view;

    private RTSideData _target;

    public void Init(RTSideData content)
    {
        _target = content;
        view.Fill(content.Side);
    }

    public void Reset()
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