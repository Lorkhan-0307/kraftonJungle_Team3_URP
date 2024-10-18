using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    bool is_player { get; set; }
    bool is_damageable { get; set; }
    void OnDamaged(GameObject attacker);
    void OnDead();

    void OnAttack(GameObject victim);
}
