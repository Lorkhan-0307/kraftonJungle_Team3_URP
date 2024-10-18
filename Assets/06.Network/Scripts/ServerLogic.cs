using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using ExitGames.Client.Photon; // �̺�Ʈ �ڵ忡 ���

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


    // Update is called once per frame
    void Update()
    {
        ApplyTime();
    }


    #region GameStart
    public void SetPlayerRole()
    {
        // ���� �뿡 ���ӵǾ��ִ� �÷��̾� ��� �޾ƿ�
        Player[] playerList = PhotonNetwork.PlayerList;

        Debug.Log("�÷��̾� ��: " + playerList.Length.ToString());
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
}
