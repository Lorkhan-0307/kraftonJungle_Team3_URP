using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Player
{
    public override void OnAttack(GameObject victim)
    {
        base.OnAttack(victim);
    }

    public override void OnDamaged(GameObject attacker)
    {
        base.OnDamaged(attacker);
    }

    public override void OnDead()
    {
        base.OnDead();
    }
}
