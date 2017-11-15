using TMPro;
using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.Chat;

public class MenuChat : MonoBehaviour
{
    [SerializeField] private TMP_Text loadingChatText;
    [SerializeField] private TMP_Text chatStatusText;
    [SerializeField] private TMP_Text roomNameText;
    [SerializeField] private TMP_InputField chatInputField;

    [SerializeField] private GameObject messagePrefab;
    [SerializeField] private GameObject contentPrefab;
    [SerializeField] private Transform viewport;

    private List<ChannelUI> channels;

    void Start()
    {
        loadingChatText.gameObject.SetActive(true);
        EventHandler.OnChatMessagesReceived += ShowNewMessages;
        EventHandler.OnChatStateChanged += ChatStateChanged;
        EventHandler.OnChatChannelSubscribed += CreateNewChannel;
        EventHandler.OnChatChannelUnsubscribed += RemoveChannel;
        StartCoroutine(WaitForChatInitialization());
    }

    void ChatStateChanged(ChatState state)
    {
        if (chatStatusText != null)
        {
            chatStatusText.text = state.ToString();
        }
    }

    IEnumerator WaitForChatInitialization()
    {
        yield return new WaitUntil(() => ChatHandler.singleton.GetChannels().Count > 0);
        loadingChatText.gameObject.SetActive(false);
    }

    public void ShowNewMessages(string channelName, string[] senders, object[] messages)
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

            GameObject chatMessageGO = Instantiate(messagePrefab, channelUI.transform, false);
            ChatMessage chatMessage = chatMessageGO.GetComponent<ChatMessage>();

            string message = $"({DateTime.Now.ToLongTimeString()}) [{PhotonNetwork.playerList.ToList().Where(x => x.UserId == senders[i]).FirstOrDefault().NickName}]: {messages[i]}";
            chatMessage.SetupMessage(message);
        }
    }

    public void SendChatMessage()
    {
        if (string.IsNullOrEmpty(chatInputField.text)) return;

        // By default we're using global chat just for testing
        string textToSend = chatInputField.text.Length > Constants.MAX_CHAT_MESSAGE_LENGTH ? chatInputField.text.Substring(0, Constants.MAX_CHAT_MESSAGE_LENGTH) : chatInputField.text;
        ChatHandler.singleton.SendText(textToSend, "global");

        chatInputField.Clear();
        chatInputField.DeactivateInputField();
    }

    void CreateNewChannel(ChatChannel channel)
    {
        GameObject newContent = Instantiate(contentPrefab, viewport, false);
        ChannelUI channelUI = newContent.GetComponent<ChannelUI>();
        channelUI.Setup(channel);
    }

    void RemoveChannel(ChatChannel channel)
    {
        ChannelUI channelUI = channels.Where(x => x.chatChannel == channel).FirstOrDefault();
        Destroy(channelUI.gameObject);
    }
}