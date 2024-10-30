using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class Interact : MonoBehaviourPun
{
    public bool isInteractable = true;

    protected virtual void Interaction()
    {
        // 기본적으로 interaction이 가능한 경우에는 interact 한다...
        
        // 상속받은 child들은 이거에 맞춰서... door인경우 열리면 닫고 닫히면 연다...
        // 근데 interaction 중인걸 또 하려고 하면 못하게
    }
    
    public void BroadcastInteraction()
    {
        photonView.RPC("ListenInteraction", RpcTarget.All);
    }
    [PunRPC]
    public void ListenInteraction()
    {
        Interaction();
    }
    
}
