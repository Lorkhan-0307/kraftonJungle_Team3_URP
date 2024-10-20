using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Scientist : Player
{

    public override void OnAttack(GameObject victim)
    {
        base.OnAttack(victim);
    }

    public override void OnDamaged(GameObject attacker)
    {
        base.OnDamaged(attacker);
        Debug.Log("ONDAMAGED");
        // 여기서 Destroy 결과 전송
        //Destroy(this.gameObject);
        PhotonNetwork.Destroy(this.gameObject);
        NetworkManager.Instance.PlayerDeath();
    }

    public override void OnDead()
    {
        base.OnDead();
    }

    public override bool AttackDetection(GameObject target)
    {
        // 낮일 떄
        if (GameManager.instance.GetTime())
        {
            if(target.CompareTag("Player") || target.CompareTag("NPC")) return true;
        }
        return false;
    }
}
