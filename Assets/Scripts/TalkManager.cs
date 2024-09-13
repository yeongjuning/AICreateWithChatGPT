using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TalkManager : MonoBehaviour
{
    [Space(10f)][Header("---------UI---------")]
    [SerializeField] Text txtAnswer;
    [SerializeField] InputField inputPrompt;

    [Header("Events")] public static Action onMessageReceived;

    public void Init()
    {
        CreateMessage("Hey There! How can I help you ?", false);
    }

    public void OnClickAskCallback()
    {
        AIManager.Instance.RequestChat(inputPrompt.text, CreateMessage);
        txtAnswer.text = string.Empty;
        inputPrompt.text = string.Empty;
    }

    void CreateMessage(string message, bool isUserMessage)
    {
        Configure(message, isUserMessage);
        onMessageReceived?.Invoke();
    }

    void Configure(string message, bool isUserMessage)
    {
        Debug.Log($"<color=green>Message : {message}</color>");
        txtAnswer.text = message;        
    }
}
