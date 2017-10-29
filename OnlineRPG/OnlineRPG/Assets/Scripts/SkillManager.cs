using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    [SerializeField] private GameObject skillPrefab;
    [SerializeField] private Transform contentTransform;
    [SerializeField] private GameObject skillXpGained;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one SkillManager in the scene!");
            return;
        }

        instance = this;
    }

    void Start()
    {
        skillXpGained.SetActive(false);
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
        StartCoroutine(ShowXPGranted(skill, xp));
    }

    IEnumerator ShowXPGranted(BaseSkill skill, float xp)
    {
        skillXpGained.SetActive(true);
        Image img = skillXpGained.GetComponentInChildren<Image>();
        TextMeshProUGUI txt = skillXpGained.GetComponentInChildren<TextMeshProUGUI>();
        txt.text = string.Format("+{0}XP", xp);
        img.sprite = skill.icon;
        Animator animator = skillXpGained.GetComponent<Animator>();

        animator.SetBool("IsShowing", true);

        yield return new WaitForSeconds(3f);

        animator.SetBool("IsShowing", false);

        yield return null;
    }
}