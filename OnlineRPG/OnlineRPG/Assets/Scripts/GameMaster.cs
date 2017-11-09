using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [SerializeField] private Texture2D defaultCursor;

	void Start()
	{
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
	}

    void Update()
    {
        BindMenuInputs();
    }

    void BindMenuInputs()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Inventory.singleton.CloseInventory();
            SkillManager.singleton.CloseSkillMenu();
            QuestBook.singleton.CloseQuestBook();
        }

        if (Input.GetKeyDown(Controls.OPEN_INVENTORY))
        {
            Inventory.singleton.ToggleInventory();
        }

        if (Input.GetKeyDown(Controls.OPEN_SKILLS))
        {
            SkillManager.singleton.ToggleSkillMenu();
        }

        if (Input.GetKeyDown(Controls.OPEN_QUESTS))
        {
            QuestBook.singleton.ToggleQuestBook();
        }

        if (Input.GetKeyDown(Controls.OPEN_CHAT))
        {
            Chat.singleton.ToggleChat();
        }
    }
}