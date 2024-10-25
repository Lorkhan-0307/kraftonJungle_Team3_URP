using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PhotonView))]
public class ChatManager : MonoBehaviourPun
{
    [SerializeField] Transform chatList;
    [SerializeField] GameObject chatPrefab;

    [SerializeField] TMP_InputField chatInput;


    public void SendChat()
    {
        string text = chatInput.text.Trim();

        if (text == "") return;

        //TODO: 인자로 UserID 대신 PlayerName 넣어줘야함.
        photonView.RPC("SendChatToClients", RpcTarget.AllBuffered,
            PhotonNetwork.LocalPlayer.UserId, text); // 모든 클라이언트에 동기화

        chatInput.text = "";
    }

    [PunRPC]
    public void SendChatToClients(string playerName, string text)
    {
        SetEachChat(playerName, text);
    }

    private void SetEachChat(string playerName, string text)
    {
        MyRoomChat p = Instantiate(chatPrefab, chatList).GetComponent<MyRoomChat>();
        p.SetupChat(new ChatElement(playerName, text));
    }
}
