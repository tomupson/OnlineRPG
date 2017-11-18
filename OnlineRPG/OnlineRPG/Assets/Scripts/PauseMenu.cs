using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    #region Singleton
    public static PauseMenu singleton;
    #endregion

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu; 

    [HideInInspector] public bool open;

    private GameMaster gameMaster;

    void Awake()
    {
        if (singleton != null)
        {
            Debug.LogError("More than one pause menu on the client");
            return;
        }

        singleton = this;
    }

    void Start()
    {
        gameMaster = FindObjectOfType<GameMaster>();
        gameMaster.QueueEscape(OpenPauseMenu);
    }

    public void OpenPauseMenu()
    {
        open = true;
        pauseMenu.SetActive(true);
        gameMaster.QueueEscape(ClosePauseMenu);
    }

    public void TogglePauseMenu()
    {
        open = !open;
        if (open) OpenPauseMenu();
        else ClosePauseMenu();
    }

    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
        open = false;
        gameMaster.QueueEscape(OpenPauseMenu);
    }

    public void ResumeGame()
    {
        ClosePauseMenu();
    }

    public void Options()
    {
        optionsMenu.SetActive(true);
        gameMaster.QueueEscape(CloseOptions);
    }

    public void CloseOptions()
    {
        if (gameMaster.IsTopOfQueue(CloseOptions)) gameMaster.TryPopFromEscapeQueue(CloseOptions);
        // Check for unapplied changes.
        optionsMenu.SetActive(false);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Menu");
    }
}