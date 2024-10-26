using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class NPC : Player
{
    private void Start()
    {
        is_player = false;
        is_damageable = true;
        type = CharacterType.NPC;
    }

    public override void OnDamaged(GameObject attacker)
    {
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
        //Destroy(this.gameObject);
        if(GetComponent<PhotonView>().AmOwner)
            PhotonNetwork.Destroy(this.gameObject);
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
}
