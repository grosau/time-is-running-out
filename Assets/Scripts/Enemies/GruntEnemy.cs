using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntEnemy : EnemyBase
{
    [SerializeField] float attackDamage;
    [SerializeField] float attackRate;

    bool canAttack;
    [SerializeField] GameObject weaponModel;

    protected override void Start()
    {
        base.Start();
        canAttack = true;
    }

    protected override void Attack()
    {
        if (!canAttack) return;

        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, directionToPlayer, out hit, attackRange))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                PlayerHealth playerHealth = hit.collider.gameObject.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(attackDamage);
                }
            }
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
