using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuestUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image questImage;
    [SerializeField] private Image progressBar;
    [SerializeField] private TMP_Text nameText;

    Quest quest;
    QuestBook questBook;

    void Start()
    {
        questBook = QuestBook.singleton;
    }

    public void Setup(Quest quest)
    {
        this.quest = quest;
        questImage.sprite = quest.Icon;
        questImage.preserveAspect = true;
        nameText.text = quest.Name;
        quest.OnQuestChanged += OnQuestChanged;
    }

    public void OnQuestChanged()
    {

    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            questBook.SetInfo(quest);
            questBook.ShowInfo();
        }
    }
}