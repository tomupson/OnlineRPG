using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoalUI : MonoBehaviour
{
    Goal goal;

    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI currentProgressText;
    [SerializeField] private TextMeshProUGUI requiredProgressText;
    [SerializeField] private Image progressBar;

    public void Setup(Goal goal)
    {
        this.goal = goal;
        progressBar.fillAmount = (float)goal.CurrentAmount / (float)goal.RequiredAmount;
        descriptionText.text = goal.Description;
        currentProgressText.text = goal.CurrentAmount.ToString();
        requiredProgressText.text = goal.RequiredAmount.ToString();
    }
}