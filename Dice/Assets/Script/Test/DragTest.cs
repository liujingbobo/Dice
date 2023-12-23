using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragTest : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerMoveHandler
{
    public GameObject Target;
    public GameObject cur;

    private void Update()
    {
        print($"position: {transform.position}");
        print($"rectp: {transform.rectTransform().position}");
    }

    public void OnDrag(PointerEventData eventData)
    {
        print($"Mouse {Input.mousePosition}");
        print($"Event {eventData.position}");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        print($"Pointer {eventData.position}");
    }
}
