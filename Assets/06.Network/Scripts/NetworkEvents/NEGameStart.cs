using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEGameStart : NetworkEvent
{

    [SerializeField]
    string playerScientistName;
    [SerializeField]
    string playerMonsterName;
    [SerializeField]
    GameObject playerObjectPrefab;
    [SerializeField]
    GameObject voicePrefab;


    protected override void Awake()
    {
        this.eventCode = EventCode.GameStart;
        base.Awake();
    }
    public override void OnEvent(object customData)
    {
        object[] datas = (object[])customData;

        Dictionary<int, string> spawnPos = (Dictionary<int, string>)datas[0];
        int[] monsterNums = (int[])datas[1];
        NetworkManager.Instance.NPCCount = (int)datas[2];

        GameSettings settings = NetworkManager.Instance.gameSettings;

        // 게임 낮 밤 시간 설정
        if (TimeManager.instance == null)
        {
            Debug.LogError("There is No TimeManager.");
        }
        else
        {
            // Apply Time Cycle Settings
            TimeManager.instance.dayTime = settings.dayLength;
            TimeManager.instance.nightTime = settings.nightLength;
        }


        // 내가 관전자일 경우
        if (settings.spectatorActorNum == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            //SpectatorManager.instance.StartSpectating();
        }
        else
        {           
            // 로컬 플레이어 캐릭터 스폰
            Vector3 myPosition = GameObject.Find(spawnPos[PhotonNetwork.LocalPlayer.ActorNumber]).transform.position;
            GameObject spawnedPlayer = null;

            // 몬스터 스폰
            if (Array.Exists(monsterNums, x => x == PhotonNetwork.LocalPlayer.ActorNumber))
                spawnedPlayer = PhotonNetwork.Instantiate(playerMonsterName, myPosition, Quaternion.identity);
            // 연구원 스폰
            else
                spawnedPlayer = PhotonNetwork.Instantiate(playerScientistName, myPosition, Quaternion.identity);


            NetworkManager.Instance.myPlayer = spawnedPlayer.GetComponent<Player>();

            GameObject po = Instantiate(playerObjectPrefab, spawnedPlayer.transform.position,
                spawnedPlayer.transform.rotation, spawnedPlayer.transform);

        }
        //GameObject voice = Instantiate(voicePrefab, Vector3.zero, Quaternion.identity);

        NetworkManager.Instance.curState = GameState.Playing;

        // 모든 이벤트 초기화 하면 좋을거같은데 지금 당장은 초기화 할게 이거밖에 없다
        GetComponent<NEAttackRequest>().Init();

        GameManager.instance.StartGame();

        GetComponent<NELoadScene>().isLoadingEnded = true;
        Debug.Log("Loading Ended!");
    }
}
