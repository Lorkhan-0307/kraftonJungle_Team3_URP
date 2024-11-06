using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NELoadScene : NetworkEvent
{
    [SerializeField] GameObject introCanvas;
    public static string mainSceneName = "maprebuilding";

    public bool isLoadingEnded = false;

    protected override void Awake()
    {
        this.eventCode = EventCode.LoadScene;
        base.Awake();
    }


    public override void OnEvent(object customData)
    {
        string sceneName = (string)customData;


        LoadingManager.instance.LoadingStart(LoadCoroutine(sceneName),
        () => { Instantiate(introCanvas); });
    }
    IEnumerator LoadCoroutine(string sceneName)
    {
        isLoadingEnded = false;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName); // 씬 로딩
        asyncLoad.allowSceneActivation = true;

        // 씬 로딩이 완료될 때까지 대기
        while (!asyncLoad.isDone) yield return null;

        NEOnSceneLoaded.SceneLoaded();

        // 다른 플레이어 로딩 완료시 까지 대기
        while(!isLoadingEnded) yield return null;
    }

    public static void LoadSceneToAllClients()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        FindObjectOfType<ServerLogic>().InitPlayerList();

        NetworkManager.SendToClients(EventCode.LoadScene, mainSceneName);
    }

}
