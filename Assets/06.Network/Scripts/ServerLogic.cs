using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using ExitGames.Client.Photon; // �̺�Ʈ �ڵ忡 ���

public enum EventCode
{
    AttackToServer = 0,
}

public class ServerLogic : MonoBehaviourPunCallbacks
{
    // �̱��� �ν��Ͻ�
    public static ServerLogic Instance { get; private set; }

    // Awake�� Start���� ���� ȣ��˴ϴ�.
    private void Awake()
    {
        // �̱��� �ν��Ͻ��� ���� ��� �� ��ü�� �ν��Ͻ��� ����
        if (Instance == null)
        {
            Instance = this;
            // �ٸ� �������� �ı����� �ʵ��� ����
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // �̹� �ν��Ͻ��� �����ϴ� ���, �ߺ��� ��ü�� �ı�
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
            object content = "TEMPTEMP"; // �̺�Ʈ�� ������ ������ (�ʿ��)
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // ��� Ŭ���̾�Ʈ���� ����
            SendOptions sendOptions = new SendOptions { Reliability = true }; // �ŷڼ� ����

            //�̰� ���� �ɵ�
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
                // ����� �����
            }
        }
        else
        {
            if (curTime > nightLength)
            {
                // ��ħ�� �����
            }
        }

        // ����ð� �����
    }

    #endregion

    #region HitScan
    public float hitRange;
    public void HitScan(Transform user, Transform target)
    {
        if(Vector3.Distance(user.position, target.position) < hitRange)
        {
            // ���� ���� ���� �����
        }
    }
    #endregion
}
