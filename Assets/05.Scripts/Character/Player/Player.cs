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
        type = type;
    }

    private void Start()
    {
        is_player = true;
        is_damageable = true;
    }

    public virtual void OnDamaged(GameObject attacker)
    {
        Debug.Log(attacker.gameObject.name);
    }

    public virtual void OnDead()
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnAttack(GameObject victim)
    {
        // Send Request to server at here
        Debug.Log("TRANSMIT TO SERVER");
        //throw new System.NotImplementedException();
    }
}
