using TMPro;
using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using ExitGames.Client.Photon.Chat;

public class Chat : MonoBehaviour, IChatUI
{
    #region Singleton
    public static Chat singleton;
    #endregion

    [SerializeField] private GameObject chat;
    [SerializeField] private TMP_Text roomNameText;
    [SerializeField] private GameObject chatMessagePrefab;
    [SerializeField] private Transform content;

    public TMP_InputField chatInputField;
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
        //EventHandler.OnChatClientConnected += SetSelectedChannel;
        EventHandler.OnChatMessagesReceived += ShowNewMessages;
        selectedChannel = ChatHandler.singleton.GetChannelByName("global");
        roomNameText.text = selectedChannel.Name;
        ClearChat();
    }

    //void SetSelectedChannel()
    //{
    //    StartCoroutine(WaitForRegisteredChannels());
    //}

    //IEnumerator WaitForRegisteredChannels()
    //{
    //    yield return new WaitUntil(() => ChatHandler.singleton.GetChannels().Count > 0);
    //    selectedChannel = ChatHandler.singleton.GetChannelByName("global");
    //    roomNameText.text = selectedChannel.Name;
    //}

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

    public void ShowNewMessages(string channelName, string[] senders, object[] messages)
    {
        if (selectedChannel.Name == channelName)
        {
            for (int i = 0; i < senders.Length; i++)
            {
                GameObject chatMessageGO = Instantiate(chatMessagePrefab, content, false);
                ChatMessage chatMessage = chatMessageGO.GetComponent<ChatMessage>();
                Debug.Log($"Sender ID: {senders[i]}");
                foreach (PhotonPlayer player in PhotonNetwork.playerList)
                {
                    Debug.Log($"Player {player.NickName}, ID = {player.UserId}");
                }
                string message = $"({DateTime.Now.ToLongTimeString()}) [{PhotonNetwork.playerList.ToList().Where(x => x.UserId == senders[i]).FirstOrDefault().NickName}]: {messages[i]}";
                chatMessage.SetupMessage(message);
            }
        }
    }

    public void MoveNextChannel()
    {
        List<ChatChannel> channels = ChatHandler.singleton.GetChannels();

        if (channels.Count <= 1) return;

        ClearChat();

        int currentIndex = channels.IndexOf(selectedChannel);
        int newIndex = currentIndex == channels.Count - 1 ? 0 : currentIndex + 1;
        selectedChannel = channels[newIndex];
        
        List<ChatSenderMessage> senderWithMessage = ChatHandler.singleton.GetMessagesFromChannel(selectedChannel.Name);
        foreach (ChatSenderMessage chatMessagePair in senderWithMessage)
        {
            GameObject chatMessageGO = Instantiate(chatMessagePrefab, content, false);
            string message = $"({DateTime.Now.ToLongTimeString()}) [{chatMessagePair.sender}]: {chatMessagePair.message.ToString()}";
            chatMessageGO.GetComponent<ChatMessage>().SetupMessage(message, false);
        }

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
        if (chatInputField.isFocused) return;
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