using Photon.Pun;
using UnityEngine;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine.InputSystem;
using System.Linq;
using System;

public enum EventCode
{
    GameStart = 0,
    AttackRequest,
    PlayerDeath,
    EndGame,
    SwitchDayNight,
    HungerGauge,
    AccelTime,
    OnDisconnected,
}

public enum GameState
{
    OnRoom,
    Playing,
    End,
}

public class ServerLogic : MonoBehaviourPunCallbacks
{
    private InputAction gameStartAction;


    private void Start()
    {
        gameStartAction = new InputAction(type: InputActionType.Value, binding: "<Keyboard>/space");
        gameStartAction.Enable();
        gameStartAction.performed += StartGameCallback;
    }

    public void StartGameCallback(InputAction.CallbackContext context)
    {
        Debug.Log("StartGame Button Clicked");
        if (NetworkManager.Instance.curState != GameState.OnRoom) return;


        // TODO: 게임 시작 기능 구현
        StartGameWithSettings(NetworkManager.Instance.gameSettings);
    }



    #region GameLogic
    public int[] monsterActorNums;

    ExitGames.Client.Photon.Hashtable isAlivePlayers = new Hashtable();


    public void PlayerDeath(int actorNum)
    {
        isAlivePlayers[actorNum] = false;

        CheckEndCondition();
    }
    #endregion

    #region GameStart
    public void StartGameWithSettings(GameSettings settings)
    {
        if (settings == null)
        {
            Debug.Log("There is no \"GameSettingsData\" in Resources folder.");
            settings = new GameSettings();
        }
        // 현재 룸에 접속되어있는 플레이어 목록을 가져옴
        Photon.Realtime.Player[] playerList = PhotonNetwork.PlayerList;

        // 인원 체크
        if (playerList.Length < settings.players && settings.players > 0)
        {
            Debug.Log($"[{playerList.Length}/{settings.players}] 인원이 부족합니다.");
            return;
        }



        // 플레이어 생존 여부 배열 초기화
        isAlivePlayers = new Hashtable();

        for (int i = 0; i < playerList.Length; i++)
        {
            isAlivePlayers.Add(playerList[i].ActorNumber, true);
        }


        // 랜덤으로 몬스터 번호 할당
        { 
            if (settings.monsterRandomSelect)
            {
                UnityEngine.Random.InitState((int)Time.time);
                monsterActorNums = SelectMonsters(settings.monsters);
            }
            else
            {
                monsterActorNums = settings.monsterActorNums;
            }
        }


        // 각 플레이어에게 랜덤 스폰 위치와 몬스터 번호를 전송
        Vector3[] randomSpawnPos = new Vector3[playerList.Length];
        for (int i = 0; i < randomSpawnPos.Length; i++)
        {
            // 각 플레이어의 랜덤 스폰 위치 설정
            randomSpawnPos[i] = NPCManager.GetRandomNavMeshPosition();

            Debug.Log($"{i}: {randomSpawnPos[i]}");
        }

        // 필드에 랜덤으로 NPC 뿌림
        NPCManager npcManager = FindObjectOfType<NPCManager>();

        // 게임 설정에서 NPCCount값 불러와서 등록
        int npcCount = settings.npcCount;
        npcManager.npcCount = npcCount;
        npcManager.SpawnNPC();


        // 이벤트 데이터에 스폰 위치와 몬스터 번호를 담음
        object[] eventData = new object[] { randomSpawnPos, monsterActorNums, npcCount };

        // 이벤트 전송 (몬스터 번호와 랜덤 스폰 위치)
        NetworkManager.SendToClients(EventCode.GameStart, eventData);
    }

    public int[] SelectMonsters(int numberOfMonsters)
    {
        Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;

        List<int> playerIndices = new List<int>();

        // 플레이어 ActorNumber을 리스트에 추가
        for (int i = 0; i < players.Length; i++)
        {
            playerIndices.Add(players[i].ActorNumber);
        }

        // Fisher-Yates 방식으로 리스트를 섞음
        for (int i = playerIndices.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            int temp = playerIndices[i];
            playerIndices[i] = playerIndices[randomIndex];
            playerIndices[randomIndex] = temp;
        }

        // 원하는 갯수만큼 몬스터를 선택
        int[] selectedMonsters = new int[numberOfMonsters];
        for (int i = 0; i < numberOfMonsters; i++)
        {
            selectedMonsters[i] = playerIndices[i];
        }
        Array.Sort(selectedMonsters);

        return selectedMonsters;
    }
    #endregion

    #region EndGame
    /// <summary>
    /// 게임 종료조건이 달성되었을 때 실행시켜주세요.
    /// Run when the game end condition is in effect.
    /// </summary>
    public void EndGame(bool isMonsterWon)
    {
        string result = "";

        List<int> players = GetAlivePlayers();

        if (players.Count > 0)
            result += players[0].ToString();

        for (int i = 1; i < players.Count; i++)
        {
            result += ",";
            result += players[i].ToString();
        }

        NetworkManager.SendToClients(EventCode.EndGame, new object[] { isMonsterWon, result });
    }

    public List<int> GetAlivePlayers()
    {
        List<int> result = new List<int>();
        foreach(var key in isAlivePlayers.Keys)
        {
            if ((bool)isAlivePlayers[key])
                result.Add((int)key);
        }

        return result;
    }

    public void CheckEndCondition()
    {
        bool isMonsterAlive = false;
        
        // 괴물이 살아있는지 체크
        for(int i = 0; i < monsterActorNums.Length;i++ )
        {
            if ((bool)isAlivePlayers[monsterActorNums[i]])
            {
                isMonsterAlive = true;
                break;
            }
        }

        // 괴물이 모두 죽었을 경우
        if (!isMonsterAlive)
        {
            EndGame(false);
            return;
        }



        // 괴물이 살아있을 경우 모든 플레이어가 죽어야 게임 종료
        foreach (var key in isAlivePlayers.Keys)
        {
            if (!(bool)isAlivePlayers[key]) continue;
            int curPlayer = (int)key;

            if (Array.Exists(monsterActorNums, x => x == curPlayer)) continue;

            return;
        }

        EndGame(true);
    }

    #endregion

    #region Time

    /// <summary>
    /// 서버가 밤낮 바뀐 시점에 실행합니다.
    /// </summary>
    /// <param name="isDay"></param>
    public void SwitchDayNight(bool isDay)
    {
        NetworkManager.SendToClients(EventCode.SwitchDayNight, isDay);
    }
    #endregion
}
