using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpectatorManager : Singleton<SpectatorManager>
{
    public GameObject spectatorPrefab; // Reference to your Spectator prefab
    private bool isSpectating = false;
    private List<GameObject> remainingPlayers = new List<GameObject>();
    private GameObject spectatorInstance;

    // private void Awake()
    // {
    //     StartSpectating();
    // }

    // 자신이 죽었을 때 호출
    public void StartSpectating()
    {
        Debug.Log("Start Spectating");
        isSpectating = true;
        spectatorInstance = Instantiate(spectatorPrefab);
    }

    // 플레이어가 죽었을 때 호출
    public void RemoveRemainingPlayer(GameObject player)
    {
        if (isSpectating)
            spectatorInstance.GetComponent<SpectatorCamera>().RemovePlayer(player);
    }
}
