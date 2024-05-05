using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Build;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class WolfChaseState : WolfBaseState
{
    public float chaseSpeed;
    private float turnSpeed = 200f;

    public override void EnterState(WolfStateManager wolf)
    {
        Debug.Log("Wolf is chasing player");

        PlayAnimation(wolf);
    }

    public override void UpdateState(WolfStateManager wolf)
    {
        ChasePlayer(wolf);

        if (wolf.PlayerInPoundRange())
        {
            wolf.SwitchState(wolf.PounceState);
        }

    }

    public override void ExitState(WolfStateManager wolf)
    {

    }
    public override void PlayAnimation(WolfStateManager wolf)
    {
        wolf.animator.SetBool("isRunning", true);

        wolf.animator.SetBool("isIdling", false);
        wolf.animator.SetBool("isWalking", false);
        wolf.animator.SetBool("isSneaking", false);

        wolf.animator.ResetTrigger("attack1");
    }

    private void ChasePlayer(WolfStateManager wolf)
    {
        Vector3 direction = wolf.GetPlayerDirection();
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        wolf.transform.rotation = Quaternion.Lerp(wolf.transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        wolf.transform.Translate(direction.normalized * chaseSpeed * Time.deltaTime, Space.World);
    }
}
