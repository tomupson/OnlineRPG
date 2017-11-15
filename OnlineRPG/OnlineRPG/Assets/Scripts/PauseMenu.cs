using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    #region Singleton
    public static PauseMenu singleton;
    #endregion

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu; 

    [HideInInspector] public bool open;

    void Awake()
    {
        if (singleton != null)
        {
            Debug.LogError("More than one pause menu on the client");
            return;
        }

        singleton = this;
    }

    public void OpenPauseMenu()
    {
        open = true;
        pauseMenu.SetActive(true);
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
    }

    public void ResumeGame()
    {
        ClosePauseMenu();
    }

    public void Options()
    {
        optionsMenu.SetActive(true);
    }

    public void CloseOptions()
    {
        // Chck for unapplied changes.
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