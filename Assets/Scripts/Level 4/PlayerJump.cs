using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerJump : MonoBehaviour
{
    public bool canJump = false;
    public int boneCount = 0;

    public float jumpHeight = 2f; 

    private CharacterController controller;
    private float gravity = -9.81f;
    private float verticalVelocity;
    float distanceToGround;

    public float distanceFromGround;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        distanceToGround = controller.bounds.extents.y;
    }

    void Update()
    {
        if (boneCount >= 2)
        {
            canJump = true;
        }
        bool isCloseToGround = Physics.Raycast(transform.position, Vector3.down, distanceToGround + distanceFromGround);

        if (canJump && Input.GetKeyDown(KeyCode.Space) && isCloseToGround)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -3f * gravity);
        }
    }
    private void FixedUpdate()
    {
        if (canJump)
        {
            verticalVelocity += gravity * Time.deltaTime;

            Vector3 move = Vector3.up * verticalVelocity * Time.deltaTime;

            controller.Move(move);
        }
    }

    public void PlusBone()
    {
        boneCount++;
        if (boneCount == 3)
        {
            GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Level Up");

        }
    }
}
