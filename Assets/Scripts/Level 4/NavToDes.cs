using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

public class NavToDes : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            //Debug.Log("Now is the time to switch to attack state.");
        }
    }

    public Transform target;

    public Transform housePosition;


    private NavMeshAgent navMeshAgent;

    private EnemyVision enemyVision;

    Animator animator;

    public float speed;

    public QuestDisplay questDisplay;

    public enum State
    {
        Idle,
        Walk,
        Run,
        MeleAttack,
        RangedAttack
    }


    public State currentState;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyVision = GetComponent<EnemyVision>();

        animator = GetComponent<Animator>();
        //currentState = State.Idle;
    }
 
    void Update()
    {
        if (questDisplay.CurrentQuestIsFinish())
        {
            GameObject.Find("FoodBar").GetComponent<FoodBarController>().enabled = false;
            gameObject.SetActive(false);
        } else
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
                case State.MeleAttack:
                    MeleAttackState();
                    break;
                case State.RangedAttack:
                    RangedAttackState();
                    break;
                default:
                    break;
            }
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

        //switch to walk state
        if (Input.GetKeyDown(KeyCode.N))
        {
            currentState = State.Walk;
        }

        //switch to attack state
        if (PlayerInAttackRange() && canSwitch) { switchState(State.MeleAttack); }
    }

    void WalkState()
    {
        //Debug.Log("enemy is in Walk State");

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
        if (PlayerInAttackRange() && canSwitch) { switchState(State.MeleAttack); }

        //switch to run state
        if (enemyVision.playerInVision && canSwitch) { switchState(State.Run); }
    }


    void RunState()
    {
        //Debug.Log("enemy is in Run State");
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
        if (PlayerInAttackRange() && canSwitch) { switchState(State.MeleAttack); }

        //switch to walk state
        if (!enemyVision.playerInVision && canSwitch) { switchState(State.Walk); }
    }


    bool canDamage = false;

    public void CanDamage()
    {
        canDamage = true;
    }

    public void CannotDamage()
    {
        canDamage = false;
    }

    public bool canSwitch = false;

    public void CanSwitch()
    {
        canSwitch = true;
    }

    public void CannotSwitch()
    {
        canSwitch = false;
    }


    void MeleAttackState()  
    {
        Quaternion rotateGoal = Quaternion.LookRotation((target.position - transform.position).normalized);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotateGoal, 12f * Time.deltaTime);

        //Debug.Log("enemy is in Attack State");

        animator.SetBool("isStanding", false);

        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);

        animator.ResetTrigger("attack");
        animator.SetTrigger("attack");

        if (canDamage && PlayerInAttackRange())
        {
            canDamage = false;
            gameObject.GetComponent<KnockBack>().KnockPlayerBack();

            target.GetComponent<Player>().TakeDamage(3);
            GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Farmer Hit");
        }

        //if (target.GetComponent<Player>().isDead)
        //{
        //    currentState = State.Idle;
        //}

        if (canSwitch)
        {
            currentState = State.Walk;
        }

        //if (!isAttacking)
        //{
        //    StartCoroutine(WaitForAttackAnimation());
        //}
    }

    bool PlayerInAttackRange()
    {
        if (target.GetComponent<Player>())
        {
            if (target.GetComponent<Player>().isDead)
            {
                return false;
            } 
        }
        if (target != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, target.position);
            if (distanceToPlayer <= 1f)
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


    void RangedAttackState()
    {
        animator.SetBool("isStanding", false);

        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);

        animator.ResetTrigger("attack");
        animator.SetTrigger("attack");
    }

    bool PlayerInThrowRange()
    {
        if (target != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, target.position);
            if (distanceToPlayer <= 6f)
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


    IEnumerator WaitForAttackAnimation()
    {
        
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        yield return new WaitForSeconds(stateInfo.length);

        //if (PlayerInAttackRange())
        //{
        //    target.GetComponent<Player>().TakeDamage(1);
        //}

        //switch to walk state
        if (!PlayerInAttackRange())
        {
            currentState = State.Walk;
        } else
        {

            target.GetComponent<Player>().TakeDamage(1);
        }

        //transform.LookAt(target.position);
    }

    void switchState(State state)
    {
        if (canSwitch)
        {
            currentState = state;
            canSwitch = false;
        }
    }

    public void PlayAttackSound()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Farmer Attack");
    }
}
