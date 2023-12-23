using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public interface SelectableUnit
{
    public string Target { get; }
}

public class UIView_PlayerUnit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, SelectableUnit
{
    [SerializeField] private TMP_Text hp;
    [SerializeField] private TMP_Text br;

    public string Target => _target;
    private string _target;

    [SerializeField] private Slider hpSlider;
    [SerializeField] private GameObject highlight;
    [SerializeField] private CacheLayoutPattern buffCache;
    [SerializeField] private Selectable hasBarrier;
    [SerializeField] private CacheLayoutPattern rolledDiceCache;

    public void Init(string id)
    {
        _target = id;
        var unit = BattleManager.Instance.Units[id].Value;

        hpSlider.maxValue = unit.maxHp;
        hpSlider.value = unit.maxHp;

        BattleManager.Instance.Units[id].Subscribe(Refresh).AddTo(this);
    }

    private void Refresh(BTUnit unit)
    {
        print($"Refresh unit {unit.id}");
        hpSlider.value = unit.hp;
        hp.text = unit.hp.ToString();

        var buffs = unit.buffs;

        var effects = buffs.Where(_ => _.Value > 0)
            .Select(buffInfo => (buffInfo, BuffManager.Instance.GetBuffInfo(unit.id, buffInfo.Key)))
            .Where(_ => _.Item2.showOnUI).ToList();

        var pairs = buffCache.Cache.UseByIndex(effects).ToList();

        foreach (var pair in pairs)
        {
            if (pair.Key.GetComponentInChildren<UI_Buff>() is { } tn)
            {
                tn.Refresh(unit, pair.Value1.buffInfo.Key, pair.Value1.Item2);
            }
        }

        if(hasBarrier) hasBarrier.interactable = unit.br > 0;
        if (br) br.text = unit.br > 0 ? unit.br.ToString() : string.Empty;
        rolledDiceCache.Cache.CloseAll();
        foreach (var pair in rolledDiceCache.Cache.Use(unit.Dices))
        {
            if (!pair.Key.TryGetComponent<UI_BattleSide>(out var side)) continue;
            
            if (pair.Value.HasSide())
            {                
                side.Init(pair.Value.GetSide());
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        highlight.SetActive(!DiceManager.Instance.dragging ||
                            B.BelongsToTarget(_target, DiceManager.Instance.CurrentDragging.GetSide().Side.TargetType));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(highlight.activeSelf) highlight.SetActive(false);
    }
}