using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIView_UnitStatus : MonoBehaviour
{
    public TMP_Text HP;
    public TMP_Text BR;

    public Slider HPSlider;

    public void Init(string ID)
    {
        var unit = BattleManager.Instance.Units[ID].Value;
        
        HPSlider.maxValue = unit.MaxHP;
        HPSlider.value = unit.MaxHP;

        BattleManager.Instance.Units[ID].Subscribe(_ =>
        {
            HPSlider.value = _.HP;
            HP.text = _.HP.ToString();
        }).AddTo(this);
    }
}
