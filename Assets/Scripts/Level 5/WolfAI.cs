using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class WolfAI : MonoBehaviour
{
    public GameObject BatteRing;

    public float walkAroundDistance;
    public float distanceToCenter;

    public Animator animator;

    public bool canSwitchState;

    public int AttackDamage;

    //private Vector3 centerPoint;  
    public enum State
    {
        Idle,
        RunToTheBound,
        WalkAround,
        ApproachPlayer,
        ComboAttack,
        JumpAttack, // prepare, jump, cooldown
        Sneaky,
        Default,
    }

    private State currentState;
    private void Start()
    {
        currentState = State.Idle;
        animator = GetComponent<Animator>();

    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                IdleLogic();
                break;
            case State.RunToTheBound:
                RunToTheBoundLogic();
                break;
            case State.WalkAround:
                WalkAroundLogic();
                break;
            case State.ComboAttack:
                ComboAttackLogic();
                break;
            case State.Sneaky:
                SneakyLogic();
                break;
            case State.JumpAttack:
                JumpAttackLogic();
                break;
            case State.ApproachPlayer:
                ApproachPlayer();
                break;
            case State.Default:
                DefaultLogic();
                break;
        }
    }

    // ---------------------------------------STATE---------------------------------------

    public float WalkAroundSpeed;
    bool prepared = false;
    void WalkAroundLogic()
    {
        //Debug.Log("Wolf enter Walk Around state");

        //animator logic
        animator.SetBool("idle", false);
        animator.SetBool("run to the bound", false);
        animator.SetBool("walk around", true);


        // walk around
        Vector3 currentPosition = transform.position;

        Vector3 centerPoint = BatteRing.transform.position;

        float angle = Time.deltaTime * WalkAroundSpeed; 

        Vector3 newPosition = centerPoint + Quaternion.Euler(0, angle, 0) * (currentPosition - centerPoint);

        Quaternion rotateGoal = Quaternion.LookRotation((newPosition - transform.position).normalized);
        float additionalAngle = 15f;
        Quaternion additionalRotation = Quaternion.Euler(0f, additionalAngle, 0f);
        rotateGoal *= additionalRotation;


        transform.rotation = Quaternion.Lerp(transform.rotation, rotateGoal, WalkAroundSpeed * Time.deltaTime);

        transform.position = newPosition;

        // prepare to jump (switch to sneaky state)
        if (!prepared)
        {
            prepared = true;
            StartCoroutine("PrepareToJump");
        }

        if (PlayerInComboAttackRange())
        {
            // turn off the couroutine
            StopCoroutine("PrepareToJump");
            
            SwichState(State.ComboAttack);
        }

    }

    IEnumerator PrepareToJump()
    {
        float walkDuration = 5f;
        yield return new WaitForSeconds(walkDuration);
        prepared = false;

        SwichState(State.Sneaky);
    }

    // ---------------------------------------

    void DefaultLogic()
    {
        animator.SetBool("idle", true);
        animator.Play("Idle");
        animator.SetBool("attack", false);
        animator.SetBool("run to the bound", false);
    }
    // -----------------
        void IdleLogic()
    {
        //Debug.Log("Wolf enter Idle state");

        //animator logic
        animator.SetBool("idle", true);
        animator.SetBool("attack", false);

        animator.SetBool("run to the bound", false);

        // switch to run to the bound state
        Vector3 centerPoint = BatteRing.transform.position;

        Vector3 currentPosition = transform.position;

        centerPoint.y = currentPosition.y;

        distanceToCenter = Vector3.Distance(currentPosition, centerPoint);
 

        // switch from idle to combo attack:
        if (PlayerInComboAttackRange())
        {
            SwichState(State.ComboAttack);
        } else
        {
            // switch from idle to run to the bound
            if (distanceToCenter < walkAroundDistance - 1)
            {
                SwichState(State.RunToTheBound);
            }

            // switch from idle to walk around
            if (distanceToCenter >= walkAroundDistance)
            {
                SwichState(State.WalkAround);
            }
        }
    }

    // -------------------------------------
    public float runToTheBoundSpeed;
    void RunToTheBoundLogic()
    {
        Debug.Log("Wolf enter Run to the bound state");

        // animator logic
        animator.SetBool("idle", false);
        animator.SetBool("run to the bound", true);

        Vector3 centerPoint = BatteRing.transform.position;

        Vector3 currentPosition = transform.position;

        centerPoint.y = currentPosition.y;
            
        distanceToCenter = Vector3.Distance(currentPosition, centerPoint);

        // run forward until bound
        if (distanceToCenter >= walkAroundDistance) // if the distance is enough
        {
            // change to idle state
            SwichState(State.Idle);
        }
        else
        {
            Vector3 targetPosition = transform.position + transform.forward;

            this.transform.Translate(transform.forward * runToTheBoundSpeed * Time.deltaTime);
            transform.LookAt(targetPosition);
        }

        if (canDamage && PlayerInComboAttackRange())
        {
            canDamage = false;
            Player.GetComponent<Player>().TakeDamage(AttackDamage);
        }

    }
    // -------------------------------------
    public bool canDamage = false;
    public void CanDamage()
    {
        canDamage = true;
    }

    public void CannotDamage()
    {
        canDamage = false;
    }
    void ComboAttackLogic()
    {
        //Debug.Log("Wolf enter combo attack state");

        //animator logic
        animator.SetBool("idle", false);
        animator.SetBool("walk around", false);

        animator.SetBool("attack", true);

        // torward player
        Vector3 playerPos = Player.position;
        playerPos.y = transform.position.y;
        Quaternion rotateGoal = Quaternion.LookRotation((playerPos - transform.position).normalized);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotateGoal, WalkAroundSpeed * Time.deltaTime);

        if (PlayerInComboAttackRange())
        {
            if (canDamage)
            {
                canDamage = false;
                Player.GetComponent<Player>().TakeDamage(AttackDamage);
            }
        } else if (canSwitchState)
        {
            SwichState(State.Idle);
        }
    }

    //---------------------------------------------------

    void SneakyLogic()
    {
        //Debug.Log("Wolf enter Sneaky state");

        //animator logic
        animator.SetBool("walk around", false);
        animator.SetBool("jump", false);

        animator.SetBool("sneaky", true);

        // look at (rotate to) player
        Quaternion rotateGoal = Quaternion.LookRotation((Player.position - transform.position).normalized);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotateGoal, WalkAroundSpeed * Time.deltaTime);

        if (!prepared)
        {
            prepared = true;
            StartCoroutine("ReadyForJump"); // switch to jump state
        }

    }

    IEnumerator ReadyForJump ()
    {
        Debug.Log("about to jump");
        float sneakyDuration = 2f;
        yield return new WaitForSeconds(sneakyDuration);
        prepared = false; // maybe it can not switch so ...
        SwichState(State.JumpAttack);
    }


    bool canApproach = false;
    void JumpAttackLogic()
    {
        //Debug.Log("Wolf enter Jump Attack state");
        //animator logic
        animator.SetBool("sneaky", false);
        animator.SetBool("jump", true);

        SwichState(State.Idle);

        if (canApproach)
        {
            ApproachPlayer();
        }

        if (canDamage && DistanceToPlayer() <= 2f)
        {
            Player.GetComponent<Player>().TakeDamage(AttackDamage);
            canDamage = false;
        }
    }

    public void CanApproach()
    {
        canApproach = true;
    }

    public void CannotApproach()
    {
        canApproach = false;
    }




    //--------------------------------------BEHAVIOUR---------------------------------------

    public Transform Player;
    public float ComboAttackRange;
    bool PlayerInComboAttackRange ()
    {
        if (DistanceToPlayer() <= ComboAttackRange - 0.5f)
        {
            return true;
        }
        return false;
    }

    float DistanceToPlayer()
    {
        Vector3 playerPos = Player.position;
        playerPos.y = transform.position.y;
        float distanceToPlayer = Vector3.Distance(transform.position, playerPos);

        //Debug.Log(distanceToPlayer);

        return distanceToPlayer;
    }

    public void ApproachPlayer()
    {
        Vector3 playerPos = Player.transform.position;
        float moveSpeed = 5f; 
        //transform.position = Vector3.MoveTowards(transform.position, playerPos, moveSpeed * Time.deltaTime);

        transform.position = Vector3.Lerp(transform.position, playerPos, moveSpeed * Time.deltaTime);

        //transform.position = Vector3.SmoothDamp(transform.position, playerPos, ref velocity, smoothTime);

    }

    //-----------------------------------STATE FUNCTION----------------------------------------------
    public void SwichState(State state)
    {
        prepared = false; // for walkaround logic

        if (state == State.Default)
        {
            currentState = state;
            canSwitchState = false;
            return;
        }

        if (canSwitchState)
        {
            canSwitchState = false;

            currentState = state;
        }
    }

    void CanSwitchState()
    {
        if (!canSwitchState)
        {
            canSwitchState = true;
            Debug.Log("can switch state now");
        }
    }

    void CannotSwitchState()
    {
        if (canSwitchState)
        {
            canSwitchState = false;
            Debug.Log("can not switch state now");
        }
    }
}
