using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestBook : MonoBehaviour
{
    #region Singleton
    public static QuestBook singleton;
    #endregion

    CharacterStats stats;

    [SerializeField] private GameObject questBook;
    [SerializeField] private GameObject questPrefab;
    [SerializeField] private GameObject goalPrefab;
    [SerializeField] private Transform contentTransform;
    [SerializeField] private Transform goalsContentTransform;
    [SerializeField] private GameObject rightMenu;

    [Space]

    [Header("Info")]
    [SerializeField] private Image questImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;

    [SerializeField] private GameObject noQuestsText;

    Quest focusedQuest;

    [HideInInspector] public bool open = false;
    bool infoSet = false;

    void Awake()
    {
        if (singleton != null)
        {
            Debug.LogError("Already a Quest Book on the client!");
            return;
        }

        singleton = this;
    }

    void Start()
    {
        stats = Player.LocalPlayer.GetComponent<CharacterStats>();
        stats.OnQuestAdded += CreateQuestFor;
        CloseQuestBook();
        infoSet = false;
    }
    
    void ManageEvents()
    {
        if (focusedQuest == null) return;

        focusedQuest.OnQuestChanged += RefreshInfo;

        foreach (Quest quest in stats.quests)
        {
            if (quest != focusedQuest)
            {
                quest.OnQuestChanged -= RefreshInfo;
            }
        }
    }

    public void CreateQuestFor(Quest quest)
    {
        GameObject questGO = Instantiate(questPrefab, contentTransform, false);
        QuestUI questUI = questGO.GetComponent<QuestUI>();
        questUI.Setup(quest);

        if (noQuestsText.activeSelf) noQuestsText.SetActive(false);
    }

    public void ShowQuestBook()
    {
        questBook.SetActive(true);
        open = true;
        HideInfo();
        if (stats.quests.Count == 0)
            noQuestsText.SetActive(true);
        else
            noQuestsText.SetActive(false);
    }

    public void ToggleQuestBook()
    {
        open = !open;
        if (open) ShowQuestBook(); // I could use "SetActive(open)" but I need to run the contents of the "OpenQuestBook" method.
        else CloseQuestBook();
    }

    public void CloseQuestBook()
    {
        questBook.SetActive(false);
        open = false;
    }

    public void SetInfo(Quest quest)
    {
        ClearChildren(goalsContentTransform);

        this.focusedQuest = quest;
        questImage.sprite = quest.Icon;
        nameText.text = quest.Name;
        descriptionText.text = quest.Description;

        foreach (Goal goal in quest.Goals)
        {
            GameObject goalGO = Instantiate(goalPrefab, goalsContentTransform, false);
            GoalUI goalUI = goalGO.GetComponent<GoalUI>();
            goalUI.Setup(goal);
        }

        rightMenu.SetActive(true);
        infoSet = true;
    }

    public void ShowInfo()
    {
        if (!infoSet)
        {
            Debug.LogError("You must set the info before showing it!");
            return;
        }

        rightMenu.SetActive(true);
        ManageEvents();
    }

    public void RefreshInfo()
    {
        SetInfo(focusedQuest);
    }

    public void HideInfo()
    {
        rightMenu.SetActive(false);
        infoSet = false;
    }

    void ClearChildren(Transform transform)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}