using System.Collections;
using System.Collections.Generic;
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
                break;
            case CharacterType.Scientist:
                // Todo: less day time
                break;
        }
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
