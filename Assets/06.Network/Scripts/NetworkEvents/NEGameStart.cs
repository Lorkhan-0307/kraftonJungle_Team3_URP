using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEGameStart : NetworkEvent
{
    protected override void Awake()
    {
        this.eventCode = EventCode.GameStart;
        base.Awake();
    }
    public override void OnEvent(object customData)
    {
        StartGame(customData);
    }

    #region StartGame
    [SerializeField]
    string playerScientistName;
    [SerializeField]
    string playerMonsterName;
    [SerializeField]
    GameObject playerObjectPrefab;
    public void StartGame(object data)
    {
        object[] datas = (object[])data;

        Vector3[] spawnPos = (Vector3[])datas[0];
        int monsterNum = (int)datas[1];
        NetworkManager.Instance.NPCCount = (int)datas[2];

        //TODO: 자신의 플레이어 ActorNumber 가 전송받은 id와 같은지 비교하고 몬스터, 연구원으로 초기화함.
        Debug.Log($"Monster : {monsterNum}");

        //Debugging
        string deb = "";
        foreach (Vector3 v in spawnPos)
        {
            deb += $"{v.ToString()}, ";
        }
        Debug.Log($"current Actornum:{PhotonNetwork.LocalPlayer.ActorNumber}  {deb}");


        Vector3 myPosition = spawnPos[PhotonNetwork.LocalPlayer.ActorNumber - 1];
        GameObject spawnedPlayer = null;
        if (monsterNum == PhotonNetwork.LocalPlayer.ActorNumber)
            spawnedPlayer = PhotonNetwork.Instantiate(playerMonsterName, myPosition, Quaternion.identity);
        else
            spawnedPlayer = PhotonNetwork.Instantiate(playerScientistName, myPosition, Quaternion.identity);

        NetworkManager.Instance.myPlayer = spawnedPlayer.GetComponent<Player>();

        GameObject po = Instantiate(playerObjectPrefab, spawnedPlayer.transform.position,
            spawnedPlayer.transform.rotation, spawnedPlayer.transform);

        NetworkManager.Instance.curState = GameState.Playing;
        GameManager.instance.StartGame();
    }
    #endregion

}
