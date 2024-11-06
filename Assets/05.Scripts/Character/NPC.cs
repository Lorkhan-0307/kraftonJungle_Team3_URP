using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class NPC : Player
{
    [SerializeField] private AudioSource deathSound;
    
    private void Start()
    {
        is_player = false;
        is_damageable = true;
        type = CharacterType.NPC;
    }

    public override void OnDamaged(GameObject attacker)
    {
        //GetComponent<BloodEffect>().OnBloodEffect();
        switch (attacker.GetComponent<Player>().type)
        {
            case CharacterType.Monster:
                Debug.Log("MONSTER ATTACKED NPC");
                break;
            case CharacterType.Scientist:
                if(NetworkManager.Instance.IsServer())
                {
                    // Todo: less day time
                    TimeManager.instance.AccelerateTime();
                    NEAccelTime.TimeAccel(
                        TimeManager.instance.GetElapsedTime());

                    Debug.Log("SCIENTIST ATTACKED NPC");
                }
                break;
        }

        // tss에서 allNPC가 바뀌기 전까지 유지할 스크립트.
        // tss에서 NPCManaer로 이동
        // TODO : TSS에서의 변화에 따라 이 부분 수정 및 캡슐화
        // -> 데모 이후 수정 에정
        NPCManager npcManager = FindObjectOfType<NPCManager>();
        npcManager.allNPC.Remove(this.gameObject);
        NetworkManager.Instance.NPCCount--;



        // 여기서 Destroy 결과 전송



        // 10.30 오전 작업물

        // 이 Trigger도 서버 전송 필요


        // 여기서 죽음 처리를 해야하는데...
        // 스크립트를 떼거나, 더이상 Detection이 안되게 하고, collider도 없애야 한다.

        // 아래에 해당하는 사항을 서버로 쏴야한다..!
        // Todo : 이전 작업물... 공격 교체로 인해 렌더러를 끈다. 주석처리한 코드가 이를 실행하는데, 바닥의 핏자국 때문에 문제가 된다.
        GetComponent<NavMeshAgent>().isStopped = true;
        GetComponentInChildren<Animator>().SetTrigger("Death");
        //GetComponentInChildren<Animator>().gameObject.SetActive(false);
        gameObject.tag = "Untagged";
        CapsuleCollider cc = GetComponent<CapsuleCollider>();
        Destroy(cc);

        //if(GetComponent<PhotonView>().AmOwner)
        //PhotonNetwork.Destroy(this.gameObject);
    }

    public override void OnDead()
    {
        Debug.Log("NPC.C :: NPC Dead");
        // Todo: Destroy
    }

    public override void OnAttack(GameObject victim)
    {
        throw new System.NotImplementedException();
    }

    public override void PlayDeathSound()
    {
        base.PlayDeathSound();
        deathSound.Play();
    }

}
