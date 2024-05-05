using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 700f;
    [SerializeField] private Camera mainCamera;
    private CharacterController characterController;
    //private float gravity = -9.81f;
    private float verticalVelocity = 0f;

    public bool isRunning = false;

    private Animator animator;

    [HideInInspector]
    public bool isAttacking = false;

    [HideInInspector]
    public bool isSneaking = false;

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
        RotateCharacter();
        CheckRunInput();
        CheckSneakyInput();
        CheckAttackInput();
        UpdateAnimatorParameters();
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
        if (desiredMoveDirection.magnitude > 1f)
        {
            desiredMoveDirection.Normalize();
            animator.SetBool("walking", true);
        } else
        {
            animator.SetBool("walking", false);
            animator.SetBool("running", false);
        }

        // Determine the speed based on whether the player is running or walking
        float currentSpeed = isRunning ? speed * 1.5f : speed;

        // Move the character
        Vector3 movement = desiredMoveDirection * currentSpeed * Time.deltaTime;
        characterController.Move(movement);

        // Rotate the character to face the same direction as the camera
        if (desiredMoveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(desiredMoveDirection, Vector3.up);
            float angle = Vector3.Angle(transform.forward, desiredMoveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void RotateCharacter()
    {
        // Rotate the character based on input (if you want rotation)
        float horizontalRotation = Input.GetAxis("Horizontal");
        Vector3 rotationDirection = new Vector3(0f, horizontalRotation, 0f);
        transform.rotation *= Quaternion.Euler(rotationDirection);
    }

    void CheckRunInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (Input.GetKey(KeyCode.LeftControl) && direction.magnitude != 0)
        {
            isRunning = true;
            GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Dog Run");
        }
        else
        {
            isRunning = false;
        }

    }

    //float attackTimeOut = 1f;
    bool canAttack = true;
    void CheckAttackInput()
    {
        if (!canAttack) { return; }


        if (Input.GetKeyDown(KeyCode.Mouse0) && !GameObject.Find("Pause Screen"))
        {
            StartCoroutine("AttackAnimTimeOut");
        }
        else
        {
            isAttacking = false;
            animator.ResetTrigger("attack");

        }
    }
    IEnumerator AttackAnimTimeOut () 
    {
        canAttack = false;
        animator.ResetTrigger("attack");
        animator.SetTrigger("attack");
        yield return new WaitForSeconds(0.5f);
        isAttacking = true;
        canAttack = true;

    }

    void CheckSneakyInput()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = false;
            isSneaking = true;
            characterController.radius = 0.2f;
            characterController.height = 0.3f;

            animator.SetBool("sneaky", true);

            animator.SetBool("walking", false);
            animator.SetBool("running", false);
        }
        else
        {
            animator.SetBool("sneaky", false);
            characterController.radius = 0.38f;
            characterController.height = 0.8f;
            isSneaking = false;
        }
    }

    void UpdateAnimatorParameters()
    {
        bool isMoving = Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f;

        animator.SetBool("walking", isMoving && !isRunning);
        animator.SetBool("running", isMoving && isRunning);
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
