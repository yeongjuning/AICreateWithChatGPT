using OpenAI;
using OpenAI.Chat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AIManager : MonoBehaviour
{
    public enum APIType { OpenAI = 0 }

    public static AIManager Instance = null;
    [Space(10f)][Header("---------Authentication---------")]
    [SerializeField] string[] apiKey;
    [SerializeField] string[] organizationId;

    [Space(10f)][Header("---------TalkManager---------")]
    [SerializeField] TalkManager talkManager;
    public OpenAIClient OpenAIAPI { private set; get; }

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    IEnumerator Start()
    {
        var type = APIType.OpenAI;
        OpenAIAPI = new OpenAIClient(new OpenAIAuthentication(apiKey[(int)type], organizationId[(int)type]));

        yield return new WaitUntil(() => OpenAIAPI != null);
        talkManager.Init();
        Debug.Log("Open AI API is Setting");
    }

    // 자연어 처리
    public async void RequestChat(string strAsk, UnityAction<string, bool> callback)
    {
        List<Message> prompts = new List<Message>();
        prompts.Add(new Message(Role.User, strAsk));

        var request = new ChatRequest
        (
            messages: prompts,
            model: OpenAI.Models.Model.GPT4o,
            temperature: 0.2f
        );

        try 
        {
            ChatResponse result = await OpenAIAPI.ChatEndpoint.GetCompletionAsync(request);
            if (result != null)
            {
                Debug.Log($"<color=cyan>Response : {result.FirstChoice.ToString()}</color>");
                callback?.Invoke(result.FirstChoice.ToString(), false);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
            throw;
        }
    }
}
