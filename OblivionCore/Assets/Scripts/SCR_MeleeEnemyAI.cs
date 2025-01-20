using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_MeleeEnemyAI : SCR_EnemyAI
{
    public float enemyDamage = 10f;
    public float attackCooldown = 1f;

    protected override void Attack()
    {
        if (timeSinceLastAttack > attackCooldown)
        {
            playerHealth.TakeDamage(enemyDamage);
            Debug.Log("Melee Attack: Damaged the player.");
            timeSinceLastAttack = 0f;
        }
    }

}
