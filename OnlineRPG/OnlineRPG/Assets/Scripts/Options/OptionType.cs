using UnityEngine;

public class OptionType : MonoBehaviour
{
    [SerializeField] private GameObject myPanel;

    void Start()
    {
        Hide();
    }

    public void Show()
    {
        foreach (OptionType typeTab in FindObjectsOfType<OptionType>())
        {
            typeTab.Hide();
        }

        myPanel.SetActive(true);
    }

    public void Hide()
    {
        myPanel.SetActive(false);
    }
}