using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.Dark;
using Photon.Realtime;

public class MyRoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject haveServer;
    [SerializeField] private GameObject noServer;

    [SerializeField] private Transform playerList;
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject readyButton;

    [SerializeField] MainPanelManager panelManager;

    [SerializeField]
    List<PlayerOnRoom> playerContents = new List<PlayerOnRoom>();

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

    public override void OnJoinedRoom()
    {
        Debug.Log($"[{PhotonNetwork.CurrentRoom.Name}] 방에 입장하였습니다.");

        panelManager.OpenPanel("MyRoom");

        //        UpdatePlayerList();
        SetEachPlayer(PhotonNetwork.LocalPlayer);
    }
    public override void OnLeftRoom()
    {
        OnRoomLeave();
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"{newPlayer.UserId} 님이 접속하였습니다.");
        SetEachPlayer(newPlayer);
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log($"{otherPlayer.UserId} 님이 접속을 해제하였습니다.");
        RemovePlayer(otherPlayer);
    }

    public void UpdatePlayerList()
    {
        // 여기에서 player List를 업데이트합니다. 유저가 방에 참여할때, 방에서 나올때 이 함수를 실행시켜주세요.
        // 방에서 나가는 유저는 이 함수를 실행시키면 안됩니다.
        // 마찬가지로, 받아온 player 값들을 foreach로 하여, 아래의 함수를 실행해주세요.

        PlayerOnRoom[] buttons = playerList.GetComponentsInChildren<PlayerOnRoom>();
        for (int i = 0; i < buttons.Length; i++)
        {
            Destroy(buttons[i].gameObject);
        }

        // 현재 방에 접속된 플레이어들 불러와서 UI에 적용
        if(PhotonNetwork.CurrentRoom == null)
        {
            Debug.Log("Not in a room");
            return;
        }
        Dictionary<int, Photon.Realtime.Player> players = PhotonNetwork.CurrentRoom.Players;
        foreach (int key in players.Keys)
        {
            Photon.Realtime.Player player = players[key];

            if (player != null)
            {
                Debug.Log("Unexpected Player!");
                continue;
            }

            SetEachPlayer(player);
        }
    }

    private void SetEachPlayer(Photon.Realtime.Player player)
    {
        PlayerOnRoom p = Instantiate(playerPrefab, playerList).GetComponent<PlayerOnRoom>();
        p.SetupPlayerOnRoom(new PlayerOnRoomElement(player));
        p.player = player;
        playerContents.Add(p);
    }
    private void RemovePlayer(Photon.Realtime.Player player)
    {
        PlayerOnRoom p = playerContents.Find(x => x.player == player);

        if (p == null)
        {
            Debug.Log("No Such Player");
        }

        playerContents.Remove(p);
        Destroy(p.gameObject);
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

    // Guest가 Ready 버튼을 누른 경우
    public void OnClickReady()
    {
        ExitGames.Client.Photon.Hashtable data = PhotonNetwork.LocalPlayer.CustomProperties;

        if (data.ContainsKey("IsReady"))
        {
            data["Ping"] = true;
            Debug.Log($"{PhotonNetwork.LocalPlayer.UserId} Pressed Ready");
            PhotonNetwork.CurrentRoom.SetCustomProperties(data);
        }
    }

    // Room Owner가 StartGame 버튼을 누른 경우
    public void OnClickStartGame()
    {
        Dictionary<int, Photon.Realtime.Player> players = PhotonNetwork.CurrentRoom.Players;
        foreach (int key in players.Keys)
        {
            Photon.Realtime.Player player = players[key];
            if (!(bool)player.CustomProperties["IsReady"])
            {
                //아직 준비 안한 사람 있음
                Debug.Log("Not All Players Are Ready");
                return;
            }
        }

        // TODO: 준비 완료. 게임 시작!
    }
}
