using UnityEngine;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public Item item;
    [HideInInspector] public int amount;
    [HideInInspector] public int slot;

    private Inventory inventory;
    private Transform content;
    private Tooltip tooltip;

    private Vector2 offset;
    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        inventory = FindObjectOfType<Inventory>();
        content = transform.parent.parent;
        tooltip = inventory.GetComponent<Tooltip>();
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            offset = eventData.position - new Vector2(transform.position.x, transform.position.y);
            transform.SetParent(content);
            transform.position = eventData.position - offset;
            canvasGroup.blocksRaycasts = false;
        }
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            transform.position = eventData.position - offset;
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        Transform slotTransform = inventory.slots[slot].transform;
        transform.SetParent(slotTransform);
        transform.position = slotTransform.position;
        canvasGroup.blocksRaycasts = true;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        tooltip.Activate(item);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        tooltip.Deactivate();
    }
}