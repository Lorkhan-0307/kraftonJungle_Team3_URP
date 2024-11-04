using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NELoadScene : NetworkEvent
{
    public static string mainSceneName = "maprebuilding";

    protected override void Awake()
    {
        this.eventCode = EventCode.LoadScene;
        base.Awake();
    }


    public override void OnEvent(object customData)
    {
        string sceneName = (string)customData;

        StartCoroutine(LoadCoroutine(sceneName));
    }
    IEnumerator LoadCoroutine(string sceneName)
    {
        LoadingManager.instance.LoadingStart();
        yield return new WaitForSeconds(1f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName); // 씬 로딩
        asyncLoad.allowSceneActivation = true;

        // 로딩이 완료될 때까지 대기
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        NEOnSceneLoaded.SceneLoaded();
    }

    public static void LoadSceneToAllClients()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        FindObjectOfType<ServerLogic>().InitPlayerList();

        NetworkManager.SendToClients(EventCode.LoadScene, mainSceneName);
    }

}
