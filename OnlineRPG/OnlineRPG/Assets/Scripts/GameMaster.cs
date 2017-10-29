using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [SerializeField] private Texture2D defaultCursor;

	void Start()
	{
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
	}
}