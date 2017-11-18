using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private GameObject tooltip;

    IOptionsInfo info;
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

    public void Activate(IOptionsInfo info)
    {
        this.info = info;
        tooltipText.text = info.Description;
        tooltip.SetActive(true);
    }

    public void Deactivate()
    {
        tooltip.SetActive(false);
    }
}