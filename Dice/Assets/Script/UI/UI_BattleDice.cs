using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UI_BattleDice : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RTDiceData target;

    private int Index => target.Index;

    [SerializeField] private Button rerollButton;

    [SerializeField] private GameObject sideFollower;

    [SerializeField] private float speed = 50;

    [SerializeField] private CanvasGroup usedCanvas;

    [SerializeField] private UIView_Dice view;

    private Vector3 _prePos;

    private void Update()
    {
        if (sideFollower && sideFollower.activeSelf)
        {
            sideFollower.rectTransform().position = Vector3.Lerp(sideFollower.rectTransform().position, _prePos, Time.deltaTime * speed);
        }
    }

    public void Awake()
    {
        rerollButton.onClick.AddListener(() => BattleManager.Instance.ReRoll(Index));
    }

    public void Init(RTDiceData data)
    {
        target = data;
        
        view.Fill(data.Sides.Select(_ => _.Side).ToList());
        
        Refresh(data);
    }

    public void Refresh(RTDiceData data)
    {
        target = data;
        rerollButton.gameObject.SetActive(data.Rerollable);
        usedCanvas.alpha = data.Used ? 0.5f : 1f;
    }

    public void SetHighLight(List<int> sideIndexes)
    {
        view.SetHighLight(sideIndexes);
    }

    public IEnumerator MockRoll(int result)
    {
        yield return view.MockRoll(result);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!target.Used)
        {
            // var gap = eventData.position - sideFollower.rectTransform().position;
            _prePos = eventData.position;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!target.Used && !DiceManager.Instance.dragging)
        {
            DiceManager.Instance.dragging = true;
            DiceManager.Instance.CurrentDragging = target;
            sideFollower.SetActive(true);
            sideFollower.rectTransform().position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!target.Used && DiceManager.Instance.dragging)
        {
            DiceManager.Instance.dragging = false;

            sideFollower.SetActive(false);
        
            var pointer = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            var results = new List<RaycastResult>();

            EventSystem.current.RaycastAll(pointer, results);

            if (results.Count > 0)
            {
                foreach (RaycastResult result in results)
                {
                    if (result.gameObject.TryGetComponent<UIView_PlayerUnit>(out var unit))
                    {
                        var side = target.GetSide().Side;
                        
                        if(B.BelongsToTarget(unit.Target, side.TargetType))
                        {
                            BattleManager.Instance.Cast(Index, unit.Target);
                            return;
                        }
                    }
                }
            }
        }
    }
}
