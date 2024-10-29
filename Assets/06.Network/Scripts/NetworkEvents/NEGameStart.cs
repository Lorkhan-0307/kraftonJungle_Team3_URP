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
    string voicePrefabName = "VoiceManager";

    protected override void Awake()
    {
        this.eventCode = EventCode.GameStart;
        base.Awake();
    }
    public override void OnEvent(object customData)
    {
        object[] datas = (object[])customData;

        Vector3[] spawnPos = (Vector3[])datas[0];
        int[] monsterNums = (int[])datas[1];
        NetworkManager.Instance.NPCCount = (int)datas[2];


        // 게임 낮 밤 시간 설정
        if (TimeManager.instance == null)
        {
            Debug.LogError("There is No TimeManager.");
        }
        else
        {
            // Apply Time Cycle Settings
            TimeManager.instance.dayTime = NetworkManager.Instance.gameSettings.dayLength;
            TimeManager.instance.nightTime = NetworkManager.Instance.gameSettings.nightLength;
        }

        //자신의 플레이어 ActorNumber 가 전송받은 id와 같은지 비교하고 몬스터, 연구원으로 초기화함.
        Debug.Log($"Monster : {monsterNums.ToString()}");

        //Debugging
        string deb = "";
        foreach (Vector3 v in spawnPos)
        {
            deb += $"{v.ToString()}, ";
        }
        Debug.Log($"current Actornum:{PhotonNetwork.LocalPlayer.ActorNumber}  {deb}");

        // 로컬 플레이어 캐릭터 스폰
        Vector3 myPosition = spawnPos[PhotonNetwork.LocalPlayer.ActorNumber - 1];
        GameObject spawnedPlayer = null;

        // 몬스터 스폰
        if(Array.Exists(monsterNums, x=> x == PhotonNetwork.LocalPlayer.ActorNumber))
            spawnedPlayer = PhotonNetwork.Instantiate(playerMonsterName, myPosition, Quaternion.identity);
        // 연구원 스폰
        else
            spawnedPlayer = PhotonNetwork.Instantiate(playerScientistName, myPosition, Quaternion.identity);

        GameObject voice = PhotonNetwork.Instantiate(voicePrefabName, spawnedPlayer.transform.position, spawnedPlayer.transform.rotation);
        voice.transform.SetParent(spawnedPlayer.transform, false);
        voice.AddComponent<VoiceManager>();

        NetworkManager.Instance.myPlayer = spawnedPlayer.GetComponent<Player>();

        GameObject po = Instantiate(playerObjectPrefab, spawnedPlayer.transform.position,
            spawnedPlayer.transform.rotation, spawnedPlayer.transform);

        NetworkManager.Instance.curState = GameState.Playing;
        GameManager.instance.StartGame();
    }
}
