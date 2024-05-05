using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WolfIdleState : WolfBaseState
{
    private float calmCoolDownTime = 0.6f;
    private bool canSwitch = true;
    WolfStateManager wolf;
    public override void EnterState(WolfStateManager wolf)
    {
        Debug.Log("Wolf is Idling");
        //wolf.animator.Play("Idle");
        PlayAnimation(wolf);

        this.wolf = wolf;
    }

    public override void UpdateState(WolfStateManager wolf)
    {
        LookAtPlayer();
        if (canSwitch && wolf.PlayerInPoundRange())
        {
            StartCoroutine(WaitBeforeSwitchState(wolf.PounceState));
        } 
        else if (canSwitch && !wolf.PlayerInPoundRange())
        {
            wolf.SwitchState(wolf.ChaseState);
        }
    }

    public override void ExitState(WolfStateManager wolf)
    {

    }
    public override void PlayAnimation(WolfStateManager wolf)
    {
        wolf.animator.SetBool("isIdling", true);

        wolf.animator.SetBool("isWalking", false);
        wolf.animator.SetBool("isRunning", false);
        wolf.animator.SetBool("isSneaking", false);

        wolf.animator.ResetTrigger("attack1");
    }

    IEnumerator WaitBeforeSwitchState(WolfBaseState state)
    {
        canSwitch = false;
        yield return new WaitForSeconds(calmCoolDownTime);
        canSwitch = true;
        wolf.SwitchState(state);
    }

    private void LookAtPlayer()
    {
        Vector3 direction = wolf.GetPlayerDirection();
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        wolf.transform.rotation = Quaternion.Lerp(wolf.transform.rotation, targetRotation, 200f * Time.deltaTime);
    }
}
