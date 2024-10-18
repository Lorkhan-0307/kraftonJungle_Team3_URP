using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using ExitGames.Client.Photon; // 이벤트 코드에 사용

public enum EventCode
{
    AttackToServer = 0,
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

    // Start is called before the first frame update
    void Start()
    {
    }

    public void SendTest()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            object content = "TEMPTEMP"; // 이벤트에 포함할 데이터 (필요시)
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // 모든 클라이언트에게 전송
            SendOptions sendOptions = new SendOptions { Reliability = true }; // 신뢰성 보장

            //이거 쓰면 될듯
            PhotonNetwork.RaiseEvent(0, content, raiseEventOptions, sendOptions);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ApplyTime();
        SendTest();
    }

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

    #region HitScan
    public float hitRange;
    public void HitScan(Transform user, Transform target)
    {
        if(Vector3.Distance(user.position, target.position) < hitRange)
        {
            // 공격 성공 판정 방송함
        }
    }
    #endregion
}
