using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : Player
{
    private GameObject hungerParticle;
    public override void OnAttack(GameObject victim)
    {
        base.OnAttack(victim);
    }

    public override void OnDamaged(GameObject attacker)
    {
        base.OnDamaged(attacker);
        
        // 여기서 Destroy 결과 전송
        Destroy(this.gameObject);
        //PhotonNetwork.Destroy(this.gameObject);
    }

    public override void OnDead()
    {
        base.OnDead();
    }

    // Use this when Hunger Gauge reach 0
    public void OnHunger()
    {
        hungerParticle = GetComponentInChildren<ParticleSystem>().GameObject();
        hungerParticle.SetActive(true);
    }

    // Use this when Hunger Gauge reset
    public void NoHunger()
    {
        hungerParticle = GetComponentInChildren<ParticleSystem>().GameObject();
        hungerParticle.SetActive(false);
    }
}
