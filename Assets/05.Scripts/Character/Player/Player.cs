using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour, ICharacter
{
    public bool is_player { get; set; }
    public bool is_damageable { get; set; }

    public CharacterType type;

    public void SetPlayer(CharacterType type)
    {
        this.type = type;
    }

    private void Start()
    {
        is_player = true;
        is_damageable = true;
    }

    public virtual void OnDamaged(GameObject attacker)
    {
        Debug.Log(attacker.gameObject.name);
        GetComponentInChildren<Animator>().SetTrigger("Death");
        CapsuleCollider cc = GetComponent<CapsuleCollider>();
        Destroy(cc);

        Transform playerObjectTransform = transform.Find("PlayerObjects(Clone)");
        // 버튼 Destroy
        Destroy(playerObjectTransform.GetComponent<PlayerMovement>().interactButton.gameObject);
        if (playerObjectTransform) Destroy(playerObjectTransform.gameObject);
        //GetComponent<BloodEffect>().OnBloodEffect();
    }

    public virtual void OnDead()
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnAttack(GameObject victim)
    {
        // Send Request to server at here
        Debug.Log("TRANSMIT TO SERVER");

        NEAttackRequest.AttackEntity(GetComponent<PhotonView>(), victim.GetComponent<PhotonView>());
    }

    public virtual bool AttackDetection(GameObject target)
    {
        return false;
    }

    public virtual void PlayKillSound()
    {

    }

    public virtual void PlayDeathSound()
    {

    }
}
