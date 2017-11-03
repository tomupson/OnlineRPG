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
    [SerializeField] private Transform contentTransform;
    [SerializeField] private GameObject rightMenu;

    [Space]

    [Header("Info")]
    [SerializeField] private Image questImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI currentProgress;
    [SerializeField] private TextMeshProUGUI requiredProgress;
    [SerializeField] private Image questProgessBar;

    Quest focusedQuest;

    bool infoSet = false;
    public bool open = false;

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
        stats = FindObjectOfType<Player>().GetComponent<CharacterStats>();
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
        Debug.Log(questGO);
        QuestUI questUI = questGO.GetComponent<QuestUI>();
        questUI.Setup(quest);
    }

    public void ShowQuestBook()
    {
        questBook.SetActive(true);
        open = true;
        HideInfo();
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
        this.focusedQuest = quest;
        questImage.sprite = quest.Icon;
        nameText.text = quest.Name;
        descriptionText.text = quest.Description;
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
}