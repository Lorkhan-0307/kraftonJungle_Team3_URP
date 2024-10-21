using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPing : MonoBehaviour
{
    public Text pingText;  // Ping 정보를 표시할 UI Text


    void Update()
    {
        // PhotonNetwork.GetPing()를 통해 현재 Ping 값을 가져와 UI에 표시
        int ping = PhotonNetwork.GetPing();
        pingText.text = $"Ping: {ping} ms";
    }
}
