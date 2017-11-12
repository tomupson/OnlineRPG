using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [SerializeField] private Texture2D defaultCursor;

    private InputManager inputMan;

	void Start()
	{
        inputMan = InputManager.singleton;
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

        if (Input.GetKeyDown(inputMan.GetKey("OPEN_INVENTORY").Key))
        {
            Inventory.singleton.ToggleInventory();
        }

        if (Input.GetKeyDown(inputMan.GetKey("OPEN_SKILLS").Key))
        {
            SkillManager.singleton.ToggleSkillMenu();
        }

        if (Input.GetKeyDown(inputMan.GetKey("OPEN_QUESTS").Key))
        {
            QuestBook.singleton.ToggleQuestBook();
        }

        if (Input.GetKeyDown(inputMan.GetKey("OPEN_CHAT").Key))
        {
            Chat.singleton.ToggleChat();
        }

        if (!QuestBook.singleton.open &&
            !Inventory.singleton.open && !SkillManager.singleton.open &&
            Input.GetKeyDown(inputMan.GetKey("OPEN_PAUSE_MENU").Key))
        {
            PauseMenu.singleton.TogglePauseMenu();
        }
    }
}