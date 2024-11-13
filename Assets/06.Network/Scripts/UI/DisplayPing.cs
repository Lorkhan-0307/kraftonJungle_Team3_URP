using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPing : MonoBehaviour
{
    public TMP_Text pingText;  // Ping 정보를 표시할 UI Text
    [SerializeField]
    float refreshRate = 1f;

    WaitForSeconds wait;
    
    private int original_ping = -999;
    private int new_ping;


    private void Start()
    {
        wait = new WaitForSeconds(refreshRate);
        StartCoroutine(RefreshPing());
    }

    /*IEnumerator RefreshPing()
    {
        while (pingText != null)
        {
            // PhotonNetwork.GetPing()를 통해 현재 Ping 값을 가져와 UI에 표시
            pingText.text = $"Ping: {PhotonNetwork.GetPing()} ms";
            yield return wait;
        }
    }*/
    
    IEnumerator RefreshPing()
    {
        while (pingText != null)
        {
            // PhotonNetwork.GetPing()를 통해 현재 Ping 값을 가져와 비교 후 다르면 교체
            new_ping = PhotonNetwork.GetPing();
            if (new_ping != original_ping)
            {
                original_ping = new_ping;
                pingText.text = $"Ping: {original_ping} ms";
            }
            yield return wait;
            // PhotonNetwork.GetPing()를 통해 현재 Ping 값을 가져와 UI에 표시
        }
    }

    // 최적화 작업
    //void Update()
    //{
    // PhotonNetwork.GetPing()를 통해 현재 Ping 값을 가져와 UI에 표시
    // pingText.text = $"Ping: {PhotonNetwork.GetPing()} ms";
    //}
}
