// �� ��� �ڵ�.
// ����� ������� �ʽ��ϴ�.
//using Photon.Pun;
//using UnityEngine;
//using UnityEngine.UI;

//public class PingDisplay : MonoBehaviour
//{
//    public Text pingText;  // Ping ������ ǥ���� UI Text

//    void Start()
//    {
//        // PingText ������Ʈ�� UI���� ã���ϴ�.
//        if (pingText == null)
//        {
//            pingText = GameObject.Find("PingText").GetComponent<Text>();
//        }
//    }

//    void Update()
//    {
//        // PhotonNetwork.GetPing()�� ���� ���� Ping ���� ������ UI�� ǥ��
//        int ping = PhotonNetwork.GetPing();
//        pingText.text = $"Ping: {ping} ms";
//    }
//}
