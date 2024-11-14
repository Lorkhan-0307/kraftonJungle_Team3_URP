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
        victim.GetComponent<BloodEffect>().OnBloodEffect();
        // victim 이 Monster이면
        //if (victim.GetComponent<Player>().type == CharacterType.Monster)
        //    // Timeline 실행
        //    victim.GetComponent<Monster>().OnDeadTimeline(gameObject);
    }

    public override void OnDamaged(GameObject attacker)
    {
        base.OnDamaged(attacker);

        //Debug.Log("ONDAMAGED");


    }

    public void OnAttackFinished()
    {
        // 여기부터 끝까지 Monster.cs의 OnAttackFinished()로 옮겨야 함
        Transform playerObjectTransform = transform.Find("PlayerObjects(Clone)");
        if (playerObjectTransform) Destroy(playerObjectTransform.gameObject);

        // 여기서 Destroy 결과 전송
        if (GetComponent<PhotonView>().AmOwner)
        {
            NEPlayerDeath.PlayerDeath();
            // 게임이 끝났으면 실행 X
            if (!TimeManager.instance.isEnd)
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

    public override bool AttackDetection(GameObject target)
    {
        // 낮일 때
        if (TimeManager.instance.GetisDay())
        {
            if (target.CompareTag("Player") || target.CompareTag("NPC")) return true;
        }
        return false;
    }
}
