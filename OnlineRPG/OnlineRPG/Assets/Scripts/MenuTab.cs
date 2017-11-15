using UnityEngine;
using UnityEngine.UI;

public class MenuTab : MonoBehaviour
{
    [SerializeField] private Color highlightColour;
    [SerializeField] private bool isDefault = false;

    private Color normalColour;
    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
        normalColour = image.color;
    }

    void Start()
    {
        if (isDefault)
        {
            Highlight();
        }
    }

    public void Highlight()
    {
        foreach (MenuTab tab in FindObjectsOfType<MenuTab>())
        {
            if (tab != this)
            {
                tab.Unhighlight();
            }
        }

        image.color = highlightColour;
    }

    public void Unhighlight()
    {
        image.color = normalColour;
    }
}