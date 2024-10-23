using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpectatorMode : MonoBehaviour
{
    private List<GameObject> RemainingPlayers = new List<GameObject>(); // 남아있는 플레이어
    public GameObject SpectatingTarget; // 내가 관전 중인 플레이어
    private bool isSpectating = false;


    // 마우스 클릭했을 때 호출되는 함수
    private void MoveToAnotherPlayer(int idx_move)
    {
        if (RemainingPlayers.Count == 0)
            return;

        int idx = RemainingPlayers.IndexOf(SpectatingTarget);
        idx += idx_move;
        if (idx < 0)
            idx = RemainingPlayers.Count - 1;
        else if (idx >= RemainingPlayers.Count)
            idx = 0;

        SpectatingTarget = RemainingPlayers[idx];
        GetComponent<SpectatorCamera>().SetSpectatingTarget(SpectatingTarget);
        Debug.Log("Move to another player: " + SpectatingTarget.name);
    }

    // 마우스 왼쪽 클릭 시 이전 플레이어로 전환
    public void OnPrevPlayer(InputAction.CallbackContext context)
    {
        if (isSpectating)
        {
            MoveToAnotherPlayer(-1); // 왼쪽 클릭은 이전 플레이어
        }
    }

    // 마우스 오른쪽 클릭 시 다음 플레이어로 전환
    public void OnNextPlayer(InputAction.CallbackContext context)
    {
        if (isSpectating)
        {
            MoveToAnotherPlayer(1); // 오른쪽 클릭은 다음 플레이어
        }
    }

    // 관전모드 시작하는 함수
    public void StartSpectating()
    {
        Debug.Log("Start Spectating");

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }

        isSpectating = true;
        RemainingPlayers = new List<GameObject>();
        RemainingPlayers.AddRange(GameObject.FindGameObjectsWithTag("Player"));

        Player p = NetworkManager.Instance.myPlayer; // 자기가 조종하고 있는 플레이어
        RemainingPlayers.Remove(p.gameObject);

        transform.SetParent(null);
        gameObject.AddComponent<SpectatorCamera>(); // 쫓아다니기 시작
        UpdateSpectatingPlayer();
    }

    // 누군가 죽었을 때 호출되는 함수
    private void UpdateSpectatingPlayer()
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

    // 누군가 죽었을 때 호출되는 함수
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