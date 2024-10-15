using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable
{
    [NonSerialized] public GameObject player;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private Stats statsScript;
    private Stats playerStats;

    private float moveSpeed = 1.5f;
    private float attackDamage = 5f;
    private const float ATTACK_RANGE = 2f;

    [SerializeField] private AnimationClip attackAnimation;
    private bool playerInAttackRange;
    private float attackDelay;
    private bool hasAttacked = false;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        statsScript = GetComponent<Stats>();
        playerStats = player.GetComponent<Stats>();

        attackDelay = attackAnimation.length;
    }

    private void Update()
    {
        if (player != null)
        {
            playerInAttackRange = Vector3.Distance(transform.position, player.transform.position) < ATTACK_RANGE;

            if (playerInAttackRange)
            {
                Attack();
            }
            else if (!hasAttacked)
            {
                ChasePlayer();
            }
        }
    }

    private void LateUpdate()
    {
        if (player != null)
        {
            Vector3 lookDirection = player.transform.position - transform.position;
            lookDirection.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), 0.1f);
        }
    }

    private void ChasePlayer()
    {
        navMeshAgent.SetDestination(player.transform.position);
        navMeshAgent.speed = moveSpeed;

        if (!animator.GetBool("isWalking"))
        {
            animator.SetBool("isWalking", true);
        }
    }

    private void Attack()
    {
        if (animator.GetBool("isWalking"))
        {
            animator.SetBool("isWalking", false);
        }

        navMeshAgent.ResetPath();

        if (!hasAttacked && !playerStats.IsDead)
        {
            animator.SetTrigger("attack");
            hasAttacked = true;
            StartCoroutine(AssessAttack());
            StartCoroutine(ResetAttack());
        }
    }

    private IEnumerator AssessAttack()
    {
        yield return new WaitForSeconds(attackAnimation.length / 2.5f);

        if (playerInAttackRange)
        {
            playerStats.TakeDamage(attackDamage * statsScript.Strength);
        }
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        hasAttacked = false;

        attackDelay = attackAnimation.length;
        attackDelay += UnityEngine.Random.Range(0f, 2f);
    }
}
