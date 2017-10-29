using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Image skillImage;
    [SerializeField] private Image xpBar;
    [SerializeField] private float xpAdditionRate;

    private BaseSkill skill;

    private float xpToAdd = 0;
    private float xpLastUpdate = 0;
    private bool animating = false;

    public void OpenSkillInfo()
    {

    }

    public void Setup(BaseSkill skill)
    {
        this.skill = skill;
        levelText.text = skill.CurrentLevel.ToString();
        skillImage.sprite = skill.icon;
        xpBar.fillAmount = skill.PercentageIntoLevel();
        skill.OnExperienceChanged += OnSkillChanged;
        xpLastUpdate = skill.TotalExperience;
    }

    void OnSkillChanged()
    {
        levelText.text = skill.CurrentLevel.ToString();
        xpBar.fillAmount = skill.PercentageIntoLevel();
        xpToAdd += (skill.TotalExperience - xpLastUpdate);
        xpLastUpdate = skill.TotalExperience;

        //if (!animating)
        //{
        //    StartCoroutine(AnimateXPBar());
        //}
    }

    //IEnumerator AnimateXPBar()
    //{
    //    animating = true;
    //    int level = skill.CurrentLevel - 1;
    //    float requiredForNextLevel = skill.TotalExperienceRequiredForNextLevel - skill.ExperienceForLevel(skill.CurrentLevel-1);
    //    while (xpToAdd > 0)
    //    {
    //        levelText.text = (level + 1).ToString();
    //        if (xpToAdd < xpAdditionRate)
    //        {
    //            xpBar.fillAmount += xpToAdd / requiredForNextLevel;
    //            xpToAdd -= xpToAdd;
    //            animating = false;
    //            yield return null;
    //        }

    //        xpBar.fillAmount += (xpAdditionRate / requiredForNextLevel);
    //        if (xpBar.fillAmount >= 1)
    //        {
    //            level++;
    //            requiredForNextLevel = skill.ExperienceForLevel(level);
    //        }
    //        xpToAdd -= xpAdditionRate;
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //}
}