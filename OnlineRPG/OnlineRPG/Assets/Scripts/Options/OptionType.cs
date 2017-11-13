using UnityEngine;

public class OptionType : MonoBehaviour
{
    [SerializeField] private GameObject myPanel;
    [SerializeField] private bool isDefault = false;

    void Start()
    {
        if (!isDefault)
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