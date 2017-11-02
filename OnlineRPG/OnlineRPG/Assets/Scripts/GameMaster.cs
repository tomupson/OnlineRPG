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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Inventory.instance.CloseInventory();
        }

        if (Input.GetKeyDown(Controls.OPEN_INVENTORY))
        {
            Inventory.instance.ToggleInventory();
        }

        if (Input.GetKeyDown(Controls.OPEN_SKILLS))
        {
            SkillManager.instance.ToggleSkillMenu();
        }
    }
}