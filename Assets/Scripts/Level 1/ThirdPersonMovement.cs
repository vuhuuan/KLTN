using System.Collections;
using System.Collections.Generic;
using UnityEditor;
//using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.UI;

public class ThirdPersonMovement : MonoBehaviour
{

    CharacterController characterController;
    [SerializeField] private float walkSpeed = 12f;
    [SerializeField] private float runSpeed = 24f;

    private float speed;
    public bool isRunning;

    float turnSmoothVelocity;

    [SerializeField] private float turnSmoothTime = 0.01f;

    [SerializeField] private Transform cam;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        characterController= GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        speed = walkSpeed;
        isRunning = false;

        if (cam == null)
        {
            cam = GameObject.Find("Main Camera").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (Input.GetKey(KeyCode.LeftControl) && direction.magnitude != 0)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

    }

    private void FixedUpdate()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude != 0)
        {
            if (isRunning)
            {
                speed = runSpeed;
                animator.SetBool("running", true);
                animator.SetBool("walking", false);
                GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Dog Run");
            }
            else
            {
                speed = walkSpeed;
                animator.SetBool("running", false);
                animator.SetBool("walking", true);
            }
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            moveDir.y -= (9.81f * 9.81f) * Time.deltaTime;

            characterController.Move(moveDir.normalized * speed * Time.deltaTime);

        }
        else
        {
            animator.SetBool("walking", false);
        }

    }

}
