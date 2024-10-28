using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PhotonView))]
public class NetSceneManager : MonoBehaviourPun
{
    public static NetSceneManager Instance;


    public string mainSceneName;

    public void LoadSceneToAllClients(string sceneName)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        photonView.RPC("LoadScene", RpcTarget.AllBuffered, sceneName); // 모든 클라이언트에 동기화

    }

    #region RPC
    [PunRPC]
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);    // 씬 로딩
    }
    #endregion
}
