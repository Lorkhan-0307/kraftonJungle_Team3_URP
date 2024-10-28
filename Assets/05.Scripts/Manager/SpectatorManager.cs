using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpectatorManager : Singleton<SpectatorManager>
{
    [SerializeField] private GameObject spectatorPrefab;
    private bool isSpectating = false;
    private List<GameObject> remainingPlayers = new List<GameObject>();
    private GameObject spectatorInstance;

    private void Awake()
    {
        StartSpectating();
    }

    // 자신이 죽었을 때 호출
    public void StartSpectating()
    {
        Debug.Log("Start Spectating");
        isSpectating = true;
        spectatorInstance = Instantiate(spectatorPrefab);
    }

    // TODO : 동시에 죽었을 때 리스트 제대로 업뎃 안되는 문제 해결
    // 플레이어가 죽었을 때 호출
    public void RemoveRemainingPlayer(GameObject player)
    {
        if (isSpectating)
        {
            spectatorInstance.GetComponent<SpectatorCamera>().RemovePlayer(player);
        }
    }
}
