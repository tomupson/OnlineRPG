using TMPro;
using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon.Chat;

public class MenuChat : MonoBehaviour, IChatUI
{
    [SerializeField] private TMP_Text loadingChatText;
    [SerializeField] private TMP_Text chatStatusText;
    [SerializeField] private TMP_InputField chatInputField;

    [SerializeField] private GameObject messagePrefab;
    [SerializeField] private Transform content;

    void Start()
    {
        loadingChatText.gameObject.SetActive(true);
        EventHandler.OnChatMessagesReceived += ShowNewMessages;
        EventHandler.OnChatStateChanged += ChatStateChanged;
        StartCoroutine(WaitForChatInitialization());
    }

    void ChatStateChanged(ChatState state)
    {
        chatStatusText.text = state.ToString();
    }

    IEnumerator WaitForChatInitialization()
    {
        yield return new WaitUntil(() => ChatHandler.singleton.GetChannels().Count > 0);
        loadingChatText.gameObject.SetActive(false);
    }

    public void ShowNewMessages(string channelName, string[] senders, object[] messages)
    {
        if (channelName == "global")
        {
            for (int i = 0; i < senders.Length; i++)
            {
                GameObject chatMessageGO = Instantiate(messagePrefab, content, false);
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

    public void SendChatMessage()
    {
        if (string.IsNullOrEmpty(chatInputField.text)) return;

        // By default we're using global chat just for testing
        string textToSend = chatInputField.text.Length > Constants.MAX_CHAT_MESSAGE_LENGTH ? chatInputField.text.Substring(0, Constants.MAX_CHAT_MESSAGE_LENGTH) : chatInputField.text;
        ChatHandler.singleton.SendText(textToSend, "global");

        chatInputField.Clear();
        chatInputField.DeactivateInputField();
    }
}