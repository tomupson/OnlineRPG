using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private GameObject tooltip;

    private Item item;
    private string data;
    private TextMeshProUGUI tooltipText;

    void Start()
    {
        tooltipText = tooltip.GetComponentInChildren<TextMeshProUGUI>();
    }
    
    void Update()
    {
        if (tooltip.activeSelf)
        {
            tooltip.transform.position = Input.mousePosition;
        }
    }

    public void Activate(Item item)
    {
        this.item = item;
        ConstructDataString();
        tooltip.SetActive(true);
    }

    public void Deactivate()
    {
        tooltip.SetActive(false);
    }

    public void ConstructDataString()
    {
        data = string.Format("<color=#D3D3D3FF><size=15>{0}</size></color>\n" +
            "<color=#D3D3D3FF><size=12>{1}</size></color>\n" +
            "<color=#D3D3D3FF><size=12>{2}</size></color>", item.Name, item.Description, string.Join(", ", item.Uses));
        tooltipText.text = data;
    }
}