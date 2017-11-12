using UnityEngine;

public class Keybinder : MonoBehaviour
{
    [SerializeField] private GameObject keybindPrefab;
    [SerializeField] private Transform content;

    private InputManager inputMan;

    void Start()
    {
        inputMan = InputManager.singleton;

        foreach (string keybind in inputMan.GetAllKeyTypes())
        {
            GameObject keybindGO = Instantiate(keybindPrefab, content, false);
            keybindGO.GetComponent<Keybind>().Setup(keybind);
        }
    }
}