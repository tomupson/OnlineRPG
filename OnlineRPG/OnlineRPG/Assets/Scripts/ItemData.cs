using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [HideInInspector] public Item item;
    private int amount;
    public int Amount
    {
        get
        {
            return amount;
        } set
        {
            amount = value;
            TextMeshProUGUI txt = GetComponentInChildren<TextMeshProUGUI>();
            if (value <= 1)
            {
                txt.text = "";
            } else
            {
                txt.text = value.ToString();
            }
        }
    }

    [HideInInspector] public int slot;

    Inventory inventory;
    Transform content;
    Tooltip tooltip;

    Vector2 offset;
    CanvasGroup canvasGroup;

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
        //tooltip.Activate(item);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        //tooltip.Deactivate();
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            inventory.SetInfo(item);
            inventory.ShowInfo();
        }
    }
}