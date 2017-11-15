using TMPro;
using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.Chat;
using System.Text.RegularExpressions;

public class Chat : MonoBehaviour
{
    #region Singleton
    public static Chat singleton;
    #endregion

    [Header("Togglable Settings")]
    [SerializeField, Tooltip("Optional -- Required only if togglable")] private GameObject chat;
    [SerializeField] private bool canToggle = true;

    [Space]

    [Header("Bottom Bar")]
    [SerializeField] private TMP_Text chatRoomNameText;
    [SerializeField] private TMP_InputField chatInputField;


    [Space]

    [Header("Channels")]
    [SerializeField] private GameObject contentPrefab;
    [SerializeField] private Transform viewport;

    [Space]

    [Header("Messages")]
    [SerializeField] private GameObject chatMessagePrefab;

    [Space]

    [Header("Other")]
    [SerializeField] private GameObject loadingChatPanel;
    [SerializeField] private TMP_Text chatStatusText;

    [HideInInspector] public bool open = false;

    ChatChannel selectedChannel;

    private List<ChannelUI> channels = new List<ChannelUI>();
    private List<ChatMessage> messages = new List<ChatMessage>();

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
        loadingChatPanel.SetActive(true);
        EventHandler.OnChatMessagesReceived += ShowNewMessages;
        EventHandler.OnChatMessagesRemoved += RemoveMessages;
        EventHandler.OnChatStateChanged += ChatStateChanged;
        EventHandler.OnChatChannelSubscribed += CreateNewChannel;
        EventHandler.OnChatChannelUnsubscribed += RemoveChannel;
        StartCoroutine(WaitForChatInitialization());
    }

    void ChatStateChanged(ChatState state)
    {
        if (chatStatusText != null)
        {
            string[] split = Regex.Split(state.ToString(), @"(?<!^)(?=[A-Z])");
            chatStatusText.text = string.Join(" ", split);
        }
    }

    IEnumerator WaitForChatInitialization()
    {
        yield return new WaitUntil(() => ChatHandler.singleton.GetChannels().Count > 0);
        loadingChatPanel.SetActive(false);
        selectedChannel = ChatHandler.singleton.GetChannelByName("global");
        chatRoomNameText.text = ChatHelper.GetHumanReadableChannelName(selectedChannel.Name);

        List<ChatChannel> chatChannels = ChatHandler.singleton.GetChannels();

        for (int i = 0; i < chatChannels.Count; i++)
        {
            CreateNewChannel(chatChannels[i]);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && chatInputField.isFocused)
        {
            MoveNextChannel();
        }
    }

    public void SendChatMessage()
    {
        if (string.IsNullOrEmpty(chatInputField.text)) return;

        // By default we're using global chat just for testing
        string textToSend = chatInputField.text.Length > Constants.MAX_CHAT_MESSAGE_LENGTH ? chatInputField.text.Substring(0, Constants.MAX_CHAT_MESSAGE_LENGTH) : chatInputField.text;
        ChatHandler.singleton.SendText(textToSend, selectedChannel.Name);

        chatInputField.Clear();
        chatInputField.DeactivateInputField();
    }

    public void ShowNewMessages(string channelName, string[] senders, object[] newMessages)
    {
        for (int i = 0; i < senders.Length; i++)
        {
            ChatChannel channel = ChatHandler.singleton.GetChannelByName(channelName);
            ChannelUI channelUI = channels.Where(x => x.chatChannel == channel).FirstOrDefault();
            if (channelUI == null)
            {
                GameObject newContent = Instantiate(contentPrefab, viewport, false);
                channelUI = newContent.GetComponent<ChannelUI>();
                channelUI.Setup(channel);
            }

            GameObject chatMessageGO = Instantiate(chatMessagePrefab, channelUI.transform, false);
            ChatMessage chatMessage = chatMessageGO.GetComponent<ChatMessage>();
            messages.Add(chatMessage);
            
            string message = $"({DateTime.Now.ToLongTimeString()}) [{senders[i]}]: {newMessages[i]}";
            chatMessage.SetupMessage(message);
        }
    }

    void RemoveMessages(ChatMessage[] messagesToRemove)
    {
        for (int i = 0; i < messagesToRemove.Length; i++)
        {
            ChatMessage message = messages.Where(x => x == messagesToRemove[i]).FirstOrDefault();
            Destroy(message.gameObject);
            messages.Remove(message);
        }
    }

    public void MoveNextChannel()
    {
        List<ChatChannel> chatChannels = ChatHandler.singleton.GetChannels();

        if (chatChannels.Count <= 1) return;

        int currentIndex = chatChannels.IndexOf(selectedChannel);
        int newIndex = currentIndex == chatChannels.Count - 1 ? 0 : currentIndex + 1;
        selectedChannel = chatChannels[newIndex];

        foreach (ChannelUI cui in channels)
        {
            cui.gameObject.SetActive(false);
        }

        ChannelUI channelUI = channels.Where(x => x.chatChannel == selectedChannel).FirstOrDefault();
        channelUI.gameObject.SetActive(true);

        chatRoomNameText.text = ChatHelper.GetHumanReadableChannelName(selectedChannel.Name);
    }

    public void ShowChat()
    {
        open = true;
        chat.SetActive(true);
    }

    public void ToggleChat()
    {
        if (!canToggle) return;

        open = !open;
        if (open) ShowChat();
        else CloseChat();
    }

    public void CloseChat()
    {
        if (chatInputField.isFocused) return;
        chat.SetActive(false);
        open = false;
    }

    void CreateNewChannel(ChatChannel channel)
    {
        GameObject newContent = Instantiate(contentPrefab, viewport, false);
        ChannelUI channelUI = newContent.GetComponent<ChannelUI>();
        channels.Add(channelUI);
        channelUI.Setup(channel);
    }

    void RemoveChannel(ChatChannel channel)
    {
        ChannelUI channelUI = channels.Where(x => x.chatChannel == channel).FirstOrDefault();
        Destroy(channelUI.gameObject);
    }

    public bool IsChatInputFocused()
    {
        return chatInputField.isFocused;
    }
}