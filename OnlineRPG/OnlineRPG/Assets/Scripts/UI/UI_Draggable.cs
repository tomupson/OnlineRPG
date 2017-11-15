using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class UI_Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector2 offset;
    CanvasGroup canvasGroup;

    [SerializeField] private Transform target;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (target == null) target = transform;

        offset = eventData.position - (Vector2)transform.parent.parent.position;
        target.position = eventData.position - offset;
        canvasGroup.blocksRaycasts = false;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        target.position = eventData.position - offset;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
    }
}