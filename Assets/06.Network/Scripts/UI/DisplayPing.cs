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

    private void Start()
    {
        wait = new WaitForSeconds(refreshRate);
        StartCoroutine(RefreshPing());
    }

    IEnumerator RefreshPing()
    {
        while (pingText != null)
        {
            // PhotonNetwork.GetPing()를 통해 현재 Ping 값을 가져와 UI에 표시
            pingText.text = $"Ping: {PhotonNetwork.GetPing()} ms";
            yield return wait;
        }
    }

    // 최적화 작업
    //void Update()
    //{
    // PhotonNetwork.GetPing()를 통해 현재 Ping 값을 가져와 UI에 표시
    // pingText.text = $"Ping: {PhotonNetwork.GetPing()} ms";
    //}
}
