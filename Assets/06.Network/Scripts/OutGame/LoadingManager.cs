using Michsky.UI.Dark;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] ModalWindowManager loadingPanel;

    public static LoadingManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadingStart()
    {
        loadingPanel.ModalWindowIn();
    }
    public void LoadingEnd()
    {
        StartCoroutine(EndCoroutine());
    }
    IEnumerator EndCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        loadingPanel.ModalWindowOut();
    }

    public void LoadMainScene()
    {
        StartCoroutine(LoadCoroutine());
    }
    IEnumerator LoadCoroutine()
    {
        LoadingStart();

        yield return new WaitForSeconds(1f);
        
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }


        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0); // 씬 로딩
        asyncLoad.allowSceneActivation = true;

        // 로딩이 완료될 때까지 대기
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        while (PhotonNetwork.NetworkClientState
        != Photon.Realtime.ClientState.JoinedLobby)
        {
            yield return null;
        }

        LoadingEnd();
    }
}
