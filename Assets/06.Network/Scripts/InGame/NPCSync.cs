using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NPCSync : MonoBehaviourPun
{
    NavMeshAgent agent;

    private void Awake()
    {
         agent = GetComponent<NavMeshAgent>();
    }

    public void SetDestination(Vector3 dest)
    {
        photonView.RPC("SyncDestination", RpcTarget.All, dest);
    }

    [PunRPC]
    public void SyncDestination(Vector3 dest)
    {
        if(agent)
            agent.SetDestination(dest);
    }
}
