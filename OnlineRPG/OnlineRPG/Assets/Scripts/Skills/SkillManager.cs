using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
    #region Singleton
    public static SkillManager singleton;
    #endregion

    CharacterStats stats;

    [SerializeField] private GameObject skillMenu;
    [SerializeField] private GameObject skillPrefab;
    [SerializeField] private GameObject rightMenu;
    [SerializeField] private Transform contentTransform;
    [SerializeField] private GameObject skillXpGained;

    [Space]

    [Header("Info")]
    [SerializeField] private Image skillImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text currentXpText;
    [SerializeField] private TMP_Text requiredXpText;
    [SerializeField] private Image xpBar;

    BaseSkill focusedSkill;

    [HideInInspector] public bool open = false;
    bool infoSet;

    Queue<Tuple<BaseSkill, float>> skillXpGainedQueue = new Queue<Tuple<BaseSkill, float>>();
    bool animating = false;

    void Awake()
    {
        if (singleton != null)
        {
            Debug.LogError("More than one SkillManager in the scene!");
            return;
        }

        singleton = this;
    }

    void Start()
    {
        stats = Player.LocalPlayer.GetComponent<CharacterStats>();
        animating = false;

        CloseSkillMenu();
        infoSet = false;
    }

    public void ManageEvents()
    {
        if (focusedSkill == null) return;

        focusedSkill.OnExperienceChanged += RefreshInfo;

        foreach (BaseSkill skill in stats.skills)
        {
            if (skill != focusedSkill)
            {
                skill.OnExperienceChanged -= RefreshInfo;
            }
        }
    }

    public void CreateSkillFor(BaseSkill skill)
    {
        GameObject skillGO = Instantiate(skillPrefab, contentTransform, false);
        SkillUI skillUI = skillGO.GetComponent<SkillUI>();
        skillUI.Setup(skill);
    }

    public void GrantXPToSkill(BaseSkill skill, float xp)
    {
        skill.GrantXP(xp);
        skillXpGainedQueue.Enqueue(new Tuple<BaseSkill, float>(skill, xp));
        if (!animating)
        {
            StartCoroutine(ShowXPGranted());
        }
        animating = true;
    }

    IEnumerator ShowXPGranted()
    {
        while (skillXpGainedQueue.Count > 0)
        {
            Tuple<BaseSkill, float> currentSkillXpGained = skillXpGainedQueue.Dequeue();
            skillXpGained.SetActive(true);
            Image img = skillXpGained.GetComponentInChildren<Image>();
            TMP_Text txt = skillXpGained.GetComponentInChildren<TMP_Text>();
            txt.text = string.Format("+{0}XP", currentSkillXpGained.Item2);
            img.sprite = currentSkillXpGained.Item1.icon;
            Animator animator = skillXpGained.GetComponent<Animator>();

            animator.SetBool("IsShowing", true);

            yield return new WaitForSeconds(3f);

            animator.SetBool("IsShowing", false);

            yield return null;
        }

        animating = false;
    }

    public void ShowInfo()
    {
        if (!infoSet)
        {
            Debug.LogError("You must set the Item information before showing it!");
            return;
        }

        rightMenu.SetActive(true);
        ManageEvents();
    }

    public void HideInfo()
    {
        infoSet = false;
        rightMenu.SetActive(false);
        focusedSkill = null;
    }

    public void SetInfo(BaseSkill skill)
    {
        focusedSkill = skill;
        skillImage.sprite = skill.icon;
        skillImage.preserveAspect = true;
        nameText.text = $"{skill.Name} lv{skill.CurrentLevelReal}";
        descriptionText.text = skill.Description;
        currentXpText.text = skill.XpIntoLevel().ToString();
        requiredXpText.text = skill.ExperienceRequiredForNextLevel.ToString();
        xpBar.fillAmount = skill.PercentageIntoLevel();
        infoSet = true;
    }

    public void RefreshInfo() // Refresh when the skill is changed so that it's up to date.
    {
        SetInfo(focusedSkill);
    }

    public void OpenSkillMenu()
    {
        skillMenu.SetActive(true);
        open = true;
        HideInfo();
    }

    public void ToggleSkillMenu()
    {
        open = !open;
        if (open) OpenSkillMenu(); // I could use "SetActive(open)" but I need to run the contents of the "OpenSkillMenu" method.
        else CloseSkillMenu();
    }

    public void CloseSkillMenu()
    {
        skillMenu.SetActive(false);
        open = false;
    }
}