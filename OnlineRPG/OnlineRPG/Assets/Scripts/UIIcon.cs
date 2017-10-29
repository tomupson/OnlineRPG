using UnityEngine;
using UnityEngine.UI;

public class UIIcon : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Color activeColor;

    private Color defaultColor;
    private Image background;

    bool isActivePanel = false;

    void Start()
    {
        panel.SetActive(false);
        isActivePanel = false;
        background = GetComponent<Image>();
        defaultColor = background.color;
    }

    void Update()
    {
        if (isActivePanel)
        {
            background.color = activeColor;
        } else
        {
            background.color = defaultColor;
        }
    }

    public void ShowPanel()
    {
        panel.SetActive(true);
        isActivePanel = true;
        UIIcon[] panels = FindObjectsOfType<UIIcon>();
        foreach (UIIcon panel in panels)
        {
            if (panel != this)
            {
                panel.HidePanel();
            }
        }
    }

    public void HidePanel()
    {
        panel.SetActive(false);
        isActivePanel = false;
    }
}