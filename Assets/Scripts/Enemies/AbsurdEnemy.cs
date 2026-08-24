using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsurdEnemy : EnemyBase
{
    [SerializeField] List<WeightedAttack> weightedAttacks;
    [SerializeField] float attackDamage;
    [SerializeField] float attackRate;
    [SerializeField] float knockBackForce;
    [SerializeField] float invertDuration;
    private bool canAttack;
    private PlayerController playerController;
    private PlayerHealth playerHealth;


    protected override void Start()
    {
        base.Start();
        canAttack = true;
        playerController = player.GetComponent<PlayerController>();
        playerHealth = player.GetComponent<PlayerHealth>();
    }



    [System.Serializable]
    public struct WeightedAttack
    {
        public AttackType attack;
        public float weight;
    }


    public enum AttackType
    {
        Melee,
        Knockback,
        ControlInvert
    }

    protected override void Attack()
    {
        if (!canAttack) return;

        float totalWeight = 0;
        foreach (var a in weightedAttacks) totalWeight += a.weight;

        float roll = Random.Range(0, totalWeight);

        AttackType selectedAttack = AttackType.Melee;
        foreach (var a in weightedAttacks)
        {
            roll -= a.weight;
            if (roll <= 0)
            {
                selectedAttack = a.attack;
                break;
            }
        }

        switch (selectedAttack)
        {
            case AttackType.Melee:
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(attackDamage);
                }

                break;
            case AttackType.Knockback:
                Vector3 knockbackDirection = (player.transform.position - transform.position).normalized;
                if (playerController != null)
                {
                    playerController.ApplyKnockback(knockbackDirection, knockBackForce);
                }
                break;
            case AttackType.ControlInvert:
                playerController.StartCoroutine(playerController.InvertControls(invertDuration));
                break;
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



// list of attacks
// regular melee attack
// knockback like rusher, should send the player flying
// melee attack that teleports the player to a random location in the arena
// melee attack that distorts the vision of the player for a few seconds
// when in line of sight the player camera locks onto the enemy, slowly zooming in, some vignetting and other visual effects, the player should enter a frenzy and hold down the fire button emptying the mag.
// melee attack that inverts the camera and WASD controls for a few seconds