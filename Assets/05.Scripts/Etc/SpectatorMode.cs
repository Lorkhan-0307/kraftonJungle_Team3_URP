using System.Collections.Generic;
using UnityEngine;

public class SpectatorMode : MonoBehaviour
{
    private List<GameObject> RemainingPlayers = new List<GameObject>(); // 남아있는 플레이어
    public GameObject SpectatingTarget; // 내가 관전 중인 플레이어
    private bool isSpectating = false;

    public void StartSpectating()
    {
        Debug.Log("Start Spectating");
        isSpectating = true;
        RemainingPlayers = new List<GameObject>();
        RemainingPlayers.AddRange(GameObject.FindGameObjectsWithTag("Player"));

        Player p = NetworkManager.Instance.myPlayer; // 자기가 조종하고 있는 플레이어
        RemainingPlayers.Remove(p.gameObject);

        transform.SetParent(null);
        gameObject.AddComponent<SpectatorCamera>(); // 쫓아다니기 시작
        gameObject.AddComponent<CharacterController>(); // 마우스 이동 추가
        UpdateSpectatingPlayer();
    }

    public void UpdateSpectatingPlayer()
    {
        // 어떤 플레이어가 죽었을 때마다 호출됨
        // 남아있는 플레이어 중에서 다음 플레이어로 변경

        if (RemainingPlayers.Count == 0)
        {
            Debug.Log("No players to spectate");
            return;
        }

        // 다음 플레이어로 변경
        SpectatingTarget = RemainingPlayers[0];
        GetComponent<SpectatorCamera>().SetSpectatingTarget(SpectatingTarget);
        Debug.Log("Remaining Players: " + SpectatingTarget.name);
    }

    public void RemoveRemainingPlayer(GameObject player)
    {
        if (isSpectating)
        {
            RemainingPlayers.Remove(player);
            // 죽은 친구가 지금 관전 중인 친구면 카메라를 옮겨야 함
            if (player == SpectatingTarget)
                UpdateSpectatingPlayer();
        }
    }
}