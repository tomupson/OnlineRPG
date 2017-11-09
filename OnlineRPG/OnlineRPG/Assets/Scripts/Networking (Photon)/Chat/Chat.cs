using TMPro;
using UnityEngine;
using System.Collections.Generic;
using ExitGames.Client.Photon.Chat;

public class Chat : MonoBehaviour
{
    #region Singleton
    public static Chat singleton;
    #endregion

    [SerializeField] private GameObject chat;
    [SerializeField] private TMP_InputField chatInputField;
    [SerializeField] private TextMeshProUGUI roomNameText;
    [SerializeField] private Transform content;

    [HideInInspector] public bool open = false;

    ChatChannel selectedChannel;

    void Awake()
    {
        if (singleton != null)
        {
            Debug.LogError("Multiple Chats on the client!");
            return;
        }

        singleton = this;
    }

    void Start()
    {
        selectedChannel = ChatHandler.singleton.GetChannelByName("global");
        ClearChat();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && chatInputField.isFocused)
        {
            MoveNextChannel();
        }
    }

    public void SendMessage()
    {
        if (string.IsNullOrEmpty(chatInputField.text)) return;

        // By default we're using global chat just for testing
        ChatHandler.singleton.SendText(chatInputField.text, "global");
    }

    public void MoveNextChannel()
    {
        List<ChatChannel> channels = ChatHandler.singleton.GetChannels();

        if (channels.Count <= 1) return;

        int currentIndex = channels.IndexOf(selectedChannel);
        int newIndex = currentIndex == channels.Count - 1 ? 0 : currentIndex + 1;
        selectedChannel = channels[newIndex];
        roomNameText.text = selectedChannel.Name;
    }

    public void ShowChat()
    {
        open = true;
        chat.SetActive(true);
    }

    public void ToggleChat()
    {
        open = !open;
        if (open) ShowChat();
        else CloseChat();
    }

    public void CloseChat()
    {
        chat.SetActive(false);
        open = false;
    }

    void ClearChat()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }
}