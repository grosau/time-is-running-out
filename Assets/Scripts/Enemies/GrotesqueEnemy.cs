using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrotesqueEnemy : EnemyBase
{
    [SerializeField] float attackDamage;
    [SerializeField] float attackRate;
    private bool canAttack;

    protected override void Start()
    {
        base.Start();
        canAttack = true;
    }

    protected override void Attack()
    {
        if (!canAttack) return;

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }
        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackRate);
        canAttack = true;
    }
}
