using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : Player
{
    private GameObject hungerParticle;

    [SerializeField] private AudioSource monster_kill_sound;
    
    public override void OnAttack(GameObject victim)
    {
        base.OnAttack(victim);

        // Todo: hunger time reset
        //attacker �� hunger time reset ȣ��
        // Monster 에게만
        switch(victim.GetComponent<Player>().type)
        {
            case CharacterType.NPC:

                Debug.Log("Your gauge is Max");
                FindObjectOfType<HungerSlider>().SetHungerMax();
                NEHungerGauge.HungerEvent(false);

                break;
        }

        //MonsterKillSoundPlay();
        //GameManager.instance.OnKilled += MonsterKillSoundPlay();

    }

    public override void OnDamaged(GameObject attacker)
    {
        base.OnDamaged(attacker);

        // 여기서 Destroy 결과 전송
        if (GetComponent<PhotonView>().AmOwner)
        {
            PhotonNetwork.Destroy(this.gameObject);
            NEPlayerDeath.PlayerDeath();
            SpectatorManager.instance.StartSpectating();
        }
        else
        {
            SpectatorManager.instance.RemoveRemainingPlayer(this.gameObject);
        }
    }

    public override void OnDead()
    {
        base.OnDead();
    }

    public void OnNightVisibleScientist()
    {
        MonsterOutlineEffect moe = GetComponentInChildren<MonsterOutlineEffect>();
        moe.EnableOutlineEffect();
    }

    // 괴물의 밤시야 끄기
    public void OnDayUniteVisibilityScientist()
    {
        MonsterOutlineEffect moe = GetComponentInChildren<MonsterOutlineEffect>();
        moe.DisableOutlineEffect();
    }

    // Use this when Hunger Gauge reach 0
    public void OnHunger()
    {
        hungerParticle = GetComponentInChildren<ParticleSystem>(true).GameObject();
        hungerParticle.SetActive(true);
    }

    // Use this when Hunger Gauge reset
    public void NoHunger()
    {
        hungerParticle = GetComponentInChildren<ParticleSystem>(true).GameObject();
        hungerParticle.SetActive(false);
    }

    public override bool AttackDetection(GameObject target)
    {
        if (TimeManager.instance.GetisDay())
        {
            if (target.CompareTag("NPC")) return true;
        }
        else
        {
            if (target.CompareTag("Player")) return true;
        }
        return false;
    }

    public override void PlayKillSound()
    {
        base.PlayKillSound();
        monster_kill_sound.Play();
    }
}
