using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEAttackRequest : NetworkEvent
{
    protected override void Awake()
    {
        this.eventCode = EventCode.AttackRequest;
        base.Awake();
    }

    public override void Init()
    {
        killLogger = FindObjectOfType<KillLogManager>();
        GameManager.instance.OnKilled += KillLogCallback;
        GameManager.instance.OnKilled += KillAniCallback;
    }

    void KillAniCallback(GameObject o1, GameObject o2)
    {
        AnimationSync ani = o1.GetComponentInChildren<AnimationSync>();
        ani?.SyncAniTrigger("Stab");

        ani = o2.GetComponentInChildren<AnimationSync>();
        ani?.SyncAniTrigger("Death");
    }
    public override void OnEvent(object customData)
    {
        object[] datas = (object[])customData; // 전송된 데이터를 받음
                                                           //데이터 : {공격자ID, 피격자ID}

        PhotonView from = PhotonNetwork.GetPhotonView((int)datas[0]);
        PhotonView to = PhotonNetwork.GetPhotonView((int)datas[1]);

        if (!from) return;
        if (!to) return;

        if (from.GetComponent<Player>().type == CharacterType.Monster)
        {
            from.GetComponent<Monster>().OnTransformation(TimeManager.instance.GetisDay());
        }
        //게임매니저 이벤트 실행
        GameManager.instance.OnKilled?.Invoke(from.gameObject, to.gameObject);


        //공격자 피격자 이용해서 해야하는 로직들 처리하기
        to.GetComponent<Player>().OnDamaged(from.gameObject);

    }

    public static void AttackEntity(PhotonView from, PhotonView to)
    {
        object[] result = { from.ViewID, to.ViewID };

        NetworkManager.SendToClients(EventCode.AttackRequest, result);
    }

    KillLogManager killLogger = null;

    void KillLogCallback(GameObject from, GameObject to)
    {
        //string killer = from.GetComponent<PhotonView>().Owner.NickName;
        //string victim = "NPC";

        //if(to.GetComponent<Player>().type != CharacterType.NPC)
        //    victim = to.GetComponent<PhotonView>().Owner.NickName;
        string killer = from.GetComponent<Player>()?.type.ToString();
        string victim = to.GetComponent<Player>()?.type.ToString();

        killLogger.AddKillLog(new KillLogElement(killer, victim));
    }
}
