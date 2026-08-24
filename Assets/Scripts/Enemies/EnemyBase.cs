using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    protected GameObject player;

    [SerializeField] float enemyHP;
    [SerializeField] float enemySpeed;
    [SerializeField] float enemyVision;
    [SerializeField] protected float attackRange;
    [SerializeField] EnemyState currentState;
    [SerializeField] float confusedSpinSpeed;

    public delegate void EnemyKilled();
    public static event EnemyKilled OnEnemyKilled;

    protected virtual void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                if (CanSeePlayer())
                {
                    currentState = EnemyState.Chasing;
                }
                break;
            case EnemyState.Chasing:

                float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
                if (distanceToPlayer <= attackRange)
                {
                    Attack();
                }
                Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
                transform.Translate(directionToPlayer * enemySpeed * Time.deltaTime, Space.World);
                if (CanSeePlayer() == false)
                {
                    currentState = EnemyState.Confused;
                }
                break;
            case EnemyState.Confused:
                transform.Rotate(Vector3.up * confusedSpinSpeed * Time.deltaTime);
                if (CanSeePlayer())
                {
                    currentState = EnemyState.Chasing;
                }
                break;
        }
    }

    public enum EnemyState
    {
        Idle,
        Chasing,
        Confused,
    }

    bool CanSeePlayer()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance > enemyVision)
        {
            return false; // cant se player
        }

        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, directionToPlayer, out hit, enemyVision))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                return true; // can see player
            }
        }
        return false; // cant see player
    }

    public void TakeDamage(float damage)
    {
        enemyHP -= damage;
        if (enemyHP <= 0)
        {
            OnEnemyKilled?.Invoke();
            Destroy(gameObject);
        }
    }

    protected virtual void Attack() { }
}
