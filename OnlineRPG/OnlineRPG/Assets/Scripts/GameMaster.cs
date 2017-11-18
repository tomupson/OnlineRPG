using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class GameMaster : MonoBehaviour
{
    [SerializeField] private Texture2D defaultCursor;

    private InputManager inputMan;

    public delegate void EscapeMenuCallbackDelegate();
    private Stack<EscapeMenuCallbackDelegate> escapeQueue = new Stack<EscapeMenuCallbackDelegate>();

    void Start()
    {
        inputMan = InputManager.singleton;
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    void Update()
    {
        BindMenuInputs();
    }

    public void QueueEscape(EscapeMenuCallbackDelegate callback)
    {
        escapeQueue.Push(callback);
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

        if (Input.GetKeyDown(KeyCode.Escape) &&
            escapeQueue.Count > 0)
        {
            escapeQueue.Pop().Invoke();
        }
    }

    public bool IsTopOfQueue(EscapeMenuCallbackDelegate callback)
    {
        return escapeQueue.First() == callback;
    }

    public void TryPopFromEscapeQueue(EscapeMenuCallbackDelegate callback)
    {
        if (!IsTopOfQueue(callback)) return;

        escapeQueue.Pop();
    }
}