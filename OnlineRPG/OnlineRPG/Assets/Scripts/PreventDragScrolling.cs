using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PreventDragScrolling : ScrollRect
{
    public override void OnBeginDrag(PointerEventData data) { }
    public override void OnDrag(PointerEventData data) { }
    public override void OnEndDrag(PointerEventData data) { }
}