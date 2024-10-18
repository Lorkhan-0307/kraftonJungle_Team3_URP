using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using ExitGames.Client.Photon; // 이벤트 코드에 사용

public enum EventCode
{
    GameStart = 0,
    AttackRequest,
    PlayerDeath,
    EndGame,
    SwitchDayNight
}

public class ServerLogic : MonoBehaviourPunCallbacks
{

    // 싱글톤 인스턴스
    public static ServerLogic Instance { get; private set; }

    // Awake는 Start보다 먼저 호출됩니다.
    private void Awake()
    {
        // 싱글톤 인스턴스가 없는 경우 이 객체를 인스턴스로 설정
        if (Instance == null)
        {
            Instance = this;
            // 다른 씬에서도 파괴되지 않도록 설정
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 이미 인스턴스가 존재하는 경우, 중복된 객체를 파괴
            Destroy(gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        ApplyTime();
    }


    #region GameStart
    public void SetPlayerRole()
    {
        // 현재 룸에 접속되어있는 플레이어 목록 받아옴
        Player[] playerList = PhotonNetwork.PlayerList;

        Debug.Log("플레이어 수: " + playerList.Length.ToString());
        int result = Random.Range(0, playerList.Length)+1;

        NetworkManager.Instance.SendToClients(EventCode.GameStart, result);
    }
    #endregion

    #region Time
    public float dayLength;
    public float nightLength;
    public float curTime;
    public bool isDay = true;

    public void ApplyTime()
    {
        curTime += Time.deltaTime;

        if (isDay)
        {
            if (curTime > dayLength)
            {
                // 저녁됨 방송함
            }
        }
        else
        {
            if (curTime > nightLength)
            {
                // 아침됨 방송함
            }
        }

        // 현재시각 방송함
    }

    #endregion
}
