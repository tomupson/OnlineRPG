using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private GameObject tooltip;

    Item item;
    string data;
    TMP_Text tooltipText;

    void Start()
    {
        tooltipText = tooltip.GetComponentInChildren<TMP_Text>();
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
        data = string.Format("<color=#D3D3D3FF><size=15>{0}</size>\n<size=12>Right click for more information.</size></color>", item.Name);
        tooltipText.text = data;
    }
}