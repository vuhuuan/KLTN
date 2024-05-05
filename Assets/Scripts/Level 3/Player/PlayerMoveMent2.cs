
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    public float speed = 5f;
    [SerializeField] private float rotationSpeed = 700f;
    [SerializeField] private Camera mainCamera;
    private CharacterController characterController;
    //private float gravity = -9.81f;
    //[SerializeField] private float gravityMultiplier = 1;
    private float verticalVelocity = 0f;


    public bool isWalking;

    public bool isRunning = false;

    [HideInInspector]
    public bool isAttacking = false;

    [SerializeField] private Transform attackRange;


    [HideInInspector]
    public bool isSneaking = false;

    private Animator animator;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        CheckBound();
        ApplyGravity();

        MoveCharacter();

        CheckAttackInput();

        CheckRollInput();

        if (Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            characterController.height = 0.43f;
            characterController.center = new Vector3(0, 0.2f, 0);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            characterController.height = 0.83f;
            characterController.center = new Vector3(0, 0.4f, 0);
        }
        //UpdateAnimatorParameters();
    }

    void ApplyGravity()
    {
        if (characterController.isGrounded)
        {
            verticalVelocity = -1f;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        Vector3 gravity = new Vector3(0, verticalVelocity, 0);
        characterController.Move(gravity * Time.deltaTime);
    }

    void MoveCharacter()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Get the forward and right vectors of the camera
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // Calculate the desired movement direction
        Vector3 desiredMoveDirection = forward * vertical + right * horizontal;

        // Normalize the movement direction to prevent faster movement diagonally
        if (desiredMoveDirection.magnitude >= 0.1f)
        {
            desiredMoveDirection.Normalize();

            // camera rotate
            UpdateRotation(desiredMoveDirection);

            // self rotate left, right
            RotateCharacter(horizontal);


            UpdateMovement(desiredMoveDirection);

            // speed up if ctrl
            CheckRunInput();

            CheckSneakingInput();

        }
        else
        {

            ResetMovementStates();
        }
    }

    void UpdateMovement(Vector3 desiredMoveDirection)
    {
        float currentSpeed = isRunning ? speed * 1.5f : speed;
        Vector3 movement = desiredMoveDirection * currentSpeed * Time.deltaTime;
        characterController.Move(movement);

        if (!isWalking)
        {
            isWalking = true;
            animator.SetBool("walking", true);
        }
    }

    void UpdateRotation(Vector3 desiredMoveDirection)
    {
        Quaternion toRotation = Quaternion.LookRotation(desiredMoveDirection, Vector3.up);
        float angle = Vector3.Angle(transform.forward, desiredMoveDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
    }

    void RotateCharacter(float horizontal)
    {
        //float horizontalRotation = Input.GetAxis("Horizontal");
        Vector3 rotationDirection = new Vector3(0f, horizontal, 0f);
        transform.rotation *= Quaternion.Euler(rotationDirection);
    }

    void ResetMovementStates()
    {
        if (isWalking || isRunning || isSneaking)
        {
            isWalking = false;
            isRunning = false;
            isSneaking = false;

            animator.SetBool("walking", false);
            animator.SetBool("running", false);
            animator.SetBool("sneaky", false);
        }
    }

    void CheckRunInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        if (Input.GetKey(KeyCode.LeftControl) && direction.magnitude != 0)
        {
                GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Dog Run");
            if (!isRunning)
            {
                isRunning = true;
                animator.SetBool("running", true);

                isWalking = false;
                //animator.SetBool("walking", false);

                isSneaking = false;
                animator.SetBool("sneaky", false);
            }
        }
        else
        {
            if (isRunning)
            {
                isRunning = false;
                animator.SetBool("running", false);
            }
        }
    }

    void CheckSneakingInput()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {

            if (!isSneaking)
            {
                isRunning = false;
                animator.SetBool("running", false);

                isWalking = false;
                //animator.SetBool("walking", false);

                isSneaking = true;
                animator.SetBool("sneaky", true);

            }
        }
        else
        {
            if (isSneaking)
            {
                isSneaking = false;
                animator.SetBool("sneaky", false);

            }

        }
    }

    bool canAttack = true;
    void CheckAttackInput()
    {
        if (!canAttack) { return; }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !GameObject.Find("Pause Screen"))
        {

            isAttacking = true;


            animator.ResetTrigger("attack");
            animator.SetTrigger("attack");

            StartCoroutine("AttackAnimTimeOut");
            StartCoroutine("AttackTimeOut");
            GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Dog Attack");

        }
        else
        {
            isAttacking = false;
            attackRange.gameObject.SetActive(false);
        }
    }
    IEnumerator AttackAnimTimeOut()
    {
        yield return new WaitForSeconds(0.3f);
        attackRange.gameObject.SetActive(true);
    }

    IEnumerator AttackTimeOut()
    {
        canAttack = false;
        yield return new WaitForSeconds(0.5f);
        isAttacking = true;
        canAttack = true;
    }


    bool canRoll = true;
    public bool isRolling = false;

    void CheckRollInput()
    {
        if (!canRoll) { return; }

        if (Input.GetKeyDown(KeyCode.Mouse1) && !GameObject.Find("Pause Screen"))
        {

            isRolling = true;

            animator.ResetTrigger("roll");
            animator.SetTrigger("roll");

            StartCoroutine("RollTimeOut");
        }
        else
        {
            isRolling = false;
            animator.ResetTrigger("roll");
        }
    }
    IEnumerator RollTimeOut()
    {
        canRoll = false;
        yield return new WaitForSeconds(0.5f);
        isRolling = true;
        canRoll = true;
    }



    void CheckBound()
    {
        float z1Range = 217f;
        float z2Range = 347f;

        if (transform.position.z < z1Range)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, z1Range);
        }

        if (transform.position.z > z2Range)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, z2Range);
        }
    }
}
