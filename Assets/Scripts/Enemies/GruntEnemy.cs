using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntEnemy : EnemyBase
{
    [SerializeField] float attackDamage;
    [SerializeField] float attackRate;
    [SerializeField] float attackRange;
    bool canAttack;
    [SerializeField] GameObject weaponModel;

    protected override void Start()
    {
        base.Start();
        canAttack = true;
    }
}
