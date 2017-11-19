using UnityEngine;
using UnityEngine.EventSystems;

public class SettingTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    IOptionsInfo info;
    Tooltip tooltip;

    void Start()
    {
        tooltip = FindObjectOfType<Tooltip>();
        info = transform.parent.GetComponentInParent<IOptionsSetting>().info;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.Activate(info);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.Deactivate();
    }
}