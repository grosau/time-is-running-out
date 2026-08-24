using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RusherEnemy : EnemyBase
{
    [SerializeField] float explosionDamage;
    [SerializeField] float explosionRadius;

    protected override void Start()
    {
        base.Start();
        attackRange = explosionRadius;
    }

    protected override void Attack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in hitColliders)
        {
            PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(explosionDamage);

                // knockback
                Vector3 knockbackDirection = (hit.transform.position - transform.position).normalized;
                PlayerController playerController = hit.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.ApplyKnockback(knockbackDirection, explosionDamage);
                }
            }
            // if another enemy
            EnemyBase enemy = hit.GetComponent<EnemyBase>();
            if (enemy != null && hit.gameObject != gameObject)
            {
                enemy.TakeDamage(explosionDamage);
            }
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
