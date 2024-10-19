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
                // Todo: hunger time reset
                //attacker �� hunger time reset ȣ��
                FindObjectOfType<HungerSlider>().OnMonsterAteNPC();
                Debug.Log("MONSTER ATTACKED NPC");
                break;
            case CharacterType.Scientist:
                // Todo: less day time
                FindObjectOfType<TimeSwitchSlider>().FastTime();
                Debug.Log("SCIENTIST ATTACKED NPC");
                break;
        }
        
        //tss에서 allNPC가 바뀌기 전까지 유지할 스크립트.
        // TODO : TSS에서의 변화에 따라 이 부분 수정 및 캡슐화
        TimeSwitchSlider tss = FindObjectOfType<TimeSwitchSlider>();
        List<GameObject> npcList = new List<GameObject>(tss.allNPC);
        npcList.Remove(this.gameObject);
        tss.allNPC = npcList.ToArray();
        
        // 여기서 Destroy 결과 전송
        Destroy(this.gameObject);
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
}
