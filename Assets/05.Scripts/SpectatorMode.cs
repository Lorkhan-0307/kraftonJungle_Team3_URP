using System.Collections.Generic;
using UnityEngine;

public class SpectatorMode : MonoBehaviour
{
    public List<GameObject> RemainingPlayers = new List<GameObject>();

    private GameObject SpectatingPlayer; // 내가 관전 중인 플레이어
    private bool isSpectating = false;

    private void UpdateSpectatingPlayer()
    {
        if (RemainingPlayers.Count == 0)
        {
            Debug.Log("No players to spectate");
            return;
        }

        // 플레이어가 죽었을 때 다음 플레이어로 변경
        if (SpectatingPlayer == null)
        {
            SpectatingPlayer = RemainingPlayers[0];
            Debug.Log("Remaining Players: " + SpectatingPlayer.name);
            transform.SetParent(RemainingPlayers[0].transform);
            transform.position = RemainingPlayers[0].transform.position;
            transform.rotation = RemainingPlayers[0].transform.rotation;
        }
        else
        {
            Debug.Log("your remaining player is null");
        }
    }

    public void StartSpectating()
    {
        isSpectating = true;
        Debug.Log("Start Spectating");
        RemainingPlayers.AddRange(GameObject.FindGameObjectsWithTag("Player"));

        Player p = NetworkManager.Instance.myPlayer; // 자기가 조종하고 있는 플레이어
        RemainingPlayers.Remove(p.gameObject);
        UpdateSpectatingPlayer();
    }

    public void RemovePlayer(GameObject player)
    {
        if (isSpectating)
        {
            RemainingPlayers.Remove(player);
            // 죽은 친구가 지금 관전 중인 친구면 카메라를 옮겨야 함
            if (player == SpectatingPlayer)
                UpdateSpectatingPlayer();
        }
    }
}