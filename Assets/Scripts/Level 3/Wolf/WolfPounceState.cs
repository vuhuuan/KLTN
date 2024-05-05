using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WolfPounceState : WolfBaseState
{
    private float jumpForce = 40f;
    private float jumpCoolDown = 5f;
    private bool canJump = true;
    WolfStateManager wolf;
    float calmCoolDownTime = 3f;

    public override void EnterState(WolfStateManager wolf)
    {
        Debug.Log("Wolf is Pouncing");
        this.wolf = wolf;
    }

    public override void UpdateState(WolfStateManager wolf)
    {
        Jump(wolf);

        if (!wolf.PlayerInPoundRange())
        {
            StartCoroutine(WaitBeforeSwitchState(wolf.ChaseState));
        }
    }

    public override void ExitState(WolfStateManager wolf)
    {

    }

    public override void PlayAnimation(WolfStateManager wolf)
    {
        wolf.animator.SetBool("isRunning", false);

        wolf.animator.SetBool("isIdling", false);
        wolf.animator.SetBool("isWalking", false);
        wolf.animator.SetBool("isSneaking", false);

        wolf.animator.ResetTrigger("attack1");
        wolf.animator.SetTrigger("attack1");
    }

    public void PlayAdditionAnimation(WolfStateManager wolf)
    { 
        wolf.animator.ResetTrigger("attack1");
        wolf.animator.SetBool("isSneaking", true);
    }


    void Jump(WolfStateManager wolf)
    {
        if (canJump && IsGrounded(wolf)) {
            StartCoroutine(WaitCoolDownJump());
            PlayAnimation(wolf);


            Vector3 forwardDirection = wolf.transform.forward.normalized;
            Vector2 jumpDirection = new Vector2(forwardDirection.x, forwardDirection.z).normalized;
            Vector2 jumpVelocity = jumpDirection * jumpForce;
            wolf.GetComponent<Rigidbody>().AddForce(new Vector3(jumpVelocity.x, jumpForce / 2, jumpVelocity.y), ForceMode.Impulse);

            GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Wolf Roar");

        }
        else if (IsGrounded(wolf))
        {
            LookAtPlayer();
            //if (canPlayAdditionAnim)
            //{
            //    StartCoroutine(WaitAdditionAnimation());
            //}
        }
    }

    IEnumerator WaitCoolDownJump()
    {
        canJump = false;
        yield return new WaitForSeconds(jumpCoolDown);
        canJump = true;
    }

    IEnumerator WaitAdditionAnimation()
    {
        yield return new WaitForSeconds(2f);
        PlayAdditionAnimation(wolf);
    }

    IEnumerator WaitBeforeSwitchState(WolfBaseState state)
    {
        yield return new WaitForSeconds(calmCoolDownTime);
        wolf.SwitchState(state);
    }

    bool IsGrounded(WolfStateManager wolf)
    {
        float raycastDistance = (wolf.GetComponent<BoxCollider>().size.y) + 0.3f;
        RaycastHit hit;

        Debug.DrawRay(wolf.GetComponent<BoxCollider>().bounds.center, -wolf.transform.up * raycastDistance, Color.red);

        if (!Physics.Raycast(wolf.GetComponent<BoxCollider>().bounds.center, -wolf.transform.up, out hit, raycastDistance))
        {
            return false;
        }

        if (!hit.collider.CompareTag("Ground")) return false;

        return true;
    }

    private void LookAtPlayer()
    {
        Vector3 direction = wolf.GetPlayerDirection();
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        wolf.transform.rotation = Quaternion.Lerp(wolf.transform.rotation, targetRotation, 200f * Time.deltaTime);
    }

}
