using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpectatorManager : Singleton<SpectatorManager>
{
    private bool isSpectating = false;
    private List<GameObject> remainingPlayers = new List<GameObject>();
    [SerializeField] private GameObject spectatorInstance;
    private SpectatorCameraColorChange spectatorCameraColorChange;

    private void Start()
    {
        StartSpectating();
    }

    // 자신이 죽었을 때 호출
    public void StartSpectating()
    {
        Debug.Log("Start Spectating");
        spectatorCameraColorChange = GetComponent<SpectatorCameraColorChange>();
        spectatorCameraColorChange.EnableOutlineEffect(); // 아웃라인 효과 활성화
        spectatorInstance = Instantiate(spectatorInstance); // 관전 카메라 생성
        isSpectating = true;
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
