using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Player
{
    public override void OnAttack(GameObject victim)
    {
        base.OnAttack(victim);
    }

    public override void OnDamaged(GameObject attacker)
    {
        base.OnDamaged(attacker);
        
        // 여기서 Destroy 결과 전송
        Destroy(this);
        //PhotonNetwork.Destroy(this.gameObject);
    }

    public override void OnDead()
    {
        base.OnDead();
    }
}
