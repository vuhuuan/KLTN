using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBackHome : MonoBehaviour
{
    public Transform target;

    private NavMeshAgent navMeshAgent;

    private EnemyVision enemyVision;

    Animator animator;

    public float speed;

    public enum State
    {
        Idle,
        Walk,
        Run,
    }


    public State currentState;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyVision = GetComponent<EnemyVision>();

        animator = GetComponent<Animator>();
        currentState = State.Walk;
    }


    void Update()
    {

        switch (currentState)
        {
            case State.Idle:
                IdleState();
                break;
            case State.Walk:
                WalkState();
                break;
            case State.Run:
                RunState();
                break;
        }
    }

    private void LookAtDirection()
    {
        Vector3 direction = transform.forward;

        if (direction.magnitude > 0.0001f)
        {
            transform.LookAt(transform.position + direction);
        }
    }

    void IdleState()
    {
        animator.SetBool("isStanding", true);

        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
        animator.ResetTrigger("attack");

        //switch to attack state
        if (PlayerInAttackRange()) { currentState = State.Idle; }
    }

    void WalkState()
    {
        animator.SetBool("isWalking", true);

        animator.SetBool("isStanding", false);
        animator.SetBool("isRunning", false);
        animator.ResetTrigger("attack");

        if (target != null)
        {
            navMeshAgent.speed = speed;
            navMeshAgent.SetDestination(target.position);
            LookAtDirection();
        }


        //switch to attack state
        if (PlayerInAttackRange()) { currentState = State.Idle; }

        //switch to run state
        if (!enemyVision.playerInVision) { currentState = State.Run; }
    }


    void RunState()
    {
        animator.SetBool("isStanding", false);

        animator.SetBool("isWalking", false);

        animator.SetBool("isRunning", true);

        animator.ResetTrigger("attack");

        if (target != null)
        {
            navMeshAgent.speed = speed * 3;

            navMeshAgent.SetDestination(target.position);
            LookAtDirection();
        }


        //switch to attack state
        if (PlayerInAttackRange()) { currentState = State.Idle; }

        //switch to walk state
        if (enemyVision.playerInVision) { currentState = State.Walk; }
    }

    bool PlayerInAttackRange()
    {
        if (target != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, target.position);
            if (distanceToPlayer <= 0.5f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

}
