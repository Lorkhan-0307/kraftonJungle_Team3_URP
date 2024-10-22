using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MyRoomManager : MonoBehaviour
{
    [SerializeField] private GameObject haveServer;
    [SerializeField] private GameObject noServer;

    [SerializeField] private Transform playerList;
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject readyButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 사용자가 방을 만들거나 참여하면 꼭 이 함수를 실행시켜주세요.
    public void OnRoomCreateOrJoin(bool isOwner)
    {
        haveServer.SetActive(true);
        noServer.SetActive(false);
        
        
        
        if(isOwner) SetOwner();
        else SetGuest();
        
        // update player List 꼭 참여할때 진행하게 해주세요!
        UpdatePlayerList();
    }

    public void UpdatePlayerList()
    {
        // 여기에서 player List를 업데이트합니다. 유저가 방에 참여할때, 방에서 나올때 이 함수를 실행시켜주세요.
        // 방에서 나가는 유저는 이 함수를 실행시키면 안됩니다.
        // 마찬가지로, 받아온 player 값들을 foreach로 하여, 아래의 함수를 실행해주세요.
    }

    private void SetEachPlayer(PlayerOnLobbyElement pole)
    {
        Instantiate(playerPrefab, playerList).GetComponent<PlayerOnLobby>().SetupPlayerOnLobby(pole);
    }

    // 사용자가 방에서 나오거나 방이 파괴되면 이 함수를 실행시켜주세요.
    public void OnRoomLeave()
    {
        haveServer.SetActive(false);
        noServer.SetActive(true);
    }

    // 방 주인이면 Game Start Button을,

    // 방장을 이어받게 된 경우 or Create Room을 한 경우
    private void SetOwner()
    {
        StartButtonActivate();
    }

    private void SetGuest()
    {
        ReadyButtonActivate();
    }
    
    // Game Start Button Activate
    private void StartButtonActivate()
    {
        startButton.SetActive(true);
        readyButton.SetActive(false);
    }
    
    // Join을 하는 경우
    private void ReadyButtonActivate()
    {
        startButton.SetActive(false);
        readyButton.SetActive(true);
    }

    // Guest가 Ready 버튼을 누른 경우(Todo)
    public void OnClickReady()
    {
        
    }

    // Room Owner가 StartGame 버튼을 누른 경우(Todo)
    public void OnClickStartGame()
    {
        
    }
}
