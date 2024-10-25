using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ChatElement
{
    public string playerName;
    public string text;

    public ChatElement(string playerName, string text)
    {
        this.playerName = playerName;
        this.text = text;
    }
}

public class MyRoomChat : MonoBehaviour
{
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text text;

    public void SetupChat(ChatElement chat)
    {
        playerName.text = chat.playerName;
        text.text = chat.text;
    }
}
