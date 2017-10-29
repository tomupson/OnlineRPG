using UnityEngine;

public class IconsManager : MonoBehaviour
{
    [SerializeField] private GameObject skillsPanel;
    [SerializeField] private GameObject bagPanel;
    [HideInInspector] public bool skillsMenuOpen = false;
    [HideInInspector] public bool bagMenuOpen = false;

    void Start()
    {
        skillsMenuOpen = false;
        bagMenuOpen = false;
    }

    public void SkillsIconPressed()
    {
        skillsMenuOpen = !skillsMenuOpen;
        skillsPanel.SetActive(skillsMenuOpen);
    }

    public void BagIconPressed()
    {
        bagMenuOpen = !bagMenuOpen;
        bagPanel.SetActive(bagMenuOpen);
    }
}