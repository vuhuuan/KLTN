using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RabbitAI : MonoBehaviour
{
    // Start is called before the first frame update

    private enum RabbitState
    {
        Chilling,
        Running
    }

    private RabbitState currentState;
    private Rabbit rabbit;

    private Animator animator;

    Transform player;

    [SerializeField] private float jumpForce = 5f;

    private Rigidbody rb;

    void Start()
    {
        player = GameObject.Find("Mick3 Player").gameObject.transform;
        animator = GetComponent<Animator>();
        currentState = RabbitState.Chilling;

        rb = GetComponent<Rigidbody>();
        rabbit = GetComponent<Rabbit>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case RabbitState.Chilling:
                ChillingState();
                break;

            case RabbitState.Running:
                RunningState();
                break;
        }
    }

    private bool getBored = true;

    [SerializeField] private float dangerDis = 2f;
    [SerializeField] private float nearDangerDis = 5f;

    void ChillingState()
    {
        PlayIdleAnim();
        //Debug.Log("Rabbit is chilling");

        bool isDanger = IsDanger();

        if (isDanger)
        {
            currentState = RabbitState.Running;
            return;
        }

        
        if (getBored)
        {
            StartCoroutine("BoringSoJump");
            getBored = false;
        }
    }

    bool canJump = true;
    [SerializeField] private float coolDownJump = 0.4f;
    void RunningState()
    {
        bool isDanger = IsDanger();
        if (!isDanger)
        {
            currentState = RabbitState.Chilling;
            return;
        }

        bool isGrounded = IsGrounded();

        if (!isGrounded )
        {
            return;
        }

        if (canJump)
        {
            canJump = false;

            PlayIdleAnim();

            RotateAgainstPlayer();

            PlayJumpAnim();

            Jump();

            StartCoroutine("WaitForNextJump");
            //Debug.Log("Rabbit is running");
        }
    }

    void PlayIdleAnim()
    {
        animator.SetBool("idle", true);
        animator.SetBool("jump", false);
        //Debug.Log("Rabbit is Idle");
    }

    
    void PlayJumpAnim()
    {

        animator.SetBool("idle", false);
        animator.SetBool("jump", true);
        //Debug.Log("Rabbit is Jump");

    }

    void Jump()
    {
        if (!rabbit.Dead)
        {
            jumpForce = Random.Range(2f, 3f);
            Vector3 forwardDirection = transform.forward.normalized;

            Vector2 jumpDirection = new Vector2(forwardDirection.x, forwardDirection.z).normalized;

            Vector2 jumpVelocity = jumpDirection * jumpForce;
            rb.AddForce(new Vector3(jumpVelocity.x, jumpForce, jumpVelocity.y), ForceMode.Impulse);
        }
    }

    IEnumerator WaitForNextJump()
    {
        canJump = false;
        yield return new WaitForSeconds(coolDownJump);
        canJump = true;
    } 

    bool IsGrounded()
    {
        float raycastDistance = (GetComponent<BoxCollider>().size.y / 2);
        RaycastHit hit;

        if (!Physics.Raycast(GetComponent<BoxCollider>().bounds.center, -transform.up, out hit, raycastDistance))
        {
            return false;
        }

        if(!hit.collider.CompareTag("Ground")) return false;

        return true;
    }


    IEnumerator BoringSoJump()
    {
        int boringTime = Random.Range(3, 10);
        yield return new WaitForSeconds(boringTime);

        RotateRandomDirection(0f, 360f);
        PlayJumpAnim();
        Jump();
        getBored = true;
    }

    void RotateRandomDirection(float minAngle, float maxAngle)
    {
        Vector3 randomRotation = new Vector3(0f, Random.Range(minAngle, maxAngle), 0f);
        Quaternion targetRotation = Quaternion.Euler(randomRotation);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 2f);
    }

    void RotateAgainstPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position);

        float randomAngleX = Random.Range(0f, 90f);
        float randomAngleZ = Random.Range(0f, 90f);

        Quaternion randomRotation = Quaternion.Euler(randomAngleX, 0f, randomAngleZ);

        directionToPlayer = randomRotation * directionToPlayer;

        directionToPlayer.y = 0f;

        Quaternion oppositeRotate = Quaternion.LookRotation(-directionToPlayer);

        transform.rotation = oppositeRotate;
    }

    bool IsPlayerClose(float distanceThreshold)
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        return distanceToPlayer <= distanceThreshold;
    }

    bool IsDanger()
    {
        if (IsPlayerClose(dangerDis) || (!player.GetComponent<PlayerMovement2>().isSneaking) && IsPlayerClose(nearDangerDis)) return true;
        return false;
    }
}
